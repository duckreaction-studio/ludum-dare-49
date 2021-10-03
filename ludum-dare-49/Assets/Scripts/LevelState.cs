using DG.Tweening;
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

    event EventHandler ballStartMoving;
    event EventHandler startReplay;

    [SerializeField]
    public float firstAnimationDuration = 2f;
    [SerializeField]
    public float animationDuration = 1f;

    [Inject(Id = "Spawn")]
    Transform _spawn;

    [Inject(Id = "StartPoint")]
    Transform _startPoint;

    public void BallReachTheEnd()
    {
        currentState = State.Replay;
        StartBall();
    }

    [Inject]
    Ball.State _ballState;

    public void Start()
    {
        _ballState.StopPhysics();
        _ballState.transform.position = _spawn.position;
        _ballState.RegisterStartInfo();
        StartBall();
    }

    public void PlayerThrowBall()
    {
        currentState = State.BallMoving;

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
                _ballState.DoImpulse();
            };
        }
    }
}
