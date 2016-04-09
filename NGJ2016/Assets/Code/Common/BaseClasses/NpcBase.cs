using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.Code;
using Assets.Code.Common.Objects;
using Assets.Code.Common.Enums;
using Assets.Code.PrefabAccess;
using UnityEditor;

namespace Assets.Code.Common.BaseClasses
{
    public class NpcBase : CharacterBase
    {
        public Guid Id;


        private ManagerBase _characterManager;
        public ManagerBase CharacterManager
        {
            get
            {
                if (_characterManager == null)
                {
                    _characterManager = ManagerCollection.Instance.GetManager(Constants.CharacterManagerName);
                }
                return _characterManager;
            }
        }

        private PlayerBase _player;
        private PlayerBase Player
        {
            get
            {
                if (_player == null)
                {
                    _player = CharacterManager.GetAllActiveObjects<PlayerBase>().First();
                }
                return _player;
            }
        }

        [SerializeField]
        public NpcStrategy Strategy;

        [SerializeField]
        public float PanicSpeed;

        public override void Move()
        {
            switch (Strategy)
            {
                case NpcStrategy.Idle:
                case NpcStrategy.Look:
                    MovementSpeed = 0;
                    FacingDirection = Player.transform.position - transform.position;
                    break;
                case NpcStrategy.Panic:
                    MovementSpeed = 1;
                    MovementDirection = Quaternion.AngleAxis(UnityEngine.Random.value*10-10, Vector3.forward) * FacingDirection;
                    MovementDirection = new Vector3(MovementDirection.x, MovementDirection.y, 0).normalized * PanicSpeed;

                    break;
            }
            transform.right = FacingDirection;
            transform.position += MovementDirection * MovementSpeed * Time.deltaTime;
        }

        public void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.GetComponent<PooBase>() != null || coll.gameObject.GetComponent<PlayerBase>() != null)
            {
                Strategy = NpcStrategy.Panic;
            }
        }

        public override void Init()
        {
            Id = Guid.NewGuid();
            Strategy = NpcStrategy.Look;
            BaseMovementSpeed = 0.5f;
            MaxSpeed = 6f;
            PanicSpeed = 6f;
            MovementDecay = .95f;
            
        }
    }
}