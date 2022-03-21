using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDrawLine : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;
    public float Length = 10f;
    private Vector3 lineDirection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lineDirection = endPos.position - startPos.position;
        Debug.DrawLine(startPos.position, startPos.position + (lineDirection * Length), Color.white);
    }
}
