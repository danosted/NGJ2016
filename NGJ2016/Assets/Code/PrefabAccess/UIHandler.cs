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
            var isPuking = Input.GetKey(KeyCode.Mouse0);
            var isFarting = Input.GetKey(KeyCode.Mouse1);

            if (isFarting || _character.FartMeter.OhShitTriggered)
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
