using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSpawn : MonoBehaviour
{

    public GameObject spawnedObject;
    public string name = "thing";
    public int amount = 1;
    [Range(1f, 30f)]
    public float spacing = 5f;
    private List<GameObject> spawnedItems = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void MakeArmy()
    {
        for (int i = 0; i < amount; i++)
        {
            float position = i * spacing;
            if (i % 2 == 0)
                position *= -1;

            GameObject g = Instantiate(spawnedObject, transform.position + (transform.right * position), Quaternion.LookRotation(transform.forward, Vector3.up));
            g.name = name + " " + Random.Range(0, 9).ToString() + Random.Range(0, 9).ToString() + Random.Range(0, 9).ToString() + Random.Range(0, 9).ToString();
            spawnedItems.Add(g);
        }
    }
    public void SetArmyNumber(UnityEngine.UI.Slider slider)
    {
        amount = Mathf.RoundToInt(slider.value);
    }
    public void DeleteArmy()
    {
        foreach(GameObject g in spawnedItems)
        {
            Destroy(g);
        }
        spawnedItems.Clear();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
