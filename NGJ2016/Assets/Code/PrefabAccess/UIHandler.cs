using System.Runtime.InteropServices;
using Assets.Code.Common;
using Assets.Code.Common.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
