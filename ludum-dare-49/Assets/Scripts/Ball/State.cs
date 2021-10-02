using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ball
{
    public class State : MonoBehaviour
    {
        Vector3 _startPosition;
        Quaternion _startRotation;
        CollisionDetectionMode _collisionDetectionMode;

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

        public void RegisterStartInfo()
        {
            _startPosition = transform.position;
            _startRotation = transform.rotation;
        }

        [ContextMenu("Reset to start")]
        [Button(ButtonSizes.Medium)]

        public void ResetToStart()
        {
            StopPhysics();
            transform.position = _startPosition;
            transform.rotation = _startRotation;
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            rigidbody.ResetCenterOfMass();
            rigidbody.ResetInertiaTensor();
            //StartPhysics();
        }

        public void StopPhysics()
        {
            _collisionDetectionMode = rigidbody.collisionDetectionMode;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rigidbody.isKinematic = true;
            rigidbody.Sleep();
        }

        public void StartPhysics()
        {
            rigidbody.isKinematic = false;
            rigidbody.collisionDetectionMode = _collisionDetectionMode;
            rigidbody.Sleep();
        }
    }
}