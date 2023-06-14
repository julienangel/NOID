using System.Collections.Generic;
using System.Linq;
using ScriptableObjectArchitecture;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Random = UnityEngine.Random;

public class GameRunner : MonoBehaviour
{
    public bool IsOnAssignment { get; set; }
    public Location TargetDestination { get; private set; } 
    
    [Header("--- SO Variables ---")]
    [Inject(Id = nameof(_gameobjectToFollowVariable)), ReadOnly] private GameObjectVariable _gameobjectToFollowVariable;
    
    [Header("--- SO Game Events ---")] 
    [Inject(Id = nameof(_gameStartedEvent)), ReadOnly] private GameEvent _gameStartedEvent;
    [Inject(Id = nameof(_gameFinishedEvent)), ReadOnly] private GameEvent _gameFinishedEvent;
    [Inject(Id = nameof(_replayStartedEvent)), ReadOnly] private GameEvent _replayStartedEvent;
    [Inject(Id = nameof(_replayFinishedEvent)), ReadOnly] private GameEvent _replayFinishedEvent;

    [Header("--- Player ---")] 
    [SerializeField] private PlayerControl _playerControl;
    [SerializeField] private Transform _playerGhostCar;

    [Header("--- Misc ---")] [SerializeField]
    private CameraFollow _cameraFollow;

    private readonly List<NPC> _npcs = new List<NPC>();
    private readonly List<Location> _locations = new List<Location>();

    [Space]
    
    private NPC _currentNpc;

    [Inject, ReadOnly]
    private ReplayManager _replayManager;

    private void OnEnable()
    {
        _replayManager.GetFrame += ReturnFrame;

        _gameStartedEvent.AddListener(OnGameStart);
        _gameFinishedEvent.AddListener(OnGameFinish);

        _replayStartedEvent.AddListener(OnReplayStart);
        _replayFinishedEvent.AddListener(OnReplayStop);
    }

    private void OnDisable()
    {
        _replayManager.GetFrame -= ReturnFrame;

        _gameStartedEvent.RemoveListener(OnGameStart);
        _gameFinishedEvent.RemoveListener(OnGameFinish);

        _replayStartedEvent.RemoveListener(OnReplayStart);
        _replayFinishedEvent.RemoveListener(OnReplayStop);

        OnGameFinish();
        OnReplayStop();
    }

    private ReplayFrame ReturnFrame()
    {
        return new ReplayFrame(_playerControl.CarPosition, _playerControl.CarRotation, _currentNpc, TargetDestination);
    }

    private void ResetState()
    {
        foreach (var npc in _npcs.Where(x => x != null))
        {
            npc.gameObject.SetActive(true);
        }

        TargetDestination = null;
        IsOnAssignment = false;
        SetAllNPCParticles(true);
    }

    public void RegisterNPC(NPC npc)
    {
        _npcs.Add(npc);
        npc.SetParticlesActive(true);
    }

    public void RegisterLocation(Location location)
    {
        _locations.Add(location);
    }

    public void SetLocationForNPC(NPC npc)
    {
        _currentNpc = npc;
        TargetDestination = _locations[Random.Range(0, _locations.Count)];
        TargetDestination.SetActiveLocation(true);
    }

    public void SetAllNPCParticles(bool particlesOn)
    {
        foreach (var npc in _npcs)
        {
            if (npc != null)
            {
                npc.SetParticlesActive(particlesOn);
            }
        }
    }

    private void OnGameStart()
    {
        ResetState();
        _replayManager.StartRecording();
    }

    private void OnGameFinish()
    {
        ResetState();
        _replayManager.StopRecording();
    }

    private async void OnReplayStart()
    {
        if (_playerGhostCar != null)
            _playerGhostCar.gameObject.SetActive(true);
        if (_playerControl != null)
            _playerControl.gameObject.SetActive(false);
        
        _gameobjectToFollowVariable.SetValue(_playerGhostCar.gameObject);
        await _replayManager.StartPlayRecord(_playerGhostCar, () => _replayFinishedEvent.Raise());
    }

    private void OnReplayStop()
    {
        _replayManager.StopPlayRecord();
        if (_playerGhostCar != null)
            _playerGhostCar.gameObject.SetActive(false);
        if (_playerControl != null)
            _playerControl.gameObject.SetActive(true);
    }
}