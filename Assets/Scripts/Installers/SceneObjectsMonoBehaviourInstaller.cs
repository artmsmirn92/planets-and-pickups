using UnityEngine;
using Zenject;

namespace MiniPlanetDefense
{
    /// <summary>
    /// A Zenject installer that binds one or more <see cref="MonoBehaviour"/>s already existing in the scene.
    /// </summary>
    public class SceneObjectsMonoBehaviourInstaller : MonoInstaller
    {
        [SerializeField] private Player            player;
        [SerializeField] private PressKeyToStart   pressKeyToStart;
        [SerializeField] private PressKeyToRestart pressKeyToRestart;
        
        public override void InstallBindings()
        {
            Container.Bind<Player>()           .FromInstance(player)           .AsSingle();
            Container.Bind<PressKeyToStart>()  .FromInstance(pressKeyToStart)  .AsSingle();
            Container.Bind<PressKeyToRestart>().FromInstance(pressKeyToRestart).AsSingle();
            
        }
    }
}