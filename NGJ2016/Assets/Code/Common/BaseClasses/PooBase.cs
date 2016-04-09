using UnityEngine;
using System.Collections;
using Assets.Code;
using Assets.Code.Common.Objects;

namespace Assets.Code.Common.BaseClasses
{
    public class PooBase : MonoBehaviour
    {
        public CharacterBase SourceCharacter { get; set; }

        public Vector3 Direction { get; set; }
        public float Speed { get; set; }

        public PooBase()
        {
            Speed = 5.0f;
        }

        void Update()
        {
            transform.position += Direction*Speed*Time.deltaTime;
        }
    }
}