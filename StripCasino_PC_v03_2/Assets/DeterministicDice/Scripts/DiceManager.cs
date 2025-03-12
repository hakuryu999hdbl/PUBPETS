using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MADD
{
    public class DiceManager : MonoBehaviour
    {
        #region member variables

        public UnityEvent<int> OnRollInstant,
            OnRoll;

        public List<GameObject> _dicePrefabs;

        public List<DiceSetup> _diceSetup;

        [HideInInspector]
        public AnimationRecorder _animRecorder;

        [HideInInspector]
        public List<DiceData> _diceDataList;

        private List<GameObject> _instantiatedDice = new List<GameObject>();
        private int _lastResult;

        #endregion

        private void Start()
        {
            _animRecorder = GetComponent<AnimationRecorder>();
        }

        void OnEnable()
        {
            if (!_animRecorder)
                _animRecorder = GetComponent<AnimationRecorder>();

            _animRecorder.OnAnimationCompleted += () => OnRoll?.Invoke(_lastResult);
        }

        void OnDisable()
        {
            if (!_animRecorder)
                _animRecorder = GetComponent<AnimationRecorder>();

            _animRecorder.OnAnimationCompleted -= () => OnRoll?.Invoke(_lastResult);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Roll();
            }
        }

        #region public API

        /// <summary>
        /// Roll the dice
        /// </summary>
        /// <returns>
        ///    The result of the dice roll
        /// </returns>
        public int Roll()
        {
            GenerateDice();

            //Generate list of dices, then put it into the simulation
            List<GameObject> diceList = new List<GameObject>();
            for (int i = 0; i < _diceSetup.Count; i++)
            {
                diceList.Add(_diceDataList[i].diceObject);
            }
            _animRecorder.StartSimulation(diceList);

            //Record the dice roll result
            for (int i = 0; i < _diceSetup.Count; i++)
            {
                int predictedResult = _diceDataList[i].diceLogic.FindFaceResult();
                // print(_diceDataList[i].diceObject.name + " - " + (result + 1).ToString());
            }

            //Reset and alter the result
            _animRecorder.ResetToInitialState();
            for (int i = 0; i < _diceSetup.Count; i++)
            {
                int faces = _diceDataList[i]._setup.faces;
                _diceDataList[i]
                    .diceLogic.RotateDice(
                        _diceDataList[i]._setup.result > 0
                            ? _diceDataList[i]._setup.result - 1
                            : Random.Range(0, faces)
                    );
            }

            _animRecorder.PlayRecording();

            // get all the results
            int result = 0;
            for (int i = 0; i < _diceSetup.Count; i++)
            {
                Dice dice = _diceDataList[i].diceLogic;
                result += dice.alteredFaceResult + 1;
            }

            _lastResult = result;
            OnRollInstant?.Invoke(_lastResult);

            return _lastResult;
        }

        /// <summary>
        /// Reset the dice to the initial state
        /// </summary>
        public void Reset()
        {
            _animRecorder.ResetToInitialState();
            GenerateDice();
        }

        public void RemoveDice(int number, int amount)
        {
            bool removeAll = amount < 0;

            for (int i = 0; i < _diceSetup.Count; i++)
            {
                if (removeAll || _diceSetup[i].faces == number)
                {
                    Destroy(_diceDataList[i].diceObject);
                    _diceDataList.RemoveAt(i);
                    _diceSetup.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// Add a dice to the dice manager
        /// </summary>
        /// <param name="number">
        ///     Number of faces of the dice
        /// </param>
        /// <param name="amount">
        ///     Amount of dices to add
        /// </param>
        /// <returns>
        ///     List of DiceSetup objects that were added
        /// </returns>
        public List<DiceSetup> AddDice(int number, int amount)
        {
            List<DiceSetup> dices = new List<DiceSetup>();

            for (int i = 0; i < amount; i++)
            {
                DiceSetup newDice = new DiceSetup();
                newDice.faces = number;
                newDice.result = -1;
                newDice.simulate = true;
                _diceSetup.Add(newDice);
                dices.Add(newDice);
            }

            return dices;
        }

        /// <summary>
        /// Pause a number of dice from the simulation
        /// </summary>
        /// <param name="number">
        ///     Number of faces of the dice
        /// </param>
        /// <param name="amount">
        ///     Amount of dices to pause. If -1, pause all dices with the given number of faces
        /// </param>
        /// <returns>
        ///     List of Dice objects that were paused
        /// </returns>
        public List<Dice> PauseDice(int number, int amount)
        {
            List<Dice> dices = new List<Dice>();
            int paused = 0;
            for (int i = 0; i < _diceSetup.Count; i++)
            {
                if (_diceSetup[i].faces == number && (paused < amount || amount < 0))
                {
                    _diceDataList[i].rb.isKinematic = true;
                    _diceDataList[i].rb.useGravity = false;
                    _diceDataList[i]._setup.simulate = false;
                    paused++;
                    dices.Add(_diceDataList[i].diceLogic);
                }
            }

            return dices;
        }

        /// <summary>
        /// Unpause a number of dice from the simulation
        /// </summary>
        /// <param name="number">
        ///     Number of faces of the dice
        /// </param>
        /// <param name="amount">
        ///     Amount of dices to unpause. If -1, unpause all dices with the given number of faces
        /// </param>
        /// <returns>
        ///     List of Dice objects that were unpaused
        /// </returns>
        public List<Dice> UnpauseDice(int number, int amount)
        {
            List<Dice> dices = new List<Dice>();
            int unpaused = 0;
            for (int i = 0; i < _diceSetup.Count; i++)
            {
                if (_diceSetup[i].faces == number && (unpaused < amount || amount < 0))
                {
                    _diceDataList[i].rb.isKinematic = false;
                    _diceDataList[i].rb.useGravity = true;
                    _diceDataList[i]._setup.simulate = true;
                    unpaused++;
                    dices.Add(_diceDataList[i].diceLogic);
                }
            }

            return dices;
        }

        #endregion

        #region utils

        private void GenerateDice()
        {
            _diceDataList = new List<DiceData>();

            //Object pooling. Only generate dices we need
            for (int i = 0; i < _diceSetup.Count; i++)
            {
                if (_diceSetup[i].simulate == false)
                {
                    continue;
                }

                DiceData newDiceData = new DiceData();

                GameObject die = null;
                bool gotDie = false;

                // check if we already have that dice
                if (_instantiatedDice != null && _instantiatedDice.Count > i)
                {
                    die = _instantiatedDice[i];
                    gotDie = true;
                }
                else
                {
                    // if not, we create a new die
                    GameObject prefab = _dicePrefabs.Find(x =>
                        x.GetComponentInChildren<Dice>().Faces() == _diceSetup[i].faces
                    );

                    if (prefab == null)
                    {
                        Debug.LogError(
                            "No dice prefab found with " + _diceSetup[i].faces + " faces"
                        );
                        return;
                    }
                    die = Instantiate(prefab, transform);
                    die.name = prefab.name + " (" + i + ")";
                    _instantiatedDice.Add(die);
                }

                // init all values
                newDiceData = new DiceData(die, _diceSetup[i]);
                newDiceData.diceLogic = die.GetComponentInChildren<Dice>();
                _diceDataList.Add(newDiceData);

                _diceDataList[i].diceLogic.Reset();
                _diceDataList[i].diceUI.Reset();
                _diceDataList[i].rb.useGravity = true;
                _diceDataList[i].rb.isKinematic = false;

                // if we have a die, it's on the floor, let's throw it
                if (gotDie)
                {
                    // do a different throw as we are on the floor
                    float x,
                        y,
                        z;
                    y = Random.Range(25, 40);
                    Vector3 force = new Vector3(0, y, 0);

                    x = Random.Range(-50, 50);
                    y = Random.Range(-50, 50);
                    z = Random.Range(-50, 50);
                    Vector3 torque = new Vector3(x, y, z);

                    _diceDataList[i].diceLogic.Reset();
                    _diceDataList[i].diceUI.Reset();
                    _diceDataList[i].rb.AddForce(force, ForceMode.Impulse);
                    _diceDataList[i].rb.AddTorque(torque, ForceMode.Impulse);
                }
                else
                {
                    //Set the position and rotation
                    InitialState initial = SetInitialState();
                    _diceDataList[i].rb.velocity = initial.force;
                    _diceDataList[i].rb.AddTorque(initial.torque, ForceMode.VelocityChange);
                }
            }
        }

        private InitialState SetInitialState()
        {
            //Randomize X, Y, Z position in the bounding box
            float x =
                transform.position.x
                + Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2);
            float y =
                transform.position.y
                + Random.Range(-transform.localScale.y / 2, transform.localScale.y / 2);
            float z =
                transform.position.z
                + Random.Range(-transform.localScale.z / 2, transform.localScale.z / 2);
            Vector3 position = new Vector3(x, y, z);

            x = Random.Range(0, 360);
            y = Random.Range(0, 360);
            z = Random.Range(0, 360);
            Quaternion rotation = Quaternion.Euler(x, y, z);

            x = Random.Range(0, 25);
            y = Random.Range(0, 25);
            z = Random.Range(0, 25);
            Vector3 force = new Vector3(x, -y, z);

            x = Random.Range(0, 50);
            y = Random.Range(0, 50);
            z = Random.Range(0, 50);
            Vector3 torque = new Vector3(x, y, z);

            return new InitialState(position, rotation, force, torque);
        }

        #endregion
    }
}
