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

        protected override void Start()
        {
            _data.position = transform.localPosition;
            _initialData = _data;
            base.Start();
        }

        protected override void DoAction()
        {
            base.DoAction();
            StartTranslation();
        }

        private void StartTranslation()
        {
            Vector3 movement = transform.rotation * _direction;
            movement *= _data.direction;
            if (_relative)
            {
                movement.Scale(bounds.size);
            }
            _data.position = transform.localPosition;
            _state = State.Action;
            transform.DOLocalMove(transform.localPosition + movement, _animationDuration).SetEase(Ease.InBack).onComplete = OnAnimationComplete;
        }

        protected override void CancelAction()
        {
            _state = State.Cancel;
            transform.DOKill();
            transform.DOLocalMove(_data.position, _cancelAnimationDuration).SetEase(Ease.InBounce).onComplete = OnCancelAnimationComplete;
        }

        protected override void OnAnimationComplete()
        {
            _data.position = transform.localPosition;
            base.OnAnimationComplete();
        }

        protected override void Replay(Data data)
        {
            transform.DOLocalMove(data.position, _animationDuration).SetEase(Ease.InBack).onComplete = () =>
            {
                OnReplayComplete(data);
            };
        }

        private void OnReplayComplete(Data data)
        {
            _data = data;
        }
    }
}