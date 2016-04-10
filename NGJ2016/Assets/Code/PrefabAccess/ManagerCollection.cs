namespace Assets.Code.PrefabAccess
{
    using Assets.Code.Common;
    using Assets.Code.Common.BaseClasses;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class ManagerCollection
    {
        private readonly Dictionary<string, ManagerBase> _managers;

        public ManagerCollection()
        {
            _managers = new Dictionary<string, ManagerBase>();
        }

        private static ManagerCollection _managerCollection;
        public static ManagerCollection Instance
        {
            get
            {
                if (_managerCollection == null)
                {
                    _managerCollection = new ManagerCollection();
                }
                return _managerCollection;
            }
        }

        public ManagerBase GetManager(string managerName)
        {
            if (!_managers.ContainsKey(managerName))
            {
                var prefab = Resources.Load(managerName);
                if (prefab == null)
                {
                    throw new Exception(string.Format("No manage with name {0}", managerName));
                }
                var GO = (GameObject.Instantiate(prefab)) as GameObject;
                var manager = GO.GetComponent<ManagerBase>();
                _managers.Add(managerName, manager);
                return manager;
            }
            return _managers[managerName];
        }

        private MovementHandler _movementHandler;
        public MovementHandler MovementHandler
        {
            get
            {
                if (_movementHandler == null)
                {
                    var prefab = Resources.Load(Constants.MovementHandlerName);
                    var GO = (GameObject.Instantiate(prefab)) as GameObject;
                    _movementHandler = GO.GetComponent<MovementHandler>();
                }
                return _movementHandler;
            }
        }

        private ProjectileHandler _projectileHandler;
        public ProjectileHandler ProjectileHandler
        {
            get
            {
                if (_projectileHandler == null)
                {
                    var prefab = Resources.Load(Constants.ProjectileHandlerName);
                    var GO = (GameObject.Instantiate(prefab)) as GameObject;
                    _projectileHandler = GO.GetComponent<ProjectileHandler>();
                }
                return _projectileHandler;
            }
        }

        private AudioHandler _audioHandler;
        public AudioHandler AudioHandler
        {
            get
            {
                if (_audioHandler == null)
                {
                    var prefab = Resources.Load(Constants.AudioHandler);
                    var GO = (GameObject.Instantiate(prefab)) as GameObject;
                    _audioHandler = GO.GetComponent<AudioHandler>();
                }
                return _audioHandler;
            }
        }

        private NpcSpawnHandler _npcSpawnHandler;
        public NpcSpawnHandler NpcSpawnHandler
        {
            get
            {
                if (_npcSpawnHandler == null)
                {
                    var prefab = Resources.Load(Constants.NpcSpawnHandlerName);
                    var GO = (GameObject.Instantiate(prefab)) as GameObject;
                    _npcSpawnHandler = GO.GetComponent<NpcSpawnHandler>();
                }
                return _npcSpawnHandler;
            }
        }

        private UIHandler _uiHandler;
        public UIHandler UIHandler
        {
            get
            {
                if (_uiHandler == null)
                {
                    var prefab = Resources.Load(Constants.UiHandlerName);
                    var GO = (GameObject.Instantiate(prefab)) as GameObject;
                    _uiHandler = GO.GetComponent<UIHandler>();
                }
                return _uiHandler;
            }
        }
    }
}
