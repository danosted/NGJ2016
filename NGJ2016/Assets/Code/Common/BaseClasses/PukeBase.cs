using UnityEngine;
using System.Collections;
using Assets.Code;
using Assets.Code.Common.Objects;

namespace Assets.Code.Common.BaseClasses
{
    public class PukeBase : MonoBehaviour
    {
        public float Range { get; set; }

        public PlayerBase SourceCharacter { get; set; }


    }
}