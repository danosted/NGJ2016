using Assets.Code.PrefabAccess;
using UnityEngine;
using System.Collections;
using Assets.Code;
using Assets.Code.Common.Objects;
using UnityEngine.SocialPlatforms;

namespace Assets.Code.Common.BaseClasses
{
    public class PooBase : MonoBehaviour
    {
        public CharacterBase SourceCharacter { get; set; }

        public Vector3 Direction { get; set; }
        public float Speed { get; set; }
        public float DistanceTravelled { get; set; }
        public float Range { get; set; }

        public PooBase()
        {
            Speed = 10.0f;
            Range = 30.0f;
        }

        void Update()
        {
            var distToTravel = Direction*Speed*Time.deltaTime;
            transform.position += distToTravel;
            DistanceTravelled += distToTravel.magnitude;
            if (DistanceTravelled > Range)
            {
                ManagerCollection.Instance.GetManager(Constants.PooManager).RecyclePrefab(gameObject);
            }
        }
    }
}