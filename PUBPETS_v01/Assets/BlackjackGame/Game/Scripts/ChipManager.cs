using System.Linq;
using UnityEngine;
namespace Blackjack_Game
{
    public enum StackType { Standard, Split, Double, DoubleSplit, Insurance }

    public class ChipManager : MonoBehaviour
    {

        public static ChipManager _Instance;

        private Chip SelectedChip;

        [Header("Chip Prefabs")]
        [SerializeField] GameObject[] Chips;

        [Space]
        [Header("Chip Stacks")]
        private ChipStack[] _stacks;

        private void Awake()
        {
            _Instance = this;
            int stackTypeLengh = System.Enum.GetValues(typeof(StackType)).Length;
            _stacks = new ChipStack[stackTypeLengh];
        }

        public static void SetWinningStack(StackType type, bool won)
        {
            _Instance._stacks[(int)type].SetWinningState(won);
        }
        public static void SetPushStack(StackType type, bool push)
        {
            _Instance._stacks[(int)type].SetPushState(push);
        }
        public static void SetBlackjackStack()
        {
            _Instance._stacks[0].SetBlackjackState();
        }

        public static void SelectChip(Chip chip)
        {
            if (_Instance.SelectedChip != null)
                _Instance.SelectedChip.Deselected();
            _Instance.SelectedChip = chip;
            chip.Selected();
        }

        public static float GetWinnings()
        {
            return _Instance._stacks.Sum(stack => stack.GetWin());
        }

        public static void AddToStack(StackType type, float value)
        {
            _Instance._stacks[(int)type].Add(value);
        }

        public static void RemoveFromStack(StackType type, float value)
        {
            _Instance._stacks[(int)type].Remove(value);
        }

        public static void ClearStack(StackType type)
        {
            _Instance._stacks[(int)type].Clear();
        }

        public static void RegisterStack(ChipStack stack)
        {
            _Instance._stacks[(int)stack.type] = stack;
        }

        public static GameObject InstanciateChip(int chipIndex)
        {
            return Instantiate(_Instance.Chips[chipIndex]);
        }
    }
}