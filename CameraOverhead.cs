using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOverhead : MonoBehaviour
{
    public enum FocusTeam { A, Z}
    public FocusTeam currentTeamFocus = FocusTeam.A;

    [Range(5f,25f)] public float cameraHeight = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SelectTeamFocus(UnityEngine.UI.Slider teamSlider)
    {
        if (teamSlider.value == 0f)
            currentTeamFocus = FocusTeam.A;
        else
            currentTeamFocus = FocusTeam.Z;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 newCamPos = Vector3.zero;
        Vector3 targetPos = Vector3.zero;
        switch(currentTeamFocus)
        {
            case FocusTeam.A:
                targetPos = FindCenterOfGroup._ACenter;
                newCamPos = FindCenterOfGroup._ACenter + (Vector3.up * cameraHeight) + (Vector3.forward * cameraHeight);
                break;
            case FocusTeam.Z:
                targetPos = FindCenterOfGroup._ZCenter;
                newCamPos = FindCenterOfGroup._ZCenter + (Vector3.up * cameraHeight) + (Vector3.forward * -cameraHeight);
                break;
        }
        Vector3 direction = targetPos - newCamPos;
        transform.position = Vector3.Lerp(transform.position, newCamPos, Time.deltaTime * 3f);
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

    }
}
