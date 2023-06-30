using UnityEngine;
using Zenject;

namespace MiniPlanetDefense
{
    /// <summary>
    /// A Zenject installer that binds one or more <see cref="MonoBehaviour"/>s already existing in the scene.
    /// </summary>
    public class SceneObjectsMonoBehaviourInstaller : MonoInstaller
    {
        [SerializeField] private MonoBehaviour[] monoBehaviours;
        
        public override void InstallBindings()
        {
            foreach (var monoBehaviour in monoBehaviours)
            {
                var t = monoBehaviour.GetType();
                Container.Bind(t).FromInstance(monoBehaviour).AsSingle();
            }
        }
    }
}