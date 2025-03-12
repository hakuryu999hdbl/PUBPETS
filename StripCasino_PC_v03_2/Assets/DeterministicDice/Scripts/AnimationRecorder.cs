using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MADD
{
    public class AnimationRecorder : MonoBehaviour
    {
        #region member variables

        public int recordingFrameLength = 10 * 50; //In frames
        public float _animationSpeed = 10f;
        public List<GameObject> objectsToRecord;
        public List<RecordingData> recordingDataList;
        public Action OnAnimationCompleted;

        private DiceManager _diceManager;
        private Coroutine playback = null;

        #endregion

        void Start()
        {
            _diceManager = GetComponent<DiceManager>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F11))
            {
                PlayRecording();
            }
        }

        public void SetAnimaitonSpeed(float speed)
        {
            if (speed <= 0)
                speed = 0.1f;
            _animationSpeed = speed;
        }

        public void StartSimulation(List<GameObject> targets)
        {
            if (playback != null)
            {
                StopCoroutine(playback);
                playback = null;
            }

            recordingDataList.Clear();
            objectsToRecord.Clear();
            objectsToRecord = targets;

            EnablePhysics();
            GetInitialState();
            StartRecording();
        }

        private void GetInitialState()
        {
            foreach (var gameObject in objectsToRecord)
            {
                Vector3 initialPosition = gameObject.transform.position;
                Quaternion initialRotation = gameObject.transform.rotation;

                Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                rb.maxAngularVelocity = 1000;

                RecordingData data = new RecordingData(rb, initialPosition, initialRotation);
                recordingDataList.Add(data);
            }
        }

        private void StartRecording()
        {
            Physics.simulationMode = SimulationMode.Script;

            //Begin recording position and rotation for every frame
            for (int i = 0; i < recordingFrameLength; i++)
            {
                //For every gameObject
                for (int j = 0; j < objectsToRecord.Count; j++)
                {
                    Vector3 position = objectsToRecord[j].transform.position;
                    Quaternion rotation = objectsToRecord[j].transform.rotation;
                    bool isColliding = _diceManager._diceDataList[j].diceUI.isColliding;
                    bool isNotMoving = CheckObjectHasStopped(_diceManager._diceDataList[j].rb);

                    // add more gravity if we are going down, this helps with the realism of the scene
                    Rigidbody rb = objectsToRecord[j].GetComponent<Rigidbody>();
                    if (isNotMoving && rb.velocity.y < 0)
                    {
                        rb.AddForce(Physics.gravity * 1.2f, ForceMode.Acceleration);
                    }

                    RecordedFrame frame = new RecordedFrame(
                        position,
                        rotation,
                        isColliding,
                        isNotMoving
                    );
                    recordingDataList[j].recordedAnimation.Add(frame);
                }
                Physics.Simulate(Time.fixedDeltaTime);
            }

            Physics.simulationMode = SimulationMode.FixedUpdate;
        }

        public void PlayRecording()
        {
            if (playback == null && recordingDataList.Count > 0)
            {
                playback = StartCoroutine(PlayAnimation());
            }
        }

        private IEnumerator PlayAnimation()
        {
            DisablePhysics();
            ResetToInitialState();

            bool completed = false;

            //Play the animation frame by frame
            for (int i = 0; i < recordingFrameLength; i++)
            {
                bool allStopped = i > 100;
                //For every objects
                for (int j = 0; j < recordingDataList.Count; j++)
                {
                    Vector3 position = recordingDataList[j].recordedAnimation[i].position;
                    Quaternion rotation = recordingDataList[j].recordedAnimation[i].rotation;
                    objectsToRecord[j].transform.position = position;
                    objectsToRecord[j].transform.rotation = rotation;

                    //Play Sound whenever contact happens
                    if (recordingDataList[j].recordedAnimation[i].isColliding)
                    {
                        _diceManager._diceDataList[j].diceUI.PlayCollisionSound();
                    }

                    if (!recordingDataList[j].recordedAnimation[i].isNotMoving)
                        allStopped = false;
                }
                // if all dice have stopped, we stop the animation and invoke the event
                if (allStopped && !completed)
                {
                    completed = true;
                    OnAnimationCompleted?.Invoke();
                    StopRecording();
                    break;
                }

                yield return new WaitForSeconds(Time.fixedDeltaTime / _animationSpeed);
            }

            playback = null;
        }

        public void StopRecording()
        {
            recordingDataList.Clear();
            objectsToRecord.Clear();
        }

        /// <summary>
        /// For optimization, this function is to check if the dice has stopped moving.
        /// We can then stop recording this dice.
        /// </summary>
        /// <param name="rb"></param>
        /// <returns></returns>
        public bool CheckObjectHasStopped(Rigidbody rb)
        {
            if (rb.velocity == Vector3.zero && rb.angularVelocity == Vector3.zero)
            {
                return true;
            }
            else
                return false;
        }

        public void ResetToInitialState()
        {
            for (int i = 0; i < objectsToRecord.Count; i++)
            {
                objectsToRecord[i].transform.position = recordingDataList[i].initialPosition;
                objectsToRecord[i].transform.rotation = recordingDataList[i].initialRotation;
            }
        }

        public void EnablePhysics()
        {
            //Enable Rigidbody
            for (int i = 0; i < recordingDataList.Count; i++)
            {
                recordingDataList[i].rb.useGravity = true;
                recordingDataList[i].rb.isKinematic = false;
            }
        }

        public void DisablePhysics()
        {
            //Disable Rigidbody
            for (int i = 0; i < recordingDataList.Count; i++)
            {
                recordingDataList[i].rb.useGravity = false;
                recordingDataList[i].rb.isKinematic = true;
            }
        }

        [System.Serializable]
        public struct RecordedFrame
        {
            public Vector3 position;
            public Quaternion rotation;
            public bool isColliding;
            public bool isNotMoving;

            public RecordedFrame(
                Vector3 position,
                Quaternion rotation,
                bool isColliding,
                bool isNotMoving
            )
            {
                this.position = position;
                this.rotation = rotation;
                this.isColliding = isColliding;
                this.isNotMoving = isNotMoving;
            }
        }

        [System.Serializable]
        public struct RecordingData
        {
            public Rigidbody rb;
            public Vector3 initialPosition;
            public Quaternion initialRotation;
            public List<RecordedFrame> recordedAnimation;

            public RecordingData(Rigidbody rb, Vector3 initialPosition, Quaternion initialRotation)
            {
                this.rb = rb;
                this.initialPosition = initialPosition;
                this.initialRotation = initialRotation;
                this.recordedAnimation = new List<RecordedFrame>();
            }
        }
    }
}
