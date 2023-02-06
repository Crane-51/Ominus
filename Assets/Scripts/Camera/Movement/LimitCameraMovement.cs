using System;
using System.Collections.Generic;
using System.Linq;
using Data.CustomExtensionClasses;
using UnityEngine;

namespace CustomCamera
{
    public class LimitCameraMovement : MonoBehaviour
    {
        public List<CameraBorders> Limits { get; set; }

        /// <summary>
        /// Screen height in world space.
        /// </summary>
        private float WorldScreenHeight { get; set; }

        /// <summary>
        /// Screen width in world space.
        /// </summary>
        private float WorldScreenWidth { get; set; }

        private void Start()
        {
            Limits = GameObject.FindGameObjectsWithTag("Border").Where(x=> x.GetComponent<CameraBorders>() != null).Select(x => x.GetComponent<CameraBorders>()).ToList();

            WorldScreenHeight = Camera.main.orthographicSize;
            WorldScreenWidth = WorldScreenHeight / Screen.height * Screen.width;
        }

        public bool CheckIfCameraCanMove(Transform target)
        {
            var lookUpPosition = new Vector2(target.position.x, transform.position.y);

            var item = Limits.FirstOrDefault(x => Math.Abs(target.transform.position.x - x.Position.x) < WorldScreenWidth && Math.Abs(target.transform.position.y - x.Position.y) < WorldScreenHeight);

            if (item != null)
            {
                if (item.Side == General.Enums.BorderSide.Right && lookUpPosition.x + WorldScreenWidth > item.Position.x)
                {
                    return false;
                }
                else if (item.Side == General.Enums.BorderSide.Left && lookUpPosition.x - WorldScreenWidth < item.Position.x)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
