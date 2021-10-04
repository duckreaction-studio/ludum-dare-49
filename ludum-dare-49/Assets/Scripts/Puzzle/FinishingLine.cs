using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Puzzle
{
    public class FinishingLine : MonoBehaviour
    {
        [Inject]
        LevelState _levelState;

        public void OnTriggerEnter(Collider other)
        {
            if (Trigger.IsBall(other.gameObject))
            {
                other.gameObject.GetComponentInParent<Ball.State>().StopPhysics();
                _levelState.BallReachTheEnd();
            }
        }
    }
}