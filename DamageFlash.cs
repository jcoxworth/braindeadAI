using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    public Material flashMaterial;
    public Material[] originalMaterials;
    public SkinnedMeshRenderer[] renderers;
    private float damageFlashTime = 0.1f;
    private float currentTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
        renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        originalMaterials = new Material[renderers.Length];
        currentTime = 0f;
        GetOriginalMaterials();
    }
    void GetOriginalMaterials()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].material;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            Flash();

        if (currentTime < 0.2f)
            FlashAllMaterials();
        else
            ResetAllMaterials();
        

        if (currentTime < 1f)
            currentTime += Time.deltaTime;
            
    }
    public void ChangeHealth(int a)
    {
        Flash();
    }
    public void Flash()
    {
        currentTime = 0f;
    }
    void FlashAllMaterials()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = flashMaterial;
        }
    }
    void ResetAllMaterials()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = originalMaterials[i];
        }
    }
}
