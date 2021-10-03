using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Ball
{
    public class Impulse : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        float _force = 1;
        [SerializeField]
        Vector3 _direction = Vector3.right;

        [Inject(Optional = true)]
        LevelState _levelState;

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

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
                UserThrowBall();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            UserThrowBall();
        }

        private void UserThrowBall()
        {
            _levelState?.PlayerThrowBall();
            DoImpulse();
        }

        public void DoImpulse()
        {
            rigidbody.AddForce(_direction * _force, ForceMode.Impulse);
        }
    }
}