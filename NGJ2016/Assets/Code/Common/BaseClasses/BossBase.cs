using System;
using UnityEngine;
using System.Linq;
using Assets.Code.Common.Enums;
using Assets.Code.PrefabAccess;
using Random = UnityEngine.Random;

namespace Assets.Code.Common.BaseClasses
{
    public class BossBase : NpcBase
    {

        [SerializeField]
        public float PanicTime;

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

                    }
                    FacingDirection = CurrentTagetCell.transform.position-transform.position;
                    MovementDirection = FacingDirection.normalized * BaseMovementSpeed;
                    break;
                case NpcStrategy.Look:
                    MovementSpeed = 0;
                    FacingDirection = Player.transform.position - transform.position;
                    break;

                case NpcStrategy.Panic:
                    MovementSpeed = 0;
                    break;
            }
            transform.right = FacingDirection;
            transform.position += MovementDirection * MovementSpeed * Time.deltaTime;
        }
        

        public override void Panic()
        {
            if (Strategy != NpcStrategy.Panic)
            {
                Player.DisgraceMeter.PercentFull += 0.25f;
                Player.DisgraceMeter.Render();
            }

            Strategy = NpcStrategy.Panic;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(106 / 255f, 56 / 255f, 10 / 255f);
            var sounds = gameObject.GetComponents<AudioSource>();
            sounds[Random.Range(0, sounds.Length)].Play();
        }
        

        public void Update()
        {
            switch (Strategy)
            {
                case NpcStrategy.Idle:
                case NpcStrategy.Look:
                    if (Player.IsFarting && (Player.transform.position - transform.position).magnitude < 3)
                    {
                        Strategy = NpcStrategy.Look;
                    }
                    else if ((Player.transform.position - transform.position).magnitude > 3)
                    {
                        Strategy = NpcStrategy.Idle;
                    }
                    break;

                case NpcStrategy.Panic:
                    PanicTime += Time.deltaTime;
                    if (PanicTime > 3)
                    {
                        ManagerCollection.Instance.NpcSpawnHandler.DeactivateNpc(this);
                    }
                    break;
            }
            
        }

        public override void PickNonPanicStrategy()
        {
            Strategy = UnityEngine.Random.value >= 0.5 ? NpcStrategy.Look : NpcStrategy.Idle;
        }

        public override void Init()
        {
            base.Init();
            BaseMovementSpeed = 0.8f;
        }
    }
}