using System;
using System.Threading;
using System.Threading.Tasks;
using ScriptableObjectArchitecture;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class CountdownTimerManager : MonoBehaviour
{
    [Header("--- Configuration ---")] 
    [SerializeField] private bool _useTimeSpan;
    [Header("--- Components ---")]
    [SerializeField] private TextMeshProUGUI _countdownTxt;
    [Header("--- SO Variables ---")]
    [SerializeField] private IntVariable _countdownTimeVariable;
    [Header("--- SO Game Events ---")]
    [SerializeField] private GameEvent _eventToRaiseWhenCountdownFinishes;

    private CancellationTokenSource _cts;

    private async void OnEnable()
    {
        _cts = new CancellationTokenSource();
        _eventToRaiseWhenCountdownFinishes.AddListener(StopCountdown);
        await StartCountDown(_cts);
    }

    private void OnDisable()
    {
        _eventToRaiseWhenCountdownFinishes.RemoveListener(StopCountdown);
        _cts.Cancel();
    }
    
    private async Task StartCountDown(CancellationTokenSource cts)
    {
        string ReturnFormattedTime(int i)
        {
            return _useTimeSpan ? $"{i / 60}:{i % 60:00}" : i.ToString();
        }

        int elapsedTime = _countdownTimeVariable.Value;
        while (elapsedTime > 0)
        {
            _countdownTxt.text = ReturnFormattedTime(elapsedTime);
            await Task.Delay(TimeSpan.FromSeconds(1));
            elapsedTime--;
            if (cts.Token.IsCancellationRequested)
            {
                return;
            }
        }
        
        _eventToRaiseWhenCountdownFinishes.RemoveListener(StopCountdown);
        _eventToRaiseWhenCountdownFinishes.Raise();
    }

    private void StopCountdown()
    {
        _cts.Cancel();
    }
}
