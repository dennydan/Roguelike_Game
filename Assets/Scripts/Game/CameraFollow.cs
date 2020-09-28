using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform PlayerTransform;

    private Vector3 cameraOffset;

    [Range(0.01f, 1f)]
    public float SmoothFactor = .5f;

    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = transform.position - PlayerTransform.position;
    }

    private void LateUpdate()
    {
        Vector3 newPos = PlayerTransform.position + cameraOffset + new Vector3(0, 4f, 0);

        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);
    }

}
