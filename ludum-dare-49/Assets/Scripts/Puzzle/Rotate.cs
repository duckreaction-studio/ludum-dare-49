using DG.Tweening;
using System;
using UnityEngine;

namespace Puzzle
{
    public class Rotate : TriggerReact
    {
        [SerializeField]
        Vector3 _rotate = new Vector3(0f, 90f, 0f);

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
    }
}