using UnityEngine;

namespace Menus
{
    public class MainMenuParallax : MonoBehaviour
    {
        private void Update()
        {
            transform.position = new Vector3(transform.position.x + 10 * Time.deltaTime, transform.position.y , transform.position.z);
        }
    }
}
