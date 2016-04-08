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
                if (mouseDirection.magnitude > 1)
                {
                    mouseDirection = mouseDirection.normalized;
                }
                var hor = mouseDirection.x;
                var vert = mouseDirection.y;

                var direction = new Vector3(c.MovementSpeed * hor, c.MovementSpeed * vert, 0f);
                c.MovementDirection += direction;
                c.MovementDirection *= c.MovementDecay;
                if (c.MovementDirection.magnitude > c.MaxSpeed)
                {
                    c.MovementDirection *= c.MaxSpeed / c.MovementDirection.magnitude;
                }

                c.transform.position += c.MovementDirection  * Time.deltaTime;
            }
            var deltaSeconds = (int)(Time.deltaTime * 1000);
        }
    }
}
