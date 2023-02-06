using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus
{
    public class MainMenuActions : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene(2);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
