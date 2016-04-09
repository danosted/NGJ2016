﻿using UnityEngine;
using System.Collections;
using Assets.Code;
using Assets.Code.Common.Objects;

namespace Assets.Code.Common.BaseClasses
{
    public class DisgraceMeterBase : MonoBehaviour
    {
        public float PercentFull { get; set; }
        public float OffsetXMin { get; set; }
        public float OffsetXMax { get; set; }
        //public float OffsetMaxXMin { get; set; }

        public float IncreaseSpeed { get; set; }
        public float DecreaseSpeed { get; set; }
        
        
        void Awake()
        {
            //OffsetMaxXMin = -600f;

            IncreaseSpeed = 0.15f;
            DecreaseSpeed = 0.01f;

            PercentFull = 1.0f;
            var rectTransform = gameObject.GetComponent<RectTransform>();
            Debug.Log("DisgraceMeterBase rectTransform " + rectTransform);

            OffsetXMax = rectTransform.rect.xMax;
            OffsetXMin = rectTransform.rect.xMin;

            Debug.Log("DisgraceMeterBase OffsetXMax " + OffsetXMax);
            Debug.Log("DisgraceMeterBaseOffsetXMin " + OffsetXMin);

            Debug.Log("DisgraceMeterBase rectTransform.offsetMax " + rectTransform.offsetMax);
        }

        public void DecreaseMeter()
        {
            PercentFull -= Time.deltaTime * DecreaseSpeed;
            
            Render();
        }
        
        public void Render()
        {
            if (PercentFull < 0)
            {
                //TODO: You lost
            }
            else if (PercentFull > 1)
            {
                PercentFull = 1f;
            }
            var rectTransform = gameObject.GetComponent<RectTransform>();
            Debug.Log(PercentFull);
            rectTransform.offsetMax = new Vector2(OffsetXMax * PercentFull * 2, rectTransform.offsetMax.y);
        }
    }
}