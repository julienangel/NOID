using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ScriptableObjectArchitecture;
using Unity.Collections;
using UnityEngine;
using Zenject;

public class ReplayManager
{
    public List<ReplayFrame> ReplayFrames { get; private set; }
    
    private CancellationTokenSource _recordingCancellationTokenSource;
    private CancellationTokenSource _playingRecordCancellationTokenSource;
    
    [Inject(Id = nameof(_gameTimeVariable)), ReadOnly] private IntVariable _gameTimeVariable;
    [Inject(Id = nameof(_recordingIntervalVariable)), ReadOnly] private FloatVariable _recordingIntervalVariable;

    [Inject]
    public void Initialize()
    {
        ReplayFrames = new List<ReplayFrame>((int)(_gameTimeVariable.Value / _recordingIntervalVariable.Value));
    }

    private void AddFrame(ReplayFrame replayFrame)
    {
        ReplayFrames.Add(replayFrame);
    }
    
    public async void StartRecording()
    {
        ReplayFrames.Clear();
        float duration = 0f;
        _recordingCancellationTokenSource = new CancellationTokenSource();
        while (!_recordingCancellationTokenSource.Token.IsCancellationRequested && duration < _gameTimeVariable.Value)
        {
            if (GetFrame != null)
            {
                AddFrame(GetFrame.Invoke());
                await Task.Delay((int)(_recordingIntervalVariable.Value * 1000));
                duration += _recordingIntervalVariable.Value;
            }
        }
    }

    public async Task StartPlayRecord(Transform ghostTransform, Action onPlayingEnd = null)
    {
        _playingRecordCancellationTokenSource = new CancellationTokenSource();
        foreach (ReplayFrame frame in ReplayFrames)
        {
            frame.PawnToDeactivated?.gameObject.SetActive(false);
            frame.LocationToActivate?.gameObject.SetActive(true);

            ghostTransform.position = frame.CarLocation;
            ghostTransform.rotation = frame.CarRotation;

            await Task.Delay((int)(_recordingIntervalVariable.Value * 1000));
            if (_playingRecordCancellationTokenSource.Token.IsCancellationRequested)
            {
                return;
            }
        }
        onPlayingEnd?.Invoke();
    }

    public void StopPlayRecord()
    {
        _playingRecordCancellationTokenSource?.Cancel();
    }

    public void StopRecording()
    {
        _recordingCancellationTokenSource?.Cancel();
    }

    public Func<ReplayFrame> GetFrame { get; set; }
}