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
                //c.transform.position = c.transform.position + new Vector3(c.MovementSpeed * hor, c.MovementSpeed * vert, 0f) * Time.deltaTime;

                var mouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - c.transform.position;
                if (mouseDirection.magnitude > 1 || c.IsFarting)
                {
                    mouseDirection = mouseDirection.normalized;
                }
                var hor = mouseDirection.x;
                var vert = mouseDirection.y;

                var totalSpeed = c.IsFarting ? c.FartSpeedBonus*c.MovementSpeed : c.MovementSpeed;

                var direction = new Vector3(totalSpeed * hor, totalSpeed * vert, 0f);
                c.MovementDirection += direction;
                c.MovementDirection *= c.MovementDecay;
                var totalMaxSpeed = c.IsFarting ? c.FartSpeedBonus * c.MaxSpeed : c.MaxSpeed;
                if (c.MovementDirection.magnitude > totalMaxSpeed)
                {
                    c.MovementDirection *= totalMaxSpeed / c.MovementDirection.magnitude;
                }

                c.transform.position += c.MovementDirection  * Time.deltaTime;
                Camera.current.transform.position = new Vector3() {x = c.transform.position.x,y= c.transform.position.y,z = -3 };
                
            }
            var deltaSeconds = (int)(Time.deltaTime * 1000);
        }
    }
}
