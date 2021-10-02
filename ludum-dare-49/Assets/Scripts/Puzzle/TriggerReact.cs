using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    public class TriggerReact : MonoBehaviour
    {
        [SerializeField]
        Trigger[] _triggers;

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

        void Start()
        {
            foreach (var trigger in _triggers)
                trigger.triggered += OnTriggered;
        }

        void OnTriggered(object sender, EventArgs e)
        {
            DoAction();
        }

        protected virtual void DoAction()
        {
            throw new NotImplementedException();
        }

        void OnDestroy()
        {
            foreach (var trigger in _triggers)
                if (trigger)
                    trigger.triggered -= OnTriggered;
        }
    }
}