using System;
using UnityEngine;
using System.Linq;
using Assets.Code.Common.Enums;
using Assets.Code.PrefabAccess;
using Random = UnityEngine.Random;

namespace Assets.Code.Common.BaseClasses
{
    public abstract class NpcBase : CharacterBase
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
        protected PlayerBase Player
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

        private ManagerBase _cellManager;
        public ManagerBase CellManager
        {
            get
            {
                if (_cellManager == null)
                {
                    _cellManager = ManagerCollection.Instance.GetManager(Constants.CellManagerName);
                }
                return _cellManager;
            }
        }

        [SerializeField]
        public NpcStrategy Strategy;


        

        public void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.GetComponent<PooBase>() != null)
            {
                Panic();
            }
        }

        public abstract void Panic();



        public abstract void PickNonPanicStrategy();

        public override void Init()
        {
            Id = Guid.NewGuid();
            PickNonPanicStrategy();
            BaseMovementSpeed = 0.5f;
            MaxSpeed = 6f;
            MovementDecay = .95f;
        }
    }
}