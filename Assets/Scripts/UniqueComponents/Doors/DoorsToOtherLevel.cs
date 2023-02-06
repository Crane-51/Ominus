using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine.SceneManagement;

namespace LevelLoader
{
    public class DoorsToOtherLevel : HighPriorityState
    {
        public string LevelToLoad;

        [InjectDiContainter]
        private IGameInformation gameInformation { get; set; }

        private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
        {
            if(collision.gameObject == gameInformation.Player)
            {
                FadeInFadeOut.singleton.OnActivation(LoadScene, General.Enums.EffectDirection.FadeOut, 0.8f);
            }
        }


        private void LoadScene()
        {
            SceneManager.LoadScene(LevelToLoad);
        }
    }
}
