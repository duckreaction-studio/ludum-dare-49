using DG.Tweening;
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

        protected override void DoAction()
        {
            Vector3 movement = transform.rotation * _direction;
            movement *= _currentDirection;
            if (_relative)
            {
                movement.Scale(bounds.size);
            }
            transform.DOLocalMove(transform.localPosition + movement, 0.3f).SetEase(Ease.InBack);

            _currentStep++;
            if (_currentStep >= _maxStep)
            {
                _currentDirection = -_currentDirection;
                _currentStep = 0;
            }
        }
    }
}