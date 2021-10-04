using DuckReaction.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GUI
{
    public class Recording : AnimatedPanel
    {
        [Inject]
        SignalBus _signalBus;

        private void Start()
        {
            _signalBus.Subscribe<GameEvent>(OnGameEventReceived);
        }

        private void OnGameEventReceived(GameEvent ge)
        {
            var type = ge.GetType<GameEventType>();
            if (type == GameEventType.PlayerThrowBall)
                Show();
            else if (type == GameEventType.Win || type == GameEventType.Reset || type == GameEventType.Restart)
                Hide();
        }
    }
}