using DiContainerLibrary.DiContainer;
using Implementation.Data;
using UnityEngine;

namespace Assets.Plugins.DiContainer
{
    public class ContainerInjection : MonoBehaviour
    {
        private void Awake()
        {
            DiContainerInitializor.Container = new System.Collections.Generic.Dictionary<System.Type, ContainerData>();
            DiContainerInitializor.Container.BindInstance<IMovementData, MovementData>().AsTransient();
            DiContainerInitializor.Container.BindInstance<ICameraData, CameraData>().AsSingle();
            DiContainerInitializor.Container.BindInstance<IMechanicsData, MechanicsData>().AsTransient();
            DiContainerInitializor.Container.BindInstance<IEnemyData, EnemyData>().AsTransient();
            DiContainerInitializor.Container.BindInstance<IPlayerKeybindsData, PlayerKeybindsData>().AsSingle();
            DiContainerInitializor.Container.BindInstance<ICharacterStats, CharacterStats>().AsTransient();
            DiContainerInitializor.Container.BindInstance<IStats, Stats>().AsTransient();
            DiContainerInitializor.Container.BindInstance<IWholeDialogBoxData, WholeDialogData>().AsTransient();
            DiContainerInitializor.Container.BindInstance<IPhysicsOverlap, PhysicsOverlap>().AsSingle();
            DiContainerInitializor.Container.BindInstance<IGameInformation, GameInformation>().AsSingle();
            DiContainerInitializor.Container.BindInstance<IInventoryData, InventoryData>().AsSingle();
            DiContainerInitializor.Container.BindInstance<ISlotData, SlotData>().AsTransient();
        }
    }
}
