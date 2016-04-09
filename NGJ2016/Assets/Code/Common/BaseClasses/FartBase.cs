using UnityEngine;
using System.Collections;
using Assets.Code;
using Assets.Code.Common.Objects;

namespace Assets.Code.Common.BaseClasses
{
    public class FartBase : MonoBehaviour
    {
        public PlayerBase SourceCharacter { get; set; }

        public float Offset { get; set; }

        public FartBase()
        {
            Offset = 1;
            transform.localScale = new Vector3(10,10,0);
        }
    }
}