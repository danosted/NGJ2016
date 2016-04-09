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
            if (_fart == null)
                return;

            if (_fart.SourceCharacter.IsFarting || _fart.SourceCharacter.IsShitting)
            {
                _fart.GetComponent<Renderer>().enabled = true;
                var audioFart = _fart.GetComponent<AudioSource>();
                if (!audioFart.isPlaying)
                {
                    audioFart.Play();
                }
                _fart.transform.position = _fart.SourceCharacter.transform.position;
                var mouseDirection = -(Camera.main.ScreenToWorldPoint(Input.mousePosition) - _fart.transform.position);
                var zDegree = Mathf.Rad2Deg*(Mathf.Atan(mouseDirection.y/mouseDirection.x));

                _fart.transform.rotation = Quaternion.Euler(0, 0, zDegree);
                mouseDirection.z = 0;
                _fart.transform.Translate(mouseDirection.normalized*_fart.Offset);

                if (_fart.SourceCharacter.IsShitting && _timeSinceLastShit > 0.5f)
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
                var audioFart = _fart.GetComponent<AudioSource>();
                audioFart.Stop();
                _puke.GetComponent<Renderer>().enabled = false;
                _fart.GetComponent<Renderer>().enabled = false;
            }
        }
    }
}
