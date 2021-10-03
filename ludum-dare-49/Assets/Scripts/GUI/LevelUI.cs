using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GUI
{
    public class LevelUI : MonoBehaviour
    {
        [Inject]
        LevelState _levelState;

        [Inject]
        SceneService _sceneService;

        [SerializeField]
        RectTransform _nextLevelLayout;
        [SerializeField]
        float _animationDuration = 0.5f;

        float _targetY;

        public void Awake()
        {
            _targetY = _nextLevelLayout.anchoredPosition.y;
            SetAnchorY(_nextLevelLayout, -400);
        }

        public void Start()
        {
            _levelState.startReplay += OnStartReplay;
        }

        public void ResetLevel()
        {
            _levelState.ResetLevel();
        }

        public void RestartLevel()
        {
            _levelState.RestartLevel();
        }

        private void OnStartReplay(object sender, EventArgs e)
        {
            _nextLevelLayout.DOAnchorPosY(_targetY, _animationDuration).SetEase(Ease.OutCubic);
        }

        public void NextLevel()
        {
            if (_levelState.currentState == LevelState.State.Replay)
            {
                string[] unload = { "Scenes/UI/LevelUI", "Scenes/Tests/TestLevel01" };
                string[] load = { "Scenes/Tests/TestLevel01", "Scenes/UI/LevelUI" };
                _sceneService.StartSceneTransition(unload, load);
            }
        }

        private void SetAnchorY(RectTransform rect, float targetY)
        {
            var pos = rect.anchoredPosition;
            pos.y = targetY;
            rect.anchoredPosition = pos;
        }
    }
}