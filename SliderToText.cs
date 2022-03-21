using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderToText : MonoBehaviour
{
    public TMPro.TMP_Text _text;
    private void Start()
    {
        UnityEngine.UI.Slider s = GetComponent<UnityEngine.UI.Slider>();
        if (s)
        {
            ChangeTextToSliderValue(s);
        }
    }


    // Update is called once per frame
    public void ChangeTextToSliderValue(UnityEngine.UI.Slider slider)
    {
        _text.text = slider.value.ToString();
    }
}
