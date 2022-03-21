using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAiming : MonoBehaviour
{
    Unit myUnit;
    public float turnSpeed = 15;
    public float currentAimWeight = 0f;
    public float aimDuration = 0.3f;
    public bool isAiming = false;
    public Rig aimLayer;
    public Rig[] otherRigs;
    public Transform EyeTransform;
    public Transform AimTransform;

    private Vector3 gunKick;
    private Vector3 targetGunKick;

    private float otherRigs_currentWeight = 0f;
    private float otherRigs_targetWeight = 0f;
    // Start is called before the first frame update
    void Start()
    {
        targetGunKick = Vector3.zero;
        gunKick = Vector3.zero;
        myUnit = GetComponent<Unit>();
    }

    // Update is called once per frame
    void Update()
    {
        Aim();
        OtherRigs();
    }
    public void TurnAllRigsOn()
    {
        otherRigs_targetWeight = 1f;
    }
    public void TurnAllRigsOff()
    {
        otherRigs_targetWeight = 0f;
    }
    void OtherRigs()
    {
        otherRigs_currentWeight = Mathf.Lerp(otherRigs_currentWeight, otherRigs_targetWeight, Time.deltaTime * 3f);
        foreach(Rig r in otherRigs)
        {
            r.weight = otherRigs_currentWeight;
        }
    }
    void Aim()
    {
        if (isAiming)
        {
            aimLayer.weight += Time.deltaTime / aimDuration;
            currentAimWeight = aimLayer.weight;
        }
        else
        {
            aimLayer.weight -= Time.deltaTime / aimDuration;
            currentAimWeight = aimLayer.weight;
        }
        targetGunKick = Vector3.Lerp(targetGunKick, Vector3.zero, myUnit.shootingSkill * Time.deltaTime);
        gunKick = Vector3.Lerp(gunKick, targetGunKick, myUnit.shootingSkill * Time.deltaTime);

        Vector3 refVeloc = Vector3.zero;

        AimTransform.position = Vector3.Lerp(AimTransform.position, EyeTransform.position + gunKick, myUnit.shootingSkill * Time.deltaTime);
    }

    public void GunKick()
    {
        float distance = Vector3.Distance(transform.position, EyeTransform.position) * 0.1f;
        targetGunKick = new Vector3(Random.Range(-distance, distance), Random.Range(0, distance), 0);
    }




}
