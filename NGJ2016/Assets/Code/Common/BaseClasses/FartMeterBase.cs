using UnityEngine;
using System.Collections;
using Assets.Code;
using Assets.Code.Common.Objects;

namespace Assets.Code.Common.BaseClasses
{
    public class FartMeterBase : MonoBehaviour
    {
        public float PercentFull { get; set; }
        public Vector2 OffsetMin { get; set; }
        public Vector2 OffsetMax { get; set; }
        public float OffsetMaxXMin { get; set; }

        public float IncreaseSpeed { get; set; }
        public float DecreaseSpeed { get; set; }

        public bool OhShitTriggered { get; set; }

        public FartMeterBase()
        {
            OffsetMin = new Vector2(10, 170);
            OffsetMax = new Vector2(-885, -10);
            OffsetMaxXMin = -600f;

            IncreaseSpeed = 0.15f;
            DecreaseSpeed = 0.3f;

            PercentFull = 0.0f;
            var rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.offsetMax = OffsetMax;
            rectTransform.offsetMin = OffsetMin;
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
            rectTransform.offsetMax = new Vector2(OffsetMax.x - (OffsetMax.x - OffsetMaxXMin) * PercentFull, OffsetMax.y);
        }

        private void OhShit()
        {
            OhShitTriggered = true;
        }
    }
}