using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButtons : MonoBehaviour {

    public string Main_Menu_Scene_Name = "MainMenu";

    public void OnMainMenuClicked()
    {
        SceneManager.LoadScene(Main_Menu_Scene_Name);
    }

    public void OnContinueClick()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
