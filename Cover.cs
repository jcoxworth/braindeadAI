using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    public Transform[] coverTransforms;
    // Start is called before the first frame update
    void OnEnable()
    {
        Units.CoverAreas.Add(gameObject);
    }

    //returns a child transform that is the farthest from an Enemy Transform that is input by the parameter
    public Transform GetCoverArea(Transform enemyTransform)
    {
        float farthestDistance = 0f;
        Transform farthestCover = transform;
        foreach (Transform t in coverTransforms)
        {
            float newDistance = Vector3.Distance(enemyTransform.position, t.position);
            if (newDistance > farthestDistance)
            {
                farthestDistance = newDistance;
                farthestCover = t;
            }
        }

        return farthestCover;
    }
}
