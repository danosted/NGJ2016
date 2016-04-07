namespace Assets.Code.PrefabAccess
{
    using Assets.Code.Common;
    using Assets.Code.Common.BaseClasses;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class ManagerCollection
    {
        private Dictionary<string, ManagerBase> _managers = new Dictionary<string, ManagerBase>();

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
    }
}
