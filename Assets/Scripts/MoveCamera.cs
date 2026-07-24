using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private float offset;
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;

    public void SetOffset(float newOffset)
    {
        offset = newOffset;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = target.position + new Vector3(offset, 0f, -10f);  
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
