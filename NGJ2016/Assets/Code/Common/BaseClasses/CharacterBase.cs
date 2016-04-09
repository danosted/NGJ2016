﻿using UnityEngine;
using System.Collections;
using Assets.Code;
using Assets.Code.Common.Objects;

namespace Assets.Code.Common.BaseClasses
{
    public abstract class CharacterBase : MonoBehaviour
    {
        [SerializeField]
        public float BaseMovementSpeed;

        [SerializeField]
        public float MaxSpeed;

        [SerializeField]
        public float MovementDecay;

        public Vector3 MovementDirection { get; set; }

        public Vector3 FacingDirection { get; set; }

        public float MovementSpeed;

        public float Health { get; set; }
        
        public float Energy { get; set; }

        public int Money { get; set; }


        public Vector2 Position2D { get; private set; }

        public abstract void Move();

        public virtual void Init()
        {
            BaseMovementSpeed = 0.25f;
            MaxSpeed = 4f;
            MovementDecay = .95f;
        }
    }
}