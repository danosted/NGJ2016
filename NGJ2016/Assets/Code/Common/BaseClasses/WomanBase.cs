using System;
using UnityEngine;
using System.Linq;
using Assets.Code.Common.Enums;
using Assets.Code.PrefabAccess;
using Random = UnityEngine.Random;

namespace Assets.Code.Common.BaseClasses
{
    public class WomanBase : NpcBase
    {

        [SerializeField]
        public float PanicTime;

        [SerializeField]
        public float PanicSpeed;

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

                    }
                    FacingDirection = CurrentTagetCell.transform.position-transform.position;
                    MovementDirection = FacingDirection.normalized * BaseMovementSpeed;
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
        

        public override void Panic()
        {
            Strategy = NpcStrategy.Panic;
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            var sound = gameObject.GetComponents<AudioSource>();
            sound[Random.Range(0,sound.Length)].Play();
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

        public override void PickNonPanicStrategy()
        {
            Strategy = UnityEngine.Random.value >= 0.5 ? NpcStrategy.Look : NpcStrategy.Idle;
        }

        public override void Init()
        {
            base.Init();
            PanicSpeed = 6f;
            PanicDirectionBias = UnityEngine.Random.value*20 - 10;
        }
    }
}