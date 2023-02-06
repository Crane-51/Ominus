using DiContainerLibrary.DiContainer;
using Implementation.Data;
using UnityEngine;

namespace Assets.Scripts.Camera.Movement
{
    class UICameraFollowObject : MonoBehaviour
    {
        [InjectDiContainter]
        /// Holds all value about camera movement.
        private ICameraData cameraData { get; set; }

        /// <summary>
        /// Gets or sets object that is being followed by camera.
        /// </summary>
        static Transform ActiveObjectToFollow { get; set; }

        /// <summary>
        /// Gets or sets player component
        /// </summary>
        private Transform player { get; set; }


        // Use this for initialization
        void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            ActiveObjectToFollow = player;
            cameraData = SaveAndLoadData<ICameraData>.LoadSpecificData("Camera");
        }

        // Update is called once per frame
        void LateUpdate()
        {
              transform.position = new Vector3(ActiveObjectToFollow.position.x, ActiveObjectToFollow.position.y + cameraData.YAxisOffset, ActiveObjectToFollow.position.z + cameraData.ZAxisOffset);
        }
    }
}
