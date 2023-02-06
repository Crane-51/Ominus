using UnityEngine;

namespace General.State
{
    /// <summary>
    /// Core state for all actions.
    /// </summary>
    public abstract class State : MonoBehaviour
    {
        /// <summary>
        /// Gets or sets state priority.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets state controller.
        /// </summary>
        public StateController controller { get; protected set; }

        /// <summary>
        /// Gets or sets design controller
        /// </summary>
        protected DesignController designController { get; set; }

        /// <summary>
        /// Initializes components and parameters.
        /// </summary>
        protected virtual void Initialization_State()
        {
            DiContainerLibrary.DiContainer.DiContainerInitializor.RegisterObject(this);
            designController = GetComponent<DesignController>();
            controller = GetComponent<StateController>();
        }

        /// <summary>
        /// Starts on beginning of the state.
        /// </summary>
        public virtual void OnEnter_State()
        {
            if (designController != null)
            {
                designController.StartTask(this);
            }
			//if (controller.Id == "SkeletonArcher")
			//{
			//	Debug.Log("Enter: " + this);
			//}
		}

        /// <summary>
        /// Checks when is the best time for state to become active.
        /// </summary>
        public virtual void Update_State()
        {
        }

        /// <summary>
        /// While state is active.
        /// </summary>
        public virtual void WhileActive_State()
        {
        }

        /// <summary>
        /// Happens when state is being swapped by other state.
        /// </summary>
        public virtual void OnExit_State()
        {
            if (this.designController != null)
            {
                this.designController.StopTask(this);
            }
			//if (controller.Id == "SkeletonArcher")
			//{
			//	Debug.Log("Exit: " + this);
			//}
		}

        // Use this for initialization
        void Start()
        {
            Initialization_State();
        }

        // Update is called once per frame
        void Update()
        {
            Update_State();
        }

        public void ActivateThisState(bool forceSwapIt = false)
        {
            if (!forceSwapIt)
            {
                controller.SwapState(this);
            }
            else
            {
                controller.ForceSwapState(this);
            }
        }
    }
}
