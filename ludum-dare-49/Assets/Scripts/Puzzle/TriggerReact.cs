using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Puzzle
{
    public class TriggerReact : MonoBehaviour
    {
        #region "data"
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
            public TimeData(float time, Data data)
            {
                this.time = time;
                this.data = data;
            }
        }

        [SerializeField]
        Trigger[] _triggers;
        [SerializeField]
        protected float _animationDuration = 0.3f;
        [SerializeField]
        protected float _cancelAnimationDuration = 0.3f;
        [SerializeField]
        int _maxStep = 1;

        [Inject(Optional = true)]
        protected LevelState _levelState;

        protected Renderer _renderer;
        public Renderer renderer
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
        protected Data _initialData = new Data(1);
        protected List<TimeData> _timeDataList;
        protected float _startTime;
        protected float _actionTime;
        protected Trigger.InputType _inputType;
        protected int _dataIndex = 0;

        #endregion

        protected virtual void Start()
        {
            foreach (var trigger in _triggers)
                trigger.triggered += OnTriggered;

            if (_levelState)
            {
                _levelState.ballStartMoving += OnBallStartMoving;
                _levelState.prepareReplay += PrepareReplay;
                _levelState.startReplay += StartReplay;
                _levelState.reset += ResetToInitialState;
            }
        }

        private void OnBallStartMoving(object sender, EventArgs e)
        {
            _timeDataList = new List<TimeData>();
            _startTime = Time.realtimeSinceStartup;
            _timeDataList.Add(new TimeData(0, _data));
        }

        private void ResetToInitialState(object sender, EventArgs e)
        {
            Replay(_initialData);
        }

        private void PrepareReplay(object sender, EventArgs e)
        {
            _dataIndex = 0;
            Replay(_timeDataList[_dataIndex].data);
        }

        private void StartReplay(object sender, EventArgs e)
        {
            StartCoroutine(ReplayCoroutine());
        }

        private IEnumerator ReplayCoroutine()
        {
            for (++_dataIndex; _dataIndex < _timeDataList.Count; ++_dataIndex)
            {
                var current = _timeDataList[_dataIndex];
                yield return new WaitForSeconds(current.time);
                Replay(current.data);
            }
        }

        void OnTriggered(object sender, Trigger.InputType inputType)
        {
            _inputType = inputType;
            if (_state == State.Idle)
                DoAction();
        }

        protected virtual void DoAction()
        {
            _actionTime = Time.realtimeSinceStartup - _startTime;
        }

        protected virtual void OnAnimationComplete()
        {
            _state = State.Idle;
            IncreaseStepAndChangeDirection();
            if (_inputType == Trigger.InputType.User && _levelState?.currentState == LevelState.State.BallMoving)
                RecordData();
        }

        protected virtual void OnCancelAnimationComplete()
        {
            _state = State.Idle;
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

        protected virtual void RecordData()
        {
            _timeDataList.Add(new TimeData(_actionTime, _data));
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

        protected virtual void Replay(Data data)
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