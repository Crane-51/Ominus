using System.Collections.Generic;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;

namespace Implementation.Custom
{
    public class Activate : HighPriorityState
    {
        /// <summary>
        /// Gets or sets enemy data.
        /// </summary>
        [InjectDiContainter]
        protected IGameInformation gameInformation { get; set; }

        public bool UseTrigger;

        public List<GameObject> ObjectsToActivate;

        protected List<IActivate> statesToActivate { get; set; }

        protected override void Initialization_State()
        {
            base.Initialization_State();
            statesToActivate = new List<IActivate>();

            foreach (var item in ObjectsToActivate)
            {
                statesToActivate.AddRange(item.GetComponents<IActivate>());
            }
        }

        private void OnDestroy()
        {
			//NP: test why is this here??
            //statesToActivate.ForEach(x => x.Activate());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject == gameInformation.Player.gameObject && UseTrigger)
            {
                statesToActivate.ForEach(x => x.Activate());
            }
        }
    }
}
