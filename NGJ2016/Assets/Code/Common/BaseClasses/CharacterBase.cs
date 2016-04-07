using UnityEngine;
using System.Collections;
using Assets.Code;
using Assets.Code.Common.Objects;

namespace Assets.Code.Common.BaseClasses
{
    public class CharacterBase : MonoBehaviour
    {
        public float MovementSpeed { get; set; }

        public float Health { get; set; }

        public float Energy { get; set; }

        public int Money { get; set; }

        public Vector2 Position2D { get; private set; }

        public Inventory Inventory { get; private set; }

        public CellBase CurrentTagetCell { get; set; }

        public void Init()
        {
            this.Inventory = new Inventory();
            MovementSpeed = 2f;
        }
    }
}