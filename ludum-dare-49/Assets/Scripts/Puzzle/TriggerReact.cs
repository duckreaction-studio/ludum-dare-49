using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    public class TriggerReact : MonoBehaviour
    {
        public enum State { Idle, Action, Cancel };

        [SerializeField]
        Trigger[] _triggers;
        [SerializeField]
        protected float _animationDuration = 0.3f;
        [SerializeField]
        protected float _cancelAnimationDuration = 0.3f;

        protected Renderer _renderer;
        public new Renderer renderer
        {
            get
            {
                if (_renderer == null)
                {
                    _renderer = GetComponentInChildren<Renderer>();
                }
                return _renderer;
            }
        }

        public Bounds bounds
        {
            get => renderer.bounds;
        }

        protected State _state = State.Idle;

        void Start()
        {
            foreach (var trigger in _triggers)
                trigger.triggered += OnTriggered;
        }

        void OnTriggered(object sender, EventArgs e)
        {
            if (_state == State.Idle)
                DoAction();
        }

        protected virtual void DoAction()
        {
            throw new NotImplementedException();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_state == State.Action && IsBlock(other.gameObject))
                CancelAction();
        }

        protected virtual void CancelAction()
        {
            throw new NotImplementedException();
        }

        private bool IsBlock(GameObject gameObject)
        {
            return gameObject.layer == LayerMask.NameToLayer("Block");
        }

        void OnDestroy()
        {
            foreach (var trigger in _triggers)
                if (trigger)
                    trigger.triggered -= OnTriggered;
        }
    }
}