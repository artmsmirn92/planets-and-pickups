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
        [SerializeField] private InputController   inputController;
        [SerializeField] private PressKeyToStart   pressKeyToStart;
        [SerializeField] private PressKeyToRestart pressKeyToRestart;
        [SerializeField] private LevelsController  levelsController;
        [SerializeField] private SavesController   savesController;
        
        public override void InstallBindings()
        {
            Container.Bind<Player>()           .FromInstance(player)           .AsSingle();
            Container.Bind<InputController>()  .FromInstance(inputController)  .AsSingle();
            Container.Bind<PressKeyToStart>()  .FromInstance(pressKeyToStart)  .AsSingle();
            Container.Bind<PressKeyToRestart>().FromInstance(pressKeyToRestart).AsSingle();
            Container.Bind<LevelsController>() .FromInstance(levelsController) .AsSingle();
            Container.Bind<SavesController>()  .FromInstance(savesController)  .AsSingle();
        }
    }
}