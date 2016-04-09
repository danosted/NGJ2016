using System.Runtime.InteropServices;
using Assets.Code.Common;
using Assets.Code.Common.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.PrefabAccess
{
    public class ProjectileHandler : MonoBehaviour
    {
        private PukeBase _puke;
        private FartBase _fart;
        public ManagerBase _projectileManager;
        public ManagerBase ProjectileManager
        {
            get
            {
                if (_projectileManager == null)
                {
                    _projectileManager = ManagerCollection.Instance.GetManager(Constants.PooManager);
                }
                return _projectileManager;
            }
        }

        
        private float _timeSinceLastShit;
        

        public void CheckForPukeAndFartContinous(PukeBase p, FartBase f)
        {
            _puke = p;
            _fart = f;
        }

        void Update()
        {
            var isPuking = Input.GetKey(KeyCode.Mouse0);
            var isFarting = Input.GetKey(KeyCode.Mouse1);
            if (_fart == null)
                return;
            _fart.SourceCharacter.IsFarting = isFarting  && _fart.SourceCharacter.CanFart;

            if (isPuking)
            {
                var mouseDirection = -_fart.SourceCharacter.MovementDirection;
                var zDegree = Mathf.Rad2Deg * (Mathf.Atan(mouseDirection.y / mouseDirection.x));

                _fart.GetComponent<Renderer>().enabled = true;
                _fart.transform.position = _fart.SourceCharacter.transform.position;
                var oppositeMouseDirection =
                    -(Camera.main.ScreenToWorldPoint(Input.mousePosition) - _fart.transform.position);
                var pooProjectile = ProjectileManager.GetPrefabFromType<PooBase>();
                pooProjectile.transform.position = _fart.SourceCharacter.transform.position;
                pooProjectile.transform.rotation = Quaternion.Euler(0, 0, zDegree);
                pooProjectile.Direction = oppositeMouseDirection.normalized;
                _timeSinceLastShit = 0.0f;
            }
            else if (_fart.SourceCharacter.IsFarting)
            {
                _fart.GetComponent<Renderer>().enabled = true;
                _fart.transform.position = _fart.SourceCharacter.transform.position;
                //var mouseDirection = -(Camera.main.ScreenToWorldPoint(Input.mousePosition) - _fart.transform.position);
                var mouseDirection = -_fart.SourceCharacter.MovementDirection;
                var zDegree = Mathf.Rad2Deg*(Mathf.Atan(mouseDirection.y/mouseDirection.x));

                _fart.transform.rotation = Quaternion.Euler(0, 0, zDegree);
                mouseDirection.z = 0;
                Debug.DrawRay(_fart.SourceCharacter.transform.position, mouseDirection);
                _fart.transform.Translate(mouseDirection.normalized*_fart.Offset);

                if (_fart.SourceCharacter.FartMeter.OhShitTriggered && _timeSinceLastShit > 0.5f)
                {
                    _fart.GetComponent<Renderer>().enabled = true;
                    _fart.transform.position = _fart.SourceCharacter.transform.position;
                    var oppositeMouseDirection =
                        -(Camera.main.ScreenToWorldPoint(Input.mousePosition) - _fart.transform.position);
                    var pooProjectile = ProjectileManager.GetPrefabFromType<PooBase>();
                    pooProjectile.transform.position = _fart.SourceCharacter.transform.position;
                    pooProjectile.transform.rotation = Quaternion.Euler(0, 0, zDegree);
                    pooProjectile.Direction = oppositeMouseDirection.normalized;
                    _timeSinceLastShit = 0.0f;
                }
                else
                {
                    _timeSinceLastShit += Time.deltaTime;
                }
            }
            else
            {
                _puke.GetComponent<Renderer>().enabled = false;
                _fart.GetComponent<Renderer>().enabled = false;
            }
        }
    }
}
