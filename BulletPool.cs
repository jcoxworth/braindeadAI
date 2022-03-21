using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool _access;
    public int numberOfBullets = 100;
    public GameObject bullet;
    public static List<GameObject> bullets = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        _access = this;
        GenerateBulletPool();
    }
    private void GenerateBulletPool()
    {
        for (int i = 0; i < numberOfBullets; i ++)
        {
            GameObject newBullet = Instantiate(bullet);
            newBullet.SetActive(false);
            bullets.Add(newBullet);
            newBullet.hideFlags = HideFlags.HideInHierarchy;
        }
    }
    public GameObject GetBullet()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            if (bullets[i].activeInHierarchy == false)
                return bullets[i];
            else continue;
        }
        return bullets[0];
    }
}
