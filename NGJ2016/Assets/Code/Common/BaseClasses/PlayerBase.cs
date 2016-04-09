using UnityEngine;
using System.Collections;
using Assets.Code;
using Assets.Code.Common.Objects;

namespace Assets.Code.Common.BaseClasses
{
    public class PlayerBase : CharacterBase
    {
        public int Money { get; set; }
        
        public Inventory Inventory { get; private set; }

        public CellBase CurrentTagetCell { get; set; }

        public override void Move()
        {
            var mouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            if (mouseDirection.magnitude > 1)
            {
                mouseDirection = mouseDirection.normalized;
            }
            var hor = mouseDirection.x;
            var vert = mouseDirection.y;

            var direction = new Vector3(BaseMovementSpeed * hor, BaseMovementSpeed * vert, 0f);
            MovementDirection += direction;
            MovementDirection *= MovementDecay;
            if (MovementDirection.magnitude > MaxSpeed)
            {
                MovementDirection *= MaxSpeed / MovementDirection.magnitude;
            }
        }

        public override void Init()
        {
            this.Inventory = new Inventory();
            BaseMovementSpeed = 0.5f;
            MovementSpeed = 1f;
            MaxSpeed = 6f;
            MovementDecay = .95f;
        }
    }
}