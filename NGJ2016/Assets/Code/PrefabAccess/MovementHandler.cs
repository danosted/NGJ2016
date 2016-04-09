using Assets.Code.Common.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.PrefabAccess
{
    public class MovementHandler : MonoBehaviour
    {
        private List<CharacterBase> _movingCharacters;

        public void MoveCharacterContinuous(CharacterBase c)
        {
            if (_movingCharacters == null)
            {
                _movingCharacters = new List<CharacterBase>();
            }
            _movingCharacters.Add(c);
        }

        void Update()
        {
            //var vert = Input.GetAxis("Vertical");
            //var hor = Input.GetAxis("Horizontal");


            foreach (var c in _movingCharacters)
            {
                //c.transform.position = c.transform.position + new Vector3(c.BaseMovementSpeed * hor, c.BaseMovementSpeed * vert, 0f) * Time.deltaTime;

                c.Move();

                Camera.current.transform.position = new Vector3() {x = c.transform.position.x,y= c.transform.position.y,z = -3 };
                
            }
            var deltaSeconds = (int)(Time.deltaTime * 1000);
        }
    }
}
