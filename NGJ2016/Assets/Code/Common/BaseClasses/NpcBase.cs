using System;
using UnityEngine;
using System.Linq;
using Assets.Code.Common.Enums;
using Assets.Code.PrefabAccess;
using Random = UnityEngine.Random;

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

        [SerializeField]
        public float PanicSpeed;

        [SerializeField]
        public float PanicTime;

        [SerializeField]
        public float PanicDirectionBias;

        public override void Move()
        {
            switch (Strategy)
            {
                case NpcStrategy.Idle:
                    MovementSpeed = 1;
                    if (CurrentTagetCell == null ||
                        (CurrentTagetCell.transform.position - transform.position).magnitude < 0.1)
                    {
                        var cellDists = CellManager.GetAllActiveObjects<CellBase>()
                            .Where(c => c.Wall == null)
                            .Select(c => new {c, (c.transform.position - transform.position).magnitude}).ToList();

                        var cell = cellDists
                            .OrderBy(c => c.magnitude)
                            .First();

                        var possibleCells = cell
                            .c.NeighBours.Where(c => c.Wall == null).ToList();

                        CurrentTagetCell = possibleCells[Random.Range(0, possibleCells.Count)];

                        FacingDirection = CurrentTagetCell.transform.position - transform.position;
                        MovementDirection = CurrentTagetCell.transform.position * BaseMovementSpeed;
                    }
                    break;
                case NpcStrategy.Look:
                    MovementSpeed = 0;
                    FacingDirection = Player.transform.position - transform.position;
                    break;
                case NpcStrategy.Panic:
                    MovementSpeed = 1;
                    var angle = Random.value*20 - 10 + PanicDirectionBias;
                    FacingDirection = Quaternion.AngleAxis(angle, Vector3.forward) * FacingDirection;
                    MovementDirection = new Vector3(FacingDirection.x, FacingDirection.y, 0).normalized * PanicSpeed;

                    break;
            }
            transform.right = FacingDirection;
            transform.position += MovementDirection * MovementSpeed * Time.deltaTime;
        }

        public void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.GetComponent<PooBase>() != null)
            {
                Panic();
            }
        }

        public void Panic()
        {
            Strategy = NpcStrategy.Panic;
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }

        public void UnPanic()
        {

            PickNonPanicStrategy();
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }

        public void Update()
        {
            switch (Strategy)
            {
                case NpcStrategy.Idle:
                case NpcStrategy.Look:
                    if (Player.IsFarting && (Player.transform.position - transform.position).magnitude < 3)
                    {
                        Panic();
                    }
                    break;

                case NpcStrategy.Panic:
                    PanicTime += Time.deltaTime;
                    if (PanicTime > 120)
                    {
                        UnPanic();
                    }
                    break;
            }
            
        }

        public void PickNonPanicStrategy()
        {
            Strategy = UnityEngine.Random.value >= 0.5 ? NpcStrategy.Look : NpcStrategy.Idle;
        }

        public override void Init()
        {
            Id = Guid.NewGuid();
            PickNonPanicStrategy();
            BaseMovementSpeed = 0.5f;
            MaxSpeed = 6f;
            PanicSpeed = 6f;
            MovementDecay = .95f;
            PanicDirectionBias = UnityEngine.Random.value*20 - 10;
        }
    }
}