using System;
using System.Collections.Generic;
using System.Linq;
using DiContainerLibrary.DiContainer;
using General.Enums;
using General.State;
using Implementation.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Character.Stats.UI
{
    public class StatsController : HighPriorityState
    {
        public Image ProfileOfSingleStatSlot;

        public Sprite EmptySprite;

        public Sprite FullSprite;

        public PlayerUIStatsForUpdate Stat;

        private static List<PlayerUIStatsForUpdate> StatsForUpdates { get; set; }

        [InjectDiContainter]
        private IGameInformation gameInformation { get; set; }

        private CharacterStatsMono CharacterStatsMono { get; set; }

        private static string PlayerId { get; set; }

        private List<Image> allImages { get; set; }

		private Slider slider;


        protected override void Initialization_State()
        {
            base.Initialization_State();
            CharacterStatsMono = gameInformation.Player.GetComponent<CharacterStatsMono>();
            PlayerId = gameInformation.Player.GetComponent<StateController>().Id;

			//allImages = GetComponentsInChildren<Image>().ToList();
			slider = GetComponent<Slider>();

            StatsForUpdates = new List<PlayerUIStatsForUpdate>();
            StatsForUpdates.Add(PlayerUIStatsForUpdate.Health);
            //StatsForUpdates.Add(PlayerUIStatsForUpdate.Mana);
            //StatsForUpdates.Add(PlayerUIStatsForUpdate.Imagination);
        }

        public override void OnEnter_State()
        {
            base.OnEnter_State();
            StatsForUpdates.Remove(Stat);
            var statData = GetStatsData();

			//if (allImages.Count != statData.max)
			//{
			//    InitializeStats(statData);
			//}
			//else
			//{
			//    for (int i = 0; i < statData.max; i++)
			//    {
			//        if (i < statData.current)
			//        {
			//            allImages[i].sprite = FullSprite;
			//        }
			//        else
			//        {
			//            allImages[i].sprite = EmptySprite;
			//        }
			//    }
			//}
			slider.value = (float) statData.current / statData.max;

            controller.EndState(this);
        }

        public override void Update_State()
        {
            if (CharacterStatsMono.IsCharacterStatsAvaliable && StatsForUpdates.Count > 0 )
            {
                if (StatsForUpdates.Contains(Stat))
                {
                    controller.SwapState(this);
                }
            }
        }

        public static void AddStat(PlayerUIStatsForUpdate stat, string id)
        {
            if(PlayerId != id)
            {
                return;
            }
            StatsForUpdates.Add(stat);

        }

        private StatsToUse GetStatsData()
        {
            switch(Stat)
            {
                case PlayerUIStatsForUpdate.Health:
                    return new StatsToUse(CharacterStatsMono.CurrentHealth, CharacterStatsMono.MaxHealth);
                case PlayerUIStatsForUpdate.Mana:
                    return new StatsToUse(CharacterStatsMono.CurrentMana, CharacterStatsMono.MaxMana);
                case PlayerUIStatsForUpdate.Imagination:
                    return new StatsToUse(CharacterStatsMono.CurrentImagination, CharacterStatsMono.MaxMana);
            }

            throw new ArgumentException();
        }

        private void InitializeStats(StatsToUse statData)
        {
            foreach (var obj in allImages)
            {
                Destroy(obj.gameObject);
            }
            for (int i = 0; i < statData.max; i++)
            {
                if (i < statData.current)
                {
                    ProfileOfSingleStatSlot.sprite = FullSprite;
                }
                else
                {
                    ProfileOfSingleStatSlot.sprite = EmptySprite;
                }

                allImages.Add(Instantiate(ProfileOfSingleStatSlot, this.transform));
            }
        }

        private class StatsToUse
        {
            public int current { get; set; }
            public int max { get; set; }

            public StatsToUse(int current , int max)
            {
                this.current = current;
                this.max = max;
            }
        }
    }
}
