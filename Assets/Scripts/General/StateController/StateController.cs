using UnityEngine;

namespace General.State
{
    /// <summary>
    /// Holds function and values for controlling the states.
    /// </summary>
    public class StateController : MonoBehaviour
    {
        /// <summary>
        /// Gets or sets identifier for data.
        /// </summary>
        public string Id;

        /// <summary>
        /// Gets and sets active state.
        /// </summary>
        public StateForMechanics ActiveStateMechanic { get; set; }

        /// <summary>
        /// Gets and sets active state.
        /// </summary>
        public StateForMovement ActiveStateMovement { get; set; }

        /// <summary>
        /// Gets or sets higher priority state.
        /// </summary>
        public HighPriorityState ActiveHighPriorityState { get; set; }

        // Use this for initialization
        void Awake()
        {
            ActiveHighPriorityState = null;
            ActiveStateMechanic = null;
            ActiveStateMovement = null;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (ActiveHighPriorityState != null)
            {
                ActiveHighPriorityState.WhileActive_State();
            }
            else
            {
                if (ActiveStateMechanic != null)
                {
                    ActiveStateMechanic.WhileActive_State();
                }
                if (ActiveStateMovement != null)
                {
                    ActiveStateMovement.WhileActive_State();
                }
            }
        }

        /// <summary>
        /// Swap state using priority
        /// </summary>
        /// <param name="newState"></param>
        public void SwapState(State newState)
        {
            if (newState == null) return;

            if(newState is HighPriorityState)
            {
                if (ActiveHighPriorityState == null 
                    || (ActiveHighPriorityState.Priority < newState.Priority && ActiveHighPriorityState != newState))
                {
                    ForceSwapState(newState);
                }
            }

            if (ActiveHighPriorityState == null)
            {
                if (newState is StateForMechanics)
                {
                    if (ActiveStateMechanic == null
                        || (ActiveStateMechanic.Priority < newState.Priority && ActiveStateMechanic != newState))
                    {
                        ForceSwapState(newState);
                    }
                }
                else if (newState is StateForMovement)
                {
                    if (ActiveStateMovement == null
                        || (ActiveStateMovement.Priority < newState.Priority && ActiveStateMovement != newState))
                    {
                        ForceSwapState(newState);
                    }
                }
            }
        }

        public void ForceSwapState(State newState)
        {
            if (newState is HighPriorityState)
            {
                if (ActiveHighPriorityState != null) ActiveHighPriorityState.OnExit_State();
                ActiveHighPriorityState = (HighPriorityState)newState;
                ActiveHighPriorityState.OnEnter_State();

                if(ActiveStateMechanic != null)
                {
                    this.EndState(ActiveStateMechanic);
                }
                if(ActiveStateMovement != null)
                {
                    this.EndState(ActiveStateMovement);
                }
            }
            else if (newState is StateForMechanics)
            {
                if (ActiveStateMechanic != null) ActiveStateMechanic.OnExit_State();
                ActiveStateMechanic = (StateForMechanics)newState;
                ActiveStateMechanic.OnEnter_State();
            }
            else if (newState is StateForMovement)
            {
                if (ActiveStateMovement != null) ActiveStateMovement.OnExit_State();
                ActiveStateMovement = (StateForMovement)newState;
                ActiveStateMovement.OnEnter_State();
            }
        }

        public void EndState(State stateToEnd)
        {
            if (stateToEnd == null) return;
            
            if (stateToEnd is HighPriorityState && stateToEnd == ActiveHighPriorityState)
            {
                ActiveHighPriorityState.OnExit_State();
                ActiveHighPriorityState = null;
            }
            if (stateToEnd is StateForMechanics && stateToEnd == ActiveStateMechanic)
            {
                ActiveStateMechanic.OnExit_State();
                ActiveStateMechanic = null;
            }
            else if (stateToEnd is StateForMovement && stateToEnd == ActiveStateMovement)
            {
                ActiveStateMovement.OnExit_State();
                ActiveStateMovement = null;
            }
        }

		public override string ToString()
		{
			string str = "";
			int pFrom;
			int pTo;

			if (ActiveHighPriorityState != null)
			{
				string hps = ActiveHighPriorityState.ToString();
				pFrom = hps.LastIndexOf(".") + 1;
				pTo = hps.LastIndexOf(")");
				str += "HPS: " + hps.Substring(pFrom, pTo - pFrom) + "\n"; 
			}

			if (ActiveStateMechanic != null)
			{
				string mech = ActiveStateMechanic.ToString();
				pFrom = mech.LastIndexOf(".") + 1;
				pTo = mech.LastIndexOf(")");
				str += "Mech: " + mech.Substring(pFrom, pTo - pFrom) + "\n"; 
			}

			if (ActiveStateMovement != null)
			{
				string mov = ActiveStateMovement.ToString();
				pFrom = mov.LastIndexOf(".") + 1;
				pTo = mov.LastIndexOf(")");
				str += "Mov: " + mov.Substring(pFrom, pTo - pFrom) + "\n"; 
			}

			return str;
		}
    }
}
