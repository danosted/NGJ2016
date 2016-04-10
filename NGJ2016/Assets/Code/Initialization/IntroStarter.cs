using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Code.Initialization
{
    public class IntroStarter : MonoBehaviour
    {

        public Transform Hearts;
        public Transform Speech;

        void Update()
        {
            if (Input.anyKey)
            {
                Destroy(Hearts.gameObject);
                SceneManager.LoadScene(1);
                gameObject.SetActive(false);
            }
        }
    }
}
