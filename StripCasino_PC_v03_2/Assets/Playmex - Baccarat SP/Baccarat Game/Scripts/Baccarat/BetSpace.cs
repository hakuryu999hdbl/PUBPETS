using UnityEngine;


namespace Baccarat_Game
{
    //public enum BetType { player, banker, tie, playerPair, perfectPair, bankerPair, small, big, eitherPair }
    public enum BetType { Player, Banker, Tie }

    public class BetSpace : MonoBehaviour
    {

        private GameObject chipStackGameObject;

        [HideInInspector]
        public ChipStack chipStack;
        [HideInInspector]
        public MeshRenderer mesh;

        [Space]
        [Header("Propierties")]
        public BetType betType;
        public float payout;

        [Space]
        public Vector3 chipStackOffsetPosition;

        public float Bet { get { return chipStack.GetValue(); } }

        private float lastBet = 0;
        private bool addedOnThisRound = false;
        public static bool firstBet = true;

        public delegate void OnBet();
        public static event OnBet OnFirstBet;

        [Space]
        [Header("Materials")]
        public Material betSpaceMat;
        public Material betSpaceWinMat;

        private void Awake()
        {
            mesh = GetComponent<MeshRenderer>();
        }

        void Start()
        {
            if (mesh)
                mesh.enabled = false;


            chipStackGameObject = Instantiate(Resources.Load<GameObject>("chipstack"));

            chipStackGameObject.transform.position = transform.position + chipStackOffsetPosition;
            chipStackGameObject.transform.SetParent(transform.parent);

            chipStack = chipStackGameObject.GetComponent<ChipStack>();

            ResultManager.RegisterBetSpace(this);
            //AmericanWheel.OnRebetAndSpin += Rebet;
        }

        private void OnEnable()
        {
            ResultManager.onResult += ResetAddedOnthisRound;
            BaccaratGame.Done += HideMesh;
            OnFirstBet += ResetLastBet;
        }

        private void OnDisable()
        {
            ResultManager.onResult -= ResetAddedOnthisRound;
            BaccaratGame.Done -= HideMesh;
            OnFirstBet -= ResetLastBet;
        }

        private void OnMouseEnter()
        {
            ToolTipManager.Instance.SelectTarget(chipStack);
            if (!ResultManager.betsEnabled)
                return;
            if (mesh)
            {
                mesh.enabled = true;
            }
        }

        void OnMouseExit()
        {
            ToolTipManager.Instance.DeselectTarget();
            if (mesh)
            {
                mesh.enabled = false;
                mesh.material = betSpaceMat;
            }
        }

        void OnMouseDown()
        {
            if (!ResultManager.betsEnabled)
                return;
            if (mesh)
            {
                mesh.enabled = true;
                mesh.material = betSpaceMat;
            }

            float selectedValue = ChipManager.GetSelectedValue();
            AddBet(selectedValue);
        }

        public float ResolveBet()
        {

            bool won = false;
            float winAmount = 0;

            if (betType == BetType.Tie)
                won = ResultManager.IsTie();

            else if (betType == BetType.Player)
                won = ResultManager.PlayerWon();

            else
                won = !ResultManager.PlayerWon();

            if (won)
            {
                winAmount = chipStack.Win(payout);
                print("Win Bet " + name + ": " + winAmount);
            }
            else
            {
                chipStack.Clear();
                winAmount = 0;
            }

            if (winAmount > 0)
            {
                mesh.enabled = true;
                mesh.material = betSpaceWinMat;
            }
            return winAmount;
        }

        public void Rebet()
        {
            if (lastBet == 0 || addedOnThisRound)
                return;

            //BetHistoryManager.getInstance().Add(chipStack, lastBet, betType, winningNumbers);
            chipStack.Add(lastBet);
        }

        public void ResetAddedOnthisRound()
        {
            addedOnThisRound = false;
        }

        private void ResetLastBet()
        {
            lastBet = 0;
        }

        public void Clear()
        {
            float value = chipStack.GetValue();
            BalanceManager.ChangeBalance(value);
            chipStack.Clear();
            mesh.material = betSpaceMat;
            mesh.enabled = false;
        }

        public void Remove(float value)
        {
            Player.totalBet -= value;
            BalanceManager.ChangeBalance(value);
            chipStack.SetValue(chipStack.GetValue() - value);
        }

        public void HideMesh()
        {
            mesh.material = betSpaceMat;
            mesh.enabled = false;
        }

        public void AddBet(float selectedValue)
        {
            if (!LimitBetPlate.AllowLimit(selectedValue))
                return;

            if (ResultManager.betsEnabled && BalanceManager.GetBalance() >= selectedValue)
            {
                if (firstBet)
                {
                    firstBet = false;
                    OnFirstBet();
                }
                AudioManager.SoundPlay(0);
                lastBet += selectedValue;
                Player.totalBet += selectedValue;

                BalanceManager.ChangeBalance(-selectedValue);
                print("Selected Bet: " + selectedValue);
                print("Total Bet: " + Player.totalBet);
                chipStack.Add(selectedValue);
                BetHistoryManager._Instance.Add((int)betType, selectedValue);
                ToolTipManager.Instance.SelectTarget(chipStack);
                addedOnThisRound = true;
            }
        }

        public void SetBet(float value)
        {
            if (!LimitBetPlate.AllowLimit(value))
                return;

            if (ResultManager.betsEnabled && BalanceManager.GetBalance() >= value)
            {
                if (firstBet)
                {
                    firstBet = false;
                    OnFirstBet();
                }
                lastBet += value;
                //BetHistoryManager.getInstance().Add(chipStack, selectedValue, betType, winningNumbers);
                Player.totalBet += value;
                print("Selected Bet: " + value);
                print("Total Bet: " + Player.totalBet);
                chipStack.SetValue(value);
                ToolTipManager.Instance.SelectTarget(chipStack);
                addedOnThisRound = true;
            }
        }

        void OnDrawGizmosSelected()
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position + chipStackOffsetPosition, .1f);
        }
    }
}

