using ScriptableObjectArchitecture;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SOInstaller", menuName = "Installers/SOInstaller")]
public class SOInstaller : ScriptableObjectInstaller<SOInstaller>
{
    [Header("Variables")]
    [SerializeField] private IntVariable _gameTimeVariable;
    [SerializeField] private FloatVariable _recordingIntervalVariable;
    [SerializeField] private GameObjectVariable _gameobjectToFollowVariable;
    
    [Header("Game Events")]
    [SerializeField] private GameEvent _gameStartedEvent;
    [SerializeField] private GameEvent _gameFinishedEvent;
    [SerializeField] private GameEvent _replayStartedEvent;
    [SerializeField] private GameEvent _replayFinishedEvent;
    [SerializeField] private GameEvent _countdownStartedEvent;
    [SerializeField] private GameEvent _countdownFinishedEvent;

    public override void InstallBindings()
    {
        Container.Bind<IntVariable>().WithId(nameof(_gameTimeVariable)).FromInstance(_gameTimeVariable);
        Container.Bind<FloatVariable>().WithId(nameof(_recordingIntervalVariable)).FromInstance(_recordingIntervalVariable);
        Container.Bind<GameObjectVariable>().WithId(nameof(_gameobjectToFollowVariable)).FromInstance(_gameobjectToFollowVariable);
        
        Container.Bind<GameEvent>().WithId(nameof(_gameStartedEvent)).FromInstance(_gameStartedEvent);
        Container.Bind<GameEvent>().WithId(nameof(_gameFinishedEvent)).FromInstance(_gameFinishedEvent);
        Container.Bind<GameEvent>().WithId(nameof(_replayStartedEvent)).FromInstance(_replayStartedEvent);
        Container.Bind<GameEvent>().WithId(nameof(_replayFinishedEvent)).FromInstance(_replayFinishedEvent);
        Container.Bind<GameEvent>().WithId(nameof(_countdownStartedEvent)).FromInstance(_countdownStartedEvent);
        Container.Bind<GameEvent>().WithId(nameof(_countdownFinishedEvent)).FromInstance(_countdownFinishedEvent);
    }
}