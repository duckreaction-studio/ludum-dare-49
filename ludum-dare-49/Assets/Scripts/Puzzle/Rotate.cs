using DG.Tweening;
using System;
using UnityEngine;

namespace Puzzle
{
    public class Rotate : TriggerReact
    {
        [SerializeField]
        Vector3 _rotate = new Vector3(0f, 90f, 0f);
        [SerializeField]
        int _maxStep = 1;

        int _currentDirection = 1;
        int _currentStep = 0;
        Quaternion _previousRotation;

        protected override void DoAction()
        {
            StartRotation();
        }

        private void IncreaseStepAndChangeDirection()
        {
            _currentStep++;
            if (_currentStep >= _maxStep)
            {
                _currentDirection = -_currentDirection;
                _currentStep = 0;
            }
        }

        private void StartRotation()
        {
            Quaternion rotation = transform.localRotation;
            rotation *= Quaternion.Euler(_currentDirection * _rotate);
            _previousRotation = transform.localRotation;
            _state = State.Action;
            transform.DOLocalRotateQuaternion(rotation, _animationDuration).SetEase(Ease.InBack).onComplete = OnAnimationComplete;
        }

        private void OnAnimationComplete()
        {
            _state = State.Idle;
            IncreaseStepAndChangeDirection();
        }

        protected override void CancelAction()
        {
            _state = State.Cancel;
            transform.DOKill();
            transform.DOLocalRotateQuaternion(_previousRotation, _cancelAnimationDuration).SetEase(Ease.InBounce).onComplete = OnCancelAnimationComplete;
        }

        private void OnCancelAnimationComplete()
        {
            _state = State.Idle;
        }
    }
}