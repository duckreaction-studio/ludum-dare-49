using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Puzzle
{
    public class Trigger : MonoBehaviour, IPointerClickHandler
    {
        public enum Mode { Click, Collide }
        public enum InputType { User, Other }

        [SerializeField]
        Mode _mode = Mode.Click;

        public event EventHandler<InputType> triggered;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_mode == Mode.Click)
                triggered?.Invoke(this, InputType.User);

        }

        public void OnCollisionEnter(Collision collision)
        {
            if (CollisionIsValid(collision.gameObject))
                triggered?.Invoke(this, InputType.Other);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (CollisionIsValid(other.gameObject))
            {
                Debug.Log(other);
                triggered?.Invoke(this, InputType.Other);
            }
        }

        public bool CollisionIsValid(GameObject go)
        {
            return _mode == Mode.Collide && IsBall(go) && LayerIsValid(go);
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