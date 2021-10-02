using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BallImpulse : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    float _force = 1;
    [SerializeField]
    Vector3 _direction = Vector3.right;

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

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Ball impulse");
        rigidbody.AddForce(_direction * _force, ForceMode.Impulse);
    }
}
