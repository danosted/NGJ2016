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

        private bool _isFarting;
        public bool IsFarting
        {
            get
            {
                return FartMeter.OhShitTriggered || _isFarting;
            }
            set { _isFarting = value; }
        }

        public float FartSpeedBonus { get; set; }

        public FartMeterBase FartMeter { get; set; }

        public override void Move()
        {
            var mouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            if (mouseDirection.magnitude > 1)
            {
                mouseDirection = mouseDirection.normalized;
            }
            var hor = mouseDirection.x;
            var vert = mouseDirection.y;
            
            var totalSpeed = IsFarting ? FartSpeedBonus * MovementSpeed : MovementSpeed;

            var direction = new Vector3(totalSpeed * hor, totalSpeed * vert, 0f);
            MovementDirection += direction;
            MovementDirection *= MovementDecay;
            var totalMaxSpeed = IsFarting ? FartSpeedBonus * MaxSpeed : MaxSpeed;
            // Normaliser til max-speed
            if (MovementDirection.magnitude > totalMaxSpeed)
            {
                MovementDirection *= totalMaxSpeed / MovementDirection.magnitude;
            }

            transform.position += MovementDirection * Time.deltaTime;
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