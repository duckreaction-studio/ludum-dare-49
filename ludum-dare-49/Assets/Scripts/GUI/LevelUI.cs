using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
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
                string[] unload = { "Scenes/UI/LevelUI", GetCurrentLevelName() };
                List<string> load = new List<string>();
                var nextLevel = GetNextLevelName();
                if (SceneExist(nextLevel))
                {
                    load.Add(nextLevel);
                    load.Add("Scenes/UI/LevelUI");
                }
                else
                {
                    load.Add("Scenes/MainMenu");
                }
                _sceneService.StartSceneTransition(unload, load.ToArray());
            }
        }

        public bool SceneExist(string name)
        {
            for (var i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                if (SceneUtility.GetScenePathByBuildIndex(i).Contains(name))
                    return true;
            }
            return false;
        }

        public string GetNextLevelName()
        {
            int number = GetCurrentLevelNumber();
            if (number < 0)
                return "";
            number++;
            return string.Format("Scenes/Levels/Level{0:D2}", number);
        }

        public int GetCurrentLevelNumber()
        {
            var currentScene = GetCurrentLevelName();
            var sceneNumber = Regex.Match(currentScene, @"\d+").Value;
            if (!string.IsNullOrEmpty(sceneNumber))
                return int.Parse(sceneNumber);
            return -1;
        }

        public string GetCurrentLevelName()
        {
            int countLoaded = SceneManager.sceneCount;
            for (int i = 0; i < countLoaded; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.path.Contains("Scenes/Levels/Level"))
                    return scene.path.Replace("Assets/", "");
            }
            return "";
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
        Screen.fullScreen = false;
    Application.OpenURL("about:blank");
#else
        Application.Quit();
#endif
        }

        private void SetAnchorY(RectTransform rect, float targetY)
        {
            var pos = rect.anchoredPosition;
            pos.y = targetY;
            rect.anchoredPosition = pos;
        }
    }
}