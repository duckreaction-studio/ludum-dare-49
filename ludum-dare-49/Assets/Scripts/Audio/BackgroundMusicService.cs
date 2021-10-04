using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Audio
{
    public class BackgroundMusicService : MonoBehaviour
    {
        [Serializable]
        public class MusicInfo
        {
            public AudioClip clip;
            public bool playOnStart;
            public string levelName;
            public float volume = 1f;

            [HideInInspector]
            public AudioSource source;
        }

        [Inject]
        SceneService _sceneTransition;

        [SerializeField]
        float _fadeIn = 2.0f;
        [SerializeField]
        List<MusicInfo> _infos;

        public void Start()
        {
            foreach (var info in _infos)
            {
                info.source = gameObject.AddComponent<AudioSource>();
                info.source.clip = info.clip;
                info.source.volume = 0f;
                info.source.Play();
                if (info.playOnStart)
                    StartPlaying(info);
            }

            _sceneTransition.sceneLoad += OnSceneLoad;
        }

        private void OnSceneLoad(object sender, string scene)
        {
            foreach (var info in _infos)
            {
                if (info.levelName.Contains(scene) || scene.Contains(info.levelName))
                {
                    StartPlaying(info);
                }
            }
        }

        private void StartPlaying(MusicInfo info)
        {
            info.source.DOFade(info.volume, _fadeIn);
        }
    }
}