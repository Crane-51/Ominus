using System.Collections.Generic;
using System.Linq;
using DiContainerLibrary.DiContainer;
using UnityEngine;

namespace Menus
{
    public class GeneralMenu : MonoBehaviour
    {
        public float TotalSegments;
        public float SpacingSegments;
        public float WidthScale;
        public float HeightScale;
        public List<GameObject> Elements;
        public List<int> NumberOfSegments;

        [Range(0,1)]
        public float OffsetFromCenterY;

        protected float Width { get; set; }
        protected float Height { get; set; }

        private void Awake()
        {
            Width = Screen.width / WidthScale;
            Height = Screen.height / HeightScale;

            transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Width);
            transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Height);
            transform.transform.localPosition = new Vector2(0, Screen.height - (Screen.height / OffsetFromCenterY));


            SpawnScreen();
        }

        public void SpawnScreen()
        {

            var segmentSize = Height / (TotalSegments+ (SpacingSegments*NumberOfSegments.Count));

            for (int i = 0; i < Elements.Count; i++)
            {
                //Elements[i].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Width);
                Elements[i].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, segmentSize* NumberOfSegments[i]);


                var button = Instantiate(Elements[i], transform);

                var newSegmentPosition = i>0 ? NumberOfSegments.GetRange(0, i).Sum(x => x) : 0;

                button.transform.localPosition = new Vector2(0, (Height/2) - (segmentSize * (newSegmentPosition + (i * SpacingSegments))));
            }
        }
    }
}
