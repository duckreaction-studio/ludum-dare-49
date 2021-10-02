using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Ball.State>().FromComponentInHierarchy(true).AsSingle();

        Container.Bind<Transform>().WithId("Spawn").FromComponentOn(GameObject.FindGameObjectWithTag("Spawn")).AsCached();
        Container.Bind<Transform>().WithId("StartPoint").FromComponentOn(GameObject.FindGameObjectWithTag("StartPoint")).AsCached();

        Container.Bind<LevelState>().FromComponentInHierarchy(false).AsSingle();
    }
}