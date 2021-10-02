using DG.Tweening;
using System;
using UnityEngine;

namespace Puzzle
{
    public class Translate : TriggerReact
    {
        [SerializeField]
        Vector3 _direction = Vector3.forward;
        [SerializeField]
        bool _relative = true;
        [SerializeField]
        int _maxStep = 1;

        int _currentDirection = 1;
        int _currentStep = 0;
        Vector3 _previousPosition;

        protected override void DoAction()
        {
            StartTranslation();
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

        private void StartTranslation()
        {
            Vector3 movement = transform.rotation * _direction;
            movement *= _currentDirection;
            if (_relative)
            {
                movement.Scale(bounds.size);
            }
            _previousPosition = transform.localPosition;
            _actionInProgress = true;
            transform.DOLocalMove(transform.localPosition + movement, 0.3f).SetEase(Ease.InBack).onComplete = OnAnimationComplete;
        }

        private void OnAnimationComplete()
        {
            _actionInProgress = false;
            IncreaseStepAndChangeDirection();
        }

        protected override void CancelAction()
        {
            _actionInProgress = false;
            transform.DOKill();
            transform.DOLocalMove(_previousPosition, 0.2f).SetEase(Ease.InBounce);
        }
    }
}