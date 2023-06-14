using System;
using ScriptableObjectArchitecture;
using Unity.Collections;
using UnityEngine;
using Zenject;

public class CameraFollow : MonoBehaviour, IUpdater
{
    [Inject(Id = nameof(_gameobjectToFollowVariable)), ReadOnly] private GameObjectVariable _gameobjectToFollowVariable;
    [Inject(Id = nameof(_gameStartedEvent)), ReadOnly] private GameEvent _gameStartedEvent;
    [Inject(Id = nameof(_gameFinishedEvent)), ReadOnly] private GameEvent _gameFinishedEvent;
    [Inject(Id = nameof(_replayStartedEvent)), ReadOnly] private GameEvent _replayStartedEvent;
    [Inject(Id = nameof(_replayFinishedEvent)), ReadOnly] private GameEvent _replayFinishedEvent;

    private Transform _cachedTransform;
    private Transform _cachedTransformToFollow;
    private Vector3 _initialCameraPos;
    private Vector3 _initialCarPos;
    private Vector3 _offset;

    private void Awake()
    {
        _cachedTransform = transform;
        _initialCameraPos = _cachedTransform.position;
        
        _gameStartedEvent.AddListener(OnGameStart);
        _gameFinishedEvent.AddListener(OnGameFinish);

        _replayStartedEvent.AddListener(OnReplayStart);
        _replayFinishedEvent.AddListener(OnReplayFinish);
        
        _gameobjectToFollowVariable.AddListener(OnGameobjectToFollowChanged);
    }

    private void OnDestroy()
    {
        _gameobjectToFollowVariable.RemoveListener(OnGameobjectToFollowChanged);
    }

    private void OnGameobjectToFollowChanged(GameObject toFollow)
    {
        _cachedTransformToFollow = toFollow.transform;
        _initialCarPos = _cachedTransformToFollow.position;
        _offset = _cachedTransform.position - _initialCarPos;
    }

    private void OnGameStart()
    {
        ResetTransforms();
        UpdateManager.Instance.AddBehaviour(this);
    }

    private void OnGameFinish()
    {
        ResetTransforms();
        UpdateManager.Instance.RemoveBehaviour(this);
    }

    private void OnReplayStart()
    {
        ResetTransforms();
        UpdateManager.Instance.AddBehaviour(this);
    }

    private void OnReplayFinish()
    {
        ResetTransforms();
        UpdateManager.Instance.RemoveBehaviour(this);
    }

    private void ResetTransforms()
    {
        _cachedTransform.position = _initialCameraPos;
        _cachedTransformToFollow.position = _initialCarPos;
    }

    public void UpdateNormal(float dt)
    {
        _cachedTransform.position = _cachedTransformToFollow.position + _offset;
    }

    public void UpdateFixed()
    {
    }

    public void UpdateLate()
    {
    }
}