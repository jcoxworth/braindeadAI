using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public float bulletTimeLimit = 5f;
    private float bulletTime = 0f;
    //public Material AMaterial;
    //public Material ZMaterial;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
        bulletTime += Time.deltaTime;
        if (bulletTime > bulletTimeLimit)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        bulletTime = 0f;
    }
    private void OnTriggerEnter(Collider other)
    {
        other.transform.BroadcastMessage("ChangeHealth", -25, SendMessageOptions.DontRequireReceiver);
        gameObject.SetActive(false);
    }
}
