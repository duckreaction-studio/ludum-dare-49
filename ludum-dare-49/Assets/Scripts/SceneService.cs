using DG.Tweening;
using DuckReaction.Common;
using DuckReaction.Common.Container;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneService : MonoBehaviour
{
    [SerializeField]
    string[] _firstScenes = { "Scenes/Home" };

    [SerializeField]
    Image _imageEffect;

    [SerializeField]
    float _transitionDuration = 0.4f;

    [SerializeField]
    Color _color;

    List<string> _scenesToUnload;
    List<string> _scenesToLoad;

    void Start()
    {
        _color.a = 1;
        _imageEffect.color = _color;

        if (SceneManager.sceneCount == 1)
            StartSceneTransition(new string[0], _firstScenes, false);
        else
            TransitionEndAnimation();
    }

    public void StartSceneTransition(string[] scenesToUnload, string[] scenesToLoad, bool fadeIn = true)
    {
        _scenesToUnload = new List<string>(scenesToUnload);
        _scenesToLoad = new List<string>(scenesToLoad);

        if (fadeIn)
            TransitionStartAnimation();
        else
            ProcessNextScene();
    }

    private void OnSceneLoadedOrUnloaded(AsyncOperation op)
    {
        ProcessNextScene();
    }

    private void ProcessNextScene()
    {
        if (_scenesToUnload.Count == 0 && _scenesToLoad.Count == 0)
        {
            TransitionEndAnimation();
        }
        else if (_scenesToUnload.Count == 0)
        {
            LoadNextScene();
        }
        else
        {
            UnloadNextScene();
        }
    }

    private void UnloadNextScene()
    {
        var scene = _scenesToUnload.Shift();
        SceneManager.UnloadSceneAsync(scene).completed += OnSceneLoadedOrUnloaded;
    }

    private void LoadNextScene()
    {
        var scene = _scenesToLoad.Shift();
        SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive).completed += OnSceneLoadedOrUnloaded;
    }

    [ContextMenu("Test transition start animation")]
    private void TransitionStartAnimation()
    {
        _imageEffect.DOColor(_color, _transitionDuration).SetEase(Ease.OutCubic).onComplete += ProcessNextScene;
    }

    [ContextMenu("Test transition end animation")]
    private void TransitionEndAnimation()
    {
        Color color = _color;
        color.a = 0;
        _imageEffect.DOColor(color, _transitionDuration).SetEase(Ease.OutCubic).onComplete += OnTransitionComplete;
    }

    private void OnTransitionComplete()
    {
        Debug.Log("Transition complete");
    }
}
