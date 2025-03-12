using System.Collections.Generic;
using UnityEngine;

namespace MADD
{
    public class MultipleRoller : MonoBehaviour
    {
        #region member variables

        public List<DiceManager> _managers;

        #endregion


        public void Roll(int manager)
        {
            _managers[manager].Roll();
        }
    }
}
