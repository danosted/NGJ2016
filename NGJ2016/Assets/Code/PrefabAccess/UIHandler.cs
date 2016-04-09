using Assets.Code.Common.BaseClasses;
using UnityEngine;

namespace Assets.Code.PrefabAccess
{
    public class UIHandler : MonoBehaviour
    {
        private PlayerBase _character;

        public void SetCharacter(PlayerBase character)
        {
            _character = character;
        }

        void Update()
        {

            if (_character.IsFarting)
            {
                _character.FartMeter.DecreaseMeter();
            }
            else
            {
                _character.FartMeter.IncreaseMeter();
            }
        }
    }
}
