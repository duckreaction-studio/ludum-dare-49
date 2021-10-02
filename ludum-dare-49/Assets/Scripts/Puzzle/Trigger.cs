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
            if (CollisionIsValid(collision.gameObject))
                triggered?.Invoke(this, null);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (CollisionIsValid(other.gameObject))
            {
                Debug.Log(other);
                triggered?.Invoke(this, null);
            }
        }

        public bool CollisionIsValid(GameObject go)
        {
            return _mode == Mode.COLLIDE && IsBall(go) && LayerIsValid(go);
        }

        public static bool IsBall(GameObject go)
        {
            return go.tag == "Ball" || (go.transform.parent && go.transform.parent.tag == "Ball");
        }

        public bool LayerIsValid(GameObject go)
        {
            return go.layer != LayerMask.NameToLayer("IgnoreTrigger");
        }
    }
}