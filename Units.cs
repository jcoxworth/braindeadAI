using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Units : MonoBehaviour
{
    public TMP_Text AUnitList;
    public TMP_Text ZUnitList;

    //public TMP_Text AUnitList_Dead;
    //public TMP_Text ZUnitList_Dead;

    public static List<GameObject> A_Units = new List<GameObject>();
    public static List<GameObject> Z_Units = new List<GameObject>();

    public static List<GameObject> A_Units_Dead = new List<GameObject>();
    public static List<GameObject> Z_Units_Dead = new List<GameObject>();

    public static List<GameObject> CoverAreas = new List<GameObject>();
    private void Update()
    {
        UpdateText_AUnits();
        UpdateText_ZUnits();
    }
    private void UpdateText_AUnits()
    {
        AUnitList.text = "";
        foreach (GameObject a in A_Units)
        {
            if (a)
                AUnitList.text += a.name + "\n";
        }
    }
    private void UpdateText_ZUnits()
    {
        ZUnitList.text = "";
        foreach (GameObject z in Z_Units)
        {
            if (z)
                ZUnitList.text += z.name + "\n";
        }
    }

    
    public static void ClearAllUnits()
    {
        foreach (GameObject a in A_Units)
        {
            Destroy(a);

         //   A_Units.Remove(a);
        }

        foreach (GameObject z in Z_Units)
        {
            Destroy(z);

            //Z_Units.Remove(z);
        }
        foreach (GameObject a in A_Units_Dead)
        {
            Destroy(a);

         //   A_Units_Dead.Remove(a);
        }

        foreach (GameObject z in Z_Units_Dead)
        {
            Destroy(z);

         //   Z_Units_Dead.Remove(z);
        }

        A_Units.Clear();
        Z_Units.Clear();
        A_Units_Dead.Clear();
        Z_Units_Dead.Clear();
    }
    
}
