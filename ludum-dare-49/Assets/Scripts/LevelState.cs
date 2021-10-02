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
    }

    [SerializeField]
    public float animationDuration = 0.3f;

    [Inject(Id = "Spawn")]
    Transform _spawn;

    [Inject(Id = "StartPoint")]
    Transform _startPoint;

    public void BallReachTheEnd()
    {
        StartBall(true);
    }

    [Inject]
    Ball.State _ballState;

    public void Start()
    {
        _ballState.StopPhysics();
        _ballState.transform.position = _spawn.position;
        _ballState.RegisterStartInfo();
        StartBall(false);
    }

    private void StartBall(bool impulse)
    {
        _ballState.ResetToStart();
        _ballState.transform.DOMove(_startPoint.position, animationDuration).SetEase(Ease.InCubic).onComplete = () =>
        {
            _ballState.StartPhysics();
            if (impulse)
                _ballState.GetComponent<Ball.Impulse>().DoImpulse();
        };
    }
}
