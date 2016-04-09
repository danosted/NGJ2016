using UnityEngine;
using System.Collections;
using Assets.Code;
using Assets.Code.Common.Objects;

namespace Assets.Code.Common.BaseClasses
{
    public class FartMeterBase : MonoBehaviour
    {
        public float PercentFull { get; set; }
        public float OffsetXMin { get; set; }
        public float OffsetXMax { get; set; }
        //public float OffsetMaxXMin { get; set; }

        public float IncreaseSpeed { get; set; }
        public float DecreaseSpeed { get; set; }

        public bool OhShitTriggered { get; set; }
        
        void Awake()
        {
            //OffsetMaxXMin = -600f;

            IncreaseSpeed = 0.15f;
            DecreaseSpeed = 0.3f;

            PercentFull = 0.0f;
            var rectTransform = gameObject.GetComponent<RectTransform>();
            Debug.Log("rectTransform " + rectTransform);

            OffsetXMax = rectTransform.rect.xMax;
            OffsetXMin = rectTransform.rect.xMin;

            Debug.Log("OffsetXMax " + OffsetXMax);
            Debug.Log("OffsetXMin " + OffsetXMin);

            Debug.Log("rectTransform.offsetMax " + rectTransform.offsetMax);
        }

        public void DecreaseMeter()
        {
            PercentFull -= Time.deltaTime * DecreaseSpeed;
            if (PercentFull < 0)
            {
                OhShitTriggered = false;
                PercentFull = 0;
            }
            Render();
        }

        public void IncreaseMeter()
        {
            PercentFull += Time.deltaTime * IncreaseSpeed;
            if (PercentFull > 1)
            {
                OhShit();
                PercentFull = 1;
            }
            Render();
        }

        private void Render()
        {
            var rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.offsetMax = new Vector2(OffsetXMax - (OffsetXMin - OffsetXMax) * PercentFull, rectTransform.offsetMax.y);
            Debug.Log("rectTransform.offsetMax " + rectTransform.offsetMax);
        }

        private void OhShit()
        {
            OhShitTriggered = true;
        }
    }
}