using System;
using ScriptableObjectArchitecture;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CanvasManager : MonoBehaviour
{
    [Header("--- UI References ---")] 
    [Space]
    [Header("--- StartGameUI ---")]
    [SerializeField] private GameObject _startGameGroupUI;
    [SerializeField] private Button _startGameBtn;
    [Header("Countdown Timer UI")]
    [SerializeField] private CountdownTimerManager _countdownTimerGroupUI;
    [Header("--- Main UI ---")]
    [SerializeField] private GameObject _mainGroupUI;
    [SerializeField] private IndicatorArrow _indicatorArrow;
    [SerializeField] private Button _endGameBtn;
    [Header("--- End Game UI ---")]
    [SerializeField] private GameObject _endGameGroupUI;
    [SerializeField] private Button _restartBtn;
    [SerializeField] private Button _replayBtn;
    
    [Inject(Id = nameof(_gameStartedEvent)), ReadOnly] private GameEvent _gameStartedEvent;
    [Inject(Id = nameof(_gameFinishedEvent)), ReadOnly] private GameEvent _gameFinishedEvent;
    [Inject(Id = nameof(_replayStartedEvent)), ReadOnly] private GameEvent _replayStartedEvent;
    [Inject(Id = nameof(_replayFinishedEvent)), ReadOnly] private GameEvent _replayFinishedEvent;
    [Inject(Id = nameof(_countdownStartedEvent)), ReadOnly] private GameEvent _countdownStartedEvent;
    [Inject(Id = nameof(_countdownFinishedEvent)), ReadOnly] private GameEvent _countdownFinishedEvent;
    
    private void OnEnable()
    {
        _gameStartedEvent.AddListener(OnGameStart);
        _gameFinishedEvent.AddListener(OnGameFinish);
        
        _countdownStartedEvent.AddListener(OnCountdownStart);
        _countdownFinishedEvent.AddListener(OnCountdownFinish);
        
        _replayStartedEvent.AddListener(OnReplayStart);
        _replayFinishedEvent.AddListener(OnReplayFinish);
        
        _startGameBtn.onClick.AddListener(_countdownStartedEvent.Raise);
        _endGameBtn.onClick.AddListener(_gameFinishedEvent.Raise);
        _restartBtn.onClick.AddListener(_countdownStartedEvent.Raise);
        _replayBtn.onClick.AddListener(_replayStartedEvent.Raise);
    }

    private void OnDisable()
    {
        _gameStartedEvent.RemoveListener(OnGameStart);
        _gameFinishedEvent.RemoveListener(OnGameFinish);
        
        _countdownStartedEvent.RemoveListener(OnCountdownStart);
        _countdownFinishedEvent.RemoveListener(OnCountdownFinish);
        
        _replayStartedEvent.RemoveListener(OnReplayStart);
        _replayFinishedEvent.RemoveListener(OnReplayFinish);
        
        _startGameBtn.onClick.RemoveListener(_countdownStartedEvent.Raise);
        _endGameBtn.onClick.RemoveListener(_gameFinishedEvent.Raise);
        _restartBtn.onClick.RemoveListener(_countdownStartedEvent.Raise);
        _replayBtn.onClick.RemoveListener(_replayStartedEvent.Raise);
    }

    private void OnGameStart()
    {
        _mainGroupUI.SetActive(true);
        _indicatorArrow.gameObject.SetActive(true);
    }

    private void OnGameFinish()
    {
        _mainGroupUI.SetActive(false);
        _endGameGroupUI.SetActive(true);
    }

    private void OnCountdownStart()
    {
        _countdownTimerGroupUI.gameObject.SetActive(true);
        _startGameGroupUI.SetActive(false);
        _mainGroupUI.SetActive(false);
        _endGameGroupUI.SetActive(false);
    }

    private void OnCountdownFinish()
    {
        _countdownTimerGroupUI.gameObject.SetActive(false);
        _gameStartedEvent.Raise();
    }

    private void OnReplayStart()
    {
        _endGameGroupUI.SetActive(false);
    }

    private void OnReplayFinish()
    {
        _endGameGroupUI.SetActive(true);
    }
}
