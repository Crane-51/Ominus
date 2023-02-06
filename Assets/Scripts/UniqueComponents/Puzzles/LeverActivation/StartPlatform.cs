using UniqueComponent.Platform;
namespace UniqueComponent.Puzzles
{
    public class StartPlatform : OnSolved
    {
        /// <summary>
        /// Gets or sets platform movement component.
        /// </summary>
        private PlatformMovement platformMovement { get; set; }

        protected override void Initialization_State()
        {
            base.Initialization_State();
            platformMovement = GetComponent<PlatformMovement>();
        }

        public override void PuzzleSolved()
        {
            controller.ForceSwapState(platformMovement);
        }
    }
}
