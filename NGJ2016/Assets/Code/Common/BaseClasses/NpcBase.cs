using UnityEngine;
using System.Collections;
using Assets.Code;
using Assets.Code.Common.Objects;
using Assets.Code.Common.Enums;
using Assets.Code.PrefabAccess;

namespace Assets.Code.Common.BaseClasses
{
    public class NpcBase : CharacterBase
    {
        public override void Move()
        {
            switch (Strategy)
            {
                case NpcStrategy.Idle:
                case NpcStrategy.Look:
                    var man = ManagerCollection.Instance.GetManager(Constants.CharacterManagerName);
                    var player = man.GetPrefabFromType<PlayerBase>();
                    MovementSpeed = 0;
                    FacingDirection = player.transform.position - transform.position;
                    break;
                case NpcStrategy.Panic:
                    MovementSpeed = 1;
                    break;
            }

            transform.position += MovementDirection * MovementSpeed * Time.deltaTime;
        }

        [SerializeField]
        public NpcStrategy Strategy;

        public override void Init()
        {
            Strategy = NpcStrategy.Look;
            BaseMovementSpeed = 0.5f;
            MaxSpeed = 6f;
            MovementDecay = .95f;
        }
    }
}