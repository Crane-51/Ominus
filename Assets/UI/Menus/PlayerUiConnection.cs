using Character.Stats;
using DiContainerLibrary.DiContainer;
using General.Enums;
using Implementation.Data;
using Player.Other;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus
{
    public class PlayerUiConnection : MonoBehaviour
    {
        /// <summary>
        /// Gets or sets enemy data.
        /// </summary>
        [InjectDiContainter]
        private IGameInformation gameInformation { get; set; }

        public float ShakeOffset;

        private bool AlreadyDone { get; set; }

        private SwapEffects effects { get; set; }

        public string Start_Click_Scene = "MainMenu";

        private void Start()
        {
             DiContainerInitializor.RegisterObject(this);
            effects = GameObject.FindObjectOfType<SwapEffects>();
        }

        private void Update()
        {
            if (gameInformation.PlayerStateController != null && gameInformation.PlayerStateController.ActiveHighPriorityState is CharacterIsDead && !AlreadyDone)
            {
                AlreadyDone = true;
                effects.OnActivation(RestartFromCheckpoint, EffectDirection.FadeOut);
            }
        }

        private void RestartFromCheckpoint()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void LoadNewScene()
        {
            SceneManager.LoadScene(Start_Click_Scene);
        }
    }
}
