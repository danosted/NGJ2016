using UnityEngine;
using System.Collections;
using Assets.Code;
using Assets.Code.Common.Objects;

namespace Assets.Code.Common.BaseClasses
{
    public class CharacterBase : MonoBehaviour
    {
        public float MovementSpeed;

        public float MaxSpeed;

        public float MovementDecay;

        public Vector3 MovementDirection { get; set; }

        public float Health { get; set; }
        
        public float Energy { get; set; }

        public int Money { get; set; }

        public Vector2 Position2D { get; private set; }

        public Inventory Inventory { get; private set; }

        public CellBase CurrentTagetCell { get; set; }

        public void Init()
        {
            this.Inventory = new Inventory();
            MovementSpeed = 0.5f;
            MaxSpeed = 6f;
            MovementDecay = .95f;
        }
    }
}