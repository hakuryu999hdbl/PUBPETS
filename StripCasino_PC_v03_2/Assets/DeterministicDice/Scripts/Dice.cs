using UnityEditor;
using UnityEngine;

namespace MADD
{
    public class Dice : MonoBehaviour
    {
        public float _transitionTime = 0.5f;

        [Header("References")]
        public Transform[] _fakeFacePositions;
        public Transform[] _actualFacePositions;

        [Header("Debug")]
        public int defaultFaceResult = -1;
        public int alteredFaceResult = -1;

        public void Reset()
        {
            _animationRecorder = FindObjectOfType<AnimationRecorder>();
            defaultFaceResult = -1;
            alteredFaceResult = -1;
        }

        private AnimationRecorder _animationRecorder;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F12))
            {
                defaultFaceResult = FindFaceResult();
                RotateDice(alteredFaceResult);
            }
            if (defaultFaceResult != -1 && alteredFaceResult != -1)
            {
                RotateDice(alteredFaceResult);
            }
        }

        public int Faces()
        {
            return _fakeFacePositions.Length;
        }

        /// <summary>
        /// Rotate the dice from the defaultFaceResult to alteredFaceResult
        /// </summary>
        /// <param name="alteredFaceResult"></param>
        public void RotateDice(int alteredFaceResult)
        {
            if (alteredFaceResult < _actualFacePositions.Length)
            {
                this.alteredFaceResult = alteredFaceResult;
                Transform currentlyUpFace = _actualFacePositions[defaultFaceResult];
                Transform targetUpFace = _fakeFacePositions[alteredFaceResult];
                Quaternion targetRotation =
                    currentlyUpFace.rotation * Quaternion.Inverse(targetUpFace.rotation);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation * transform.rotation,
                    Time.deltaTime / _transitionTime * _animationRecorder._animationSpeed
                );
            }
        }

        public int FindFaceResult()
        {
            int maxIndex = 0;
            for (int i = 1; i < _actualFacePositions.Length; i++)
            {
                if (_actualFacePositions[maxIndex].position.y < _actualFacePositions[i].position.y)
                {
                    maxIndex = i;
                }
            }
            defaultFaceResult = maxIndex;
            return maxIndex;
        }

        public Transform FindTopFace()
        {
            int maxIndex = 0;
            for (int i = 1; i < _actualFacePositions.Length; i++)
            {
                if (_fakeFacePositions[maxIndex].position.y < _fakeFacePositions[i].position.y)
                {
                    maxIndex = i;
                }
            }
            return _fakeFacePositions[maxIndex];
        }
#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            foreach (var face in _actualFacePositions)
            {
                if (face == null)
                {
                    continue;
                }
                Gizmos.DrawLine(face.position, face.position + face.up);
            }
            ;

            Gizmos.color = Color.green;
            foreach (var face in _actualFacePositions)
            {
                if (face == null)
                {
                    continue;
                }
                Gizmos.DrawLine(face.position, face.position + face.up);
                // draw face position as text
                GUIStyle style = new GUIStyle();
                style.fontSize = 20;
                style.normal.textColor = Color.green;
                Handles.Label(face.position + face.up, face.name, style);
            }
            ;
        }
#endif
    }
}
