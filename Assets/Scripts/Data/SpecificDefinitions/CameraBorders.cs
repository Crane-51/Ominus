using General.Enums;
using UnityEngine;

namespace Data.CustomExtensionClasses
{
    public class CameraBorders : MonoBehaviour
    {
        public Vector2 Position { get; set; }

        public BorderSide Side;

        private void Awake()
        {
            Position = transform.position;
        }


        public void OnTriggerEnter2D(Collider2D collision)
        {
            Destroy(this.gameObject);
        }
    }
}
