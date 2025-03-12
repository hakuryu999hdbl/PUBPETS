using UnityEngine;

namespace MADD
{
    [System.Serializable]
    public class DiceSetup
    {
        public int faces;
        public int result;
        public bool simulate = true;
    }

    [System.Serializable]
    public struct DiceData
    {
        public DiceSetup _setup;
        public GameObject diceObject;
        public Rigidbody rb;
        public Dice diceLogic;
        public DiceShell diceUI;

        public DiceData(GameObject diceObject, DiceSetup setup)
        {
            this._setup = setup;
            this.diceObject = diceObject;
            this.rb = diceObject.GetComponent<Rigidbody>();
            this.diceLogic = diceObject.transform.GetChild(0).GetComponent<Dice>();
            this.diceUI = diceObject.GetComponent<DiceShell>();
            this.rb.maxAngularVelocity = 1000;
        }
    }

    [System.Serializable]
    public struct InitialState
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 force;
        public Vector3 torque;

        public InitialState(Vector3 position, Quaternion rotation, Vector3 force, Vector3 torque)
        {
            this.position = position;
            this.rotation = rotation;
            this.force = force;
            this.torque = torque;
        }
    }
}
