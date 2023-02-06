using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using General.State;

namespace UniqueComponent.Puzzles
{
    public abstract class OnSolved : HighPriorityState
    {
        /// <summary>
        /// Defines on puzzle solved function.
        /// </summary>
        public virtual void PuzzleSolved()
        {
            controller.SwapState(this);
        }
    }
}
