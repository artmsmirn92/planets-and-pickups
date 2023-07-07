using mazing.common.Runtime.Ticker;
using Zenject;

namespace MiniPlanetDefense
{
    public class MazingMonoInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IViewGameTicker>().To<ViewGameTicker>().AsSingle();
        }
    }
}