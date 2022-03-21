using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollowTransform : MonoBehaviour
{
    public Transform targetTransform;
    public float followSpeed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 refVeloc = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, targetTransform.position, ref refVeloc, followSpeed);
    }
}
