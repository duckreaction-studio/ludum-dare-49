using DG.Tweening;
using DuckReaction.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelState : MonoBehaviour
{
    public enum State { UserPlaying, BallMoving, Replay }

    public State currentState
    {
        get; private set;
    } = State.UserPlaying;

    public event EventHandler ballStartMoving;
    public event EventHandler prepareReplay;
    public event EventHandler startReplay;
    public event EventHandler reset;

    [SerializeField]
    public float firstAnimationDuration = 2f;
    [SerializeField]
    public float animationDuration = 1f;

    [Inject(Id = "Spawn")]
    Transform _spawn;

    [Inject(Id = "StartPoint")]
    Transform _startPoint;

    [Inject]
    Ball.State _ballState;

    [Inject]
    SignalBus _signalBus;

    public void Start()
    {
        _ballState.StopPhysics();
        _ballState.transform.position = _spawn.position;
        _ballState.RegisterStartInfo();
        _ballState.gameObject.SetActive(true);
        StartBall();
    }

    public void RestartLevel()
    {
        _signalBus.Fire(new GameEvent(GameEventType.Restart));
        currentState = State.UserPlaying;
        StartBall();
    }

    public void ResetLevel()
    {
        _signalBus.Fire(new GameEvent(GameEventType.Reset));
        RestartLevel();
        reset?.Invoke(this, null);
    }

    public void PlayerThrowBall()
    {
        _signalBus.Fire(new GameEvent(GameEventType.PlayerThrowBall));
        currentState = State.BallMoving;
        ballStartMoving?.Invoke(this, null);
    }

    public void BallReachTheEnd()
    {
        _signalBus.Fire(new GameEvent(GameEventType.Win));
        currentState = State.Replay;
        StartBall();
        prepareReplay?.Invoke(this, null);
    }

    private void StartBall()
    {
        _ballState.ResetToStart();
        if (currentState == State.UserPlaying)
        {
            _ballState.transform.DOMove(_startPoint.position, firstAnimationDuration).SetEase(Ease.OutCubic).onComplete = () =>
            {
                _ballState.StartPhysics();
            };
        }
        else
        {
            _ballState.transform.DOMove(_startPoint.position, animationDuration).SetEase(Ease.InCubic).onComplete = () =>
            {
                startReplay?.Invoke(this, null);
                _ballState.DoImpulse();
            };
        }
    }
}
