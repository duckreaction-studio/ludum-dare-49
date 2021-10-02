using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Puzzle
{
    public class Trigger : MonoBehaviour, IPointerClickHandler
    {
        public enum Mode { CLICK, COLLIDE }

        [SerializeField]
        Mode _mode = Mode.CLICK;

        public event EventHandler triggered;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_mode == Mode.CLICK)
                triggered?.Invoke(this, null);

        }

        public void OnCollisionEnter(Collision collision)
        {
            if (_mode == Mode.COLLIDE && collision.gameObject.tag == "Ball")
                triggered?.Invoke(this, null);
        }
    }
}