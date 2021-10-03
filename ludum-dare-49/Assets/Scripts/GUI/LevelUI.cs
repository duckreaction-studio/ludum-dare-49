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

        public void ResetLevel()
        {
            _levelState.ResetLevel();
        }

        public void RestartLevel()
        {
            _levelState.RestartLevel();
        }
    }
}