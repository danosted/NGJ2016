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
        public float Offset;

        public PooBase()
        {
            Speed = 10.0f;
            Range = 30.0f;
            Offset = 1f;
        }


        public void OnCollisionEnter2D(Collision2D coll)
        {
            var player = coll.gameObject.GetComponent<PlayerBase>();
            if (player != null)
            {
                return;
            }

            var npc = coll.gameObject.GetComponent<NpcBase>();
            if (npc != null)
            {
                npc.Panic();
            }

            ManagerCollection.Instance.GetManager(Constants.PooManager).RecyclePrefab(gameObject);
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