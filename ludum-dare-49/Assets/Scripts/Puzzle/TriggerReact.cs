using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    public class TriggerReact : MonoBehaviour
    {
        public enum State { Idle, Action, Cancel };

        public struct Data
        {
            public int direction;
            public int step;
            public Vector3 position;
            public Quaternion rotation;

            public Data(int direction)
            {
                this.direction = direction;
                step = 0;
                position = new Vector3();
                rotation = new Quaternion();
            }
        }

        public struct TimeData
        {
            public float time;
            public Data data;
        }

        [SerializeField]
        Trigger[] _triggers;
        [SerializeField]
        protected float _animationDuration = 0.3f;
        [SerializeField]
        protected float _cancelAnimationDuration = 0.3f;
        [SerializeField]
        int _maxStep = 1;

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
        protected Data _data = new Data(1);
        protected List<TimeData> _timeDataList;

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

        protected virtual void IncreaseStepAndChangeDirection()
        {
            _data.step++;
            if (_data.step >= _maxStep)
            {
                _data.direction = -_data.direction;
                _data.step = 0;
            }
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