using System.Collections.Generic;
using System.Linq;
using Character.Stats;
using DiContainerLibrary.DiContainer;
using Secret;
using UnityEngine;
using UnityEngine.SceneManagement;
using General.State;

namespace Implementation.Data.Runtime
{
    public class CheckpointController : MonoBehaviour
    {
        public static CheckpointController singleton { get; set; }

        private string CurrentLevel { get; set; }

        /// <summary>
        /// Gets or sets enemy data.
        /// </summary>
        [InjectDiContainter]
        private IGameInformation gameInformation { get; set; }

        /// <summary>
        /// Gets or sets position of checkpoint.
        /// </summary>
        private Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets all puzzles with.
        /// </summary>
        public Dictionary<string, bool> Puzzles { get; set; }

        /// <summary>
        /// Gets or sets all alive enemies.
        /// </summary>
        public List<string> Enemies { get; set; }

        /// <summary>
        /// Gets or sets secrets found.
        /// </summary>
        public List<string> Secrets { get; set; }

        /// <summary>
        /// Gets or sets player stats.
        /// </summary>
        private Stats playerStats { get; set; }

		private bool saved = false;

        private void Awake()
        {
            DontDestroyOnLoad(transform);
            if (FindObjectsOfType(GetType()).Length > 1)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            Puzzles = new Dictionary<string, bool>();
            Enemies = new List<string>();
            DiContainerInitializor.RegisterObject(this);
            singleton = this;
            CurrentLevel = SceneManager.GetActiveScene().name;
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        public void LoadCheckpoint()
        {
			if (gameInformation.Player == null)
			{
				SceneManager.sceneLoaded -= OnLevelFinishedLoading;
				Destroy(this);
				return;
			}

			if (!saved)
			{
				return;
			}

            if(CurrentLevel != SceneManager.GetActiveScene().name) SaveCheckpoint(gameInformation.Player.transform);

            gameInformation.Player.transform.position = Position;
            //var allPuzzles = GameObject.FindGameObjectsWithTag("Puzzle").Where(x => x.GetComponent<PuzzleController>() != null).Select(x => x.GetComponent<PuzzleController>());
            var allEnemies = GameObject.FindGameObjectsWithTag("Enemy").Where(x => x.GetComponent<CharacterStatsMono>() != null).Select(x => x.GetComponent<CharacterStatsMono>());

            //foreach (var item in allPuzzles)
            //{
            //    if (Puzzles.ContainsKey(item.name))
            //    {
            //        item.IsSolved = Puzzles[item.name];
            //    }
            //}

            foreach (var enemy in allEnemies)
            {
                if (!Enemies.Contains(enemy.gameObject.name))
                {
                    Destroy(enemy.gameObject);
                }
            }

            var playerCurrentStats = gameInformation.Player.GetComponent<Character.Stats.CharacterStatsMono>();
            if (playerCurrentStats != null)
            {
                var stats = gameInformation.Player.GetComponent<Character.Stats.CharacterStatsMono>();
                stats.SetHealth(playerStats.CurrentHealth);
                ////stats.CurrentMana = playerStats.CurrentMana;
                ////stats.CurrentImagination = playerStats.CurrentImagination;
                ////stats.UpdateStatsForUI();
            }

            var secretsToDestroy = GameObject.FindGameObjectsWithTag("Secret").Where(x => !Secrets.Contains(x.gameObject.name)).ToList();

            secretsToDestroy.ForEach(x => SecretCounter.SecretFound(x.name));
            secretsToDestroy.ForEach(x => Destroy(x));

        }

        public void SaveCheckpoint(Transform checkPoint)
        {
			saved = true; 

            if (Position == checkPoint.position) return;

            CurrentLevel = SceneManager.GetActiveScene().name;

            Position = checkPoint.position;
            //Puzzles = GameObject.FindGameObjectsWithTag("Puzzle").Where(x=> x.GetComponent<PuzzleController>() != null).Select(x => x.GetComponent<PuzzleController>()).ToDictionary(x => x.name, y => y.IsSolved);
            //Enemies = GameObject.FindGameObjectsWithTag("Enemy").Where(x => x.GetComponent<CharacterStatsMono>() != null).Select(x => x.gameObject.name).ToList();
            Enemies = GameObject.FindGameObjectsWithTag("Enemy").Where(x => !(x.GetComponent<StateController>().ActiveHighPriorityState is CharacterIsDead)).Select(x => x.gameObject.name).ToList();
            Secrets = GameObject.FindGameObjectsWithTag("Secret").Select(x => x.gameObject.name).ToList();

            var stats = gameInformation.Player.GetComponent<Character.Stats.CharacterStatsMono>();
            if (stats == null) return;

            playerStats = new Stats()
            {
                CurrentHealth = stats.CurrentHealth,
                ////CurrentImagination = stats.CharacterStats.CurrentImagination,
                ////CurrentMana = stats.CharacterStats.CurrentMana
            };

        }

        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            DiContainerInitializor.RegisterObject(this);

            if (gameInformation.Player == null)
            {
                SceneManager.sceneLoaded -= OnLevelFinishedLoading;
                Destroy(this);
            }
            else
            {
                //LoadCheckpoint();
            }
        }
    }
}
