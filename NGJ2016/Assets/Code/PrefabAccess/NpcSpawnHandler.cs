using Assets.Code.Common.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Common;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Code.PrefabAccess
{
    public class NpcSpawnHandler : MonoBehaviour
    {
        private Dictionary<Guid, NpcBase> _activeNpcs;

        public Dictionary<Guid, NpcBase> ActiveNpcs
        {
            get
            {
                if (_activeNpcs == null)
                {
                    _activeNpcs = new Dictionary<Guid, NpcBase>();
                }
                return _activeNpcs;
            }
        }

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

        private float _spawnRate;
        private int _maxNpcs;

        public NpcSpawnHandler()
        {
            _spawnRate = 0.1f;
            _maxNpcs = 20;
        }

        public void SpawnNpc()
        {
            var npc = CharacterManager.GetPrefabFromType<NpcBase>();
            npc.Init();
            var activeCells = CellManager.GetAllActiveObjects<CellBase>().Where(c => c.Wall == null).ToList();
            var cellPosition = activeCells[Random.Range(0, activeCells.Count)].transform.position;
            npc.transform.position = cellPosition;
            ActiveNpcs.Add(npc.Id, npc);
            ManagerCollection.Instance.MovementHandler.MoveCharacterContinuous(npc);
        }

        public void DeactivateNpc(NpcBase npc)
        {
            if (ActiveNpcs.ContainsKey(npc.Id))
            {
                ActiveNpcs.Remove(npc.Id);
            }
            CharacterManager.RecyclePrefab(npc.gameObject);
        }

        void Update()
        {
            var r = Random.value;
            if (ActiveNpcs.Count <= _maxNpcs && r < _spawnRate)
            {
                SpawnNpc();
            }
        }
    }
}
