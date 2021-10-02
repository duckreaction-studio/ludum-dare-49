using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ball
{
    public class State : MonoBehaviour
    {
        private Vector3 _startPosition;
        private Quaternion _startRotation;

        Rigidbody _rigidbody;
        public new Rigidbody rigidbody
        {
            get
            {
                if (_rigidbody == null)
                {
                    _rigidbody = GetComponent<Rigidbody>();
                }
                return _rigidbody;
            }
        }

        void Awake()
        {
            _startPosition = transform.position;
            _startRotation = transform.rotation;
        }

        [ContextMenu("Reset to start")]
        [Button(ButtonSizes.Medium)]

        public void ResetToStart()
        {
            rigidbody.isKinematic = true;
            transform.position = _startPosition;
            transform.rotation = _startRotation;
            rigidbody.isKinematic = false;
        }
    }
}