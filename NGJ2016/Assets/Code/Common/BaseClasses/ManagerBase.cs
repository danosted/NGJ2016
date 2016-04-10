using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Code.Common.BaseClasses
{
    public class ManagerBase : MonoBehaviour
    {
        #region Properties
        [SerializeField]
        private List<GameObject> _prefabPool;
        public List<GameObject> PrefabPool
        {
            get
            {
                return _prefabPool;
            }
            set
            {
                _prefabPool = value;
            }
        }

        [SerializeField]
        private List<GameObject> _activeObjects = new List<GameObject>();
        public List<GameObject> ActiveObjects
        {
            get
            {
                return _activeObjects;
            }
            set
            {
                _activeObjects = value;
            }
        }

        [SerializeField]
        private List<GameObject> _inactiveObjects = new List<GameObject>();
        public List<GameObject> InactiveObjects
        {
            get
            {
                return _inactiveObjects;
            }
            set
            {
                _inactiveObjects = value;
            }
        }
        #endregion

        #region Get
        public T GetPrefabFromType<T>()
        {
            var inactiveGO = InactiveObjects.Find(x => x.GetComponent<T>() != null);
            if (inactiveGO != null)
            {
                InactiveObjects.Remove(inactiveGO);
                ActiveObjects.Add(inactiveGO);
                return inactiveGO.GetComponent<T>();
            }
            var GO = PrefabPool.Find(x => x.GetComponent<T>() != null);
            var resultGO = GameObject.Instantiate(GO) as GameObject;
            ActiveObjects.Add(resultGO.gameObject);
            resultGO.transform.SetParent(transform);
            return resultGO.GetComponent<T>();
        }

        public T GetRandomPrefabFromType<T>()
        {
            var inactiveGOs = InactiveObjects.FindAll(x => x.GetComponent<T>() != null).ToList();
            if (inactiveGOs.Any())
            {
                //var rand = Random.Range(0, inactiveGOs.Count());
                //var inactiveGO = inactiveGOs[rand];
                //if (inactiveGO != null)
                //{
                //    InactiveObjects.Remove(inactiveGO);
                //    ActiveObjects.Add(inactiveGO);
                //    return inactiveGO.GetComponent<T>();
                //}
            }
            var GOs = PrefabPool.FindAll(x => x.GetComponent<T>() != null);
            var rand = Random.Range(0, GOs.Count());
            var resultGO = GameObject.Instantiate(GOs[rand]) as GameObject;
            ActiveObjects.Add(resultGO.gameObject);
            resultGO.transform.SetParent(transform);
            return resultGO.GetComponent<T>();
        }

        public List<T> GetAllActiveObjects<T>()
        {
            return ActiveObjects.Select(x => x.GetComponent<T>()).ToList();
        }
        #endregion

        #region Recycling
        internal void RecyclePrefab(GameObject gameObject)
        {
            //var entityGO = gameObject.gameObject;
            ActiveObjects.Remove(gameObject);
            InactiveObjects.Add(gameObject);
            gameObject.transform.position = Vector3.zero;
            gameObject.SetActive(false);
        }
        #endregion
    }
}
