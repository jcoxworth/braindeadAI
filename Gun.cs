using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private CharacterAiming characterAiming;
    public Transform bore;
    public bool debug = false;
    private GameObject lastBullet;
    private float currentCooldown = 0f;
    public float coolDown = 0.2f;

    public int magazineMin = 3;
    public int magazineMax = 10;
    public int currentMagazine = 0;

    [Range(0f, 3f)]
    public float kickAmount = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        characterAiming = GetComponentInParent<CharacterAiming>();
        currentMagazine = Random.Range(magazineMin, magazineMax);
    }

    // Update is called once per frame
    void Update()
    {
        if (debug)
        {
            if (Input.GetButton("Fire1"))
            {
                Fire();
            }
        }
    }
    public void Fire()
    {
        if (characterAiming.currentAimWeight < 0.9f)
            return;

        if (currentCooldown > coolDown)
        {
            ActivatePositionBullet();
            KickGun();
            currentCooldown = 0;
            /*
            if (currentMagazine > 0)
            {
                ActivatePositionBullet();
                currentMagazine--;
                currentCooldown = 0;
            }
            else
            {
                currentMagazine = Random.Range(magazineMin, magazineMax);
            }*/
        }
        else
        {
            currentCooldown += Time.deltaTime;
        }
    }
    private void ActivatePositionBullet()
    {
        lastBullet = BulletPool._access.GetBullet();
        lastBullet.transform.position = bore.position;
        lastBullet.transform.rotation = bore.rotation;
        lastBullet.gameObject.SetActive(true);
    }
    private void KickGun()
    {
        characterAiming.GunKick();
    }
}

