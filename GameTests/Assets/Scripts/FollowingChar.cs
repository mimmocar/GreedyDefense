using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingChar : MonoBehaviour
{
    [SerializeField] Transform PlayerTransform;
    private Vector3 _cameraOffset;

    [Range(0.01f, 1.0f)]
    [SerializeField] float SmoothFactor = 0.5f;

    void Start()
    {
        _cameraOffset = transform.position - PlayerTransform.position;
    }

    void LateUpdate()
    {

        Vector3 newPos = PlayerTransform.position + _cameraOffset;

        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);
    }
}
