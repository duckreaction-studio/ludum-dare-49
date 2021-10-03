using DG.Tweening;
using System;
using UnityEngine;

namespace Puzzle
{
    public class Rotate : TriggerReact
    {
        [SerializeField]
        Vector3 _rotate = new Vector3(0f, 90f, 0f);

        protected override void Start()
        {
            _data.rotation = transform.localRotation;
            _initialData = _data;
            base.Start();
        }

        protected override void DoAction()
        {
            base.DoAction();
            StartRotation();
        }

        private void StartRotation()
        {
            Quaternion rotation = transform.localRotation;
            rotation *= Quaternion.Euler(_data.direction * _rotate);
            _data.rotation = transform.localRotation;
            _state = State.Action;
            transform.DOLocalRotateQuaternion(rotation, _animationDuration).SetEase(Ease.InBack).onComplete = OnAnimationComplete;
        }

        protected override void CancelAction()
        {
            _state = State.Cancel;
            transform.DOKill();
            transform.DOLocalRotateQuaternion(_data.rotation, _cancelAnimationDuration).SetEase(Ease.InBounce).onComplete = OnCancelAnimationComplete;
        }

        protected override void OnAnimationComplete()
        {
            _data.rotation = transform.localRotation;
            base.OnAnimationComplete();
        }

        protected override void Replay(Data data)
        {
            transform.DOLocalRotateQuaternion(data.rotation, _animationDuration).SetEase(Ease.InBack).onComplete = () =>
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