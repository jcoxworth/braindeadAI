using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindCenterOfGroup : MonoBehaviour
{
    public Transform ACenter, ZCenter;

    public static Vector3 _ACenter, _ZCenter;
    public static Transform _AcenterTransform, _ZcenterTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ACenter.position = GroupCenter(Units.A_Units);
        ZCenter.position = GroupCenter(Units.Z_Units);

        _ACenter = ACenter.position;
        _ZCenter = ZCenter.position;

        _AcenterTransform = ACenter;
        _ZcenterTransform = ZCenter;
    }
    Vector3 GroupCenter(List<GameObject> group)
    {
        Vector3 center = new Vector3(0, 0, 0);
        int count = 0;
        foreach (GameObject u in group)
        {
            center += u.transform.position;
            count++;
        }
        Vector3 theCenter = Vector3.zero;
        if (count > 0)
            theCenter = center / count;

        return theCenter;
    }
}
