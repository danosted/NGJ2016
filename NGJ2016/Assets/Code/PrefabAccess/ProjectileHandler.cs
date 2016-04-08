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
        private ManagerBase _projectileManager;

        public void CheckForPukeAndFartContinous(PukeBase p, FartBase f)
        {
            _puke = p;
            _fart = f;
        }

        void Update()
        {
            var isPuking = Input.GetKey(KeyCode.Mouse0);
            var isFarting = Input.GetKey(KeyCode.Mouse1);

            if (isPuking)
            {
                _puke.GetComponent<Renderer>().enabled = true;
                _puke.transform.position = _puke.SourceCharacter.transform.position;
                var mouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _puke.transform.position;
                var zDegree = Mathf.Rad2Deg*(Mathf.Atan(mouseDirection.y/mouseDirection.x));

                _puke.transform.rotation = Quaternion.Euler(0, 0, zDegree);
                mouseDirection.z = 0;
                var length = mouseDirection.sqrMagnitude;
                _puke.transform.Translate(mouseDirection*0.5f);
                _puke.transform.localScale = new Vector3(length*0.5f, 2, 1);
            }
            else if (isFarting)
            {
                Debug.DrawRay(_fart.SourceCharacter.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                _fart.GetComponent<Renderer>().enabled = true;
                _fart.transform.position = _fart.SourceCharacter.transform.position;
                var mouseDirection = -Camera.main.ScreenToWorldPoint(Input.mousePosition) - _fart.transform.position;
                var zDegree = Mathf.Rad2Deg * (Mathf.Atan(mouseDirection.y / mouseDirection.x));

                _fart.transform.rotation = Quaternion.Euler(0, 0, zDegree);
                mouseDirection.z = 0;
                var length = mouseDirection.sqrMagnitude;
                _fart.transform.Translate(mouseDirection * 0.5f);
                _fart.transform.localScale = new Vector3(length * 0.5f, 2, 1);
            }
            else
            {
                _puke.GetComponent<Renderer>().enabled = false;
                _fart.GetComponent<Renderer>().enabled = false;
            }

            var deltaSeconds = (int)(Time.deltaTime * 1000);
        }
    }
}
