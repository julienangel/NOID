using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ReplayManager>().AsSingle().NonLazy();
    }
}