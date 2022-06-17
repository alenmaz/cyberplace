using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WavesBar : Bar
{
    [SerializeField] private Slider subSlider;

    public override void SetMaxValue(float wave)
    {
        slider.maxValue = wave;
        slider.value = 0;
        if (subSlider != null)
        {
            subSlider.maxValue = wave;
            subSlider.value = 1;
        }
        UpdateUI();
    }

    public override void SetValue(float wave)
    {
        slider.value = wave;
        if (subSlider != null) subSlider.value = wave + 1 <= slider.maxValue ? wave + 1 : wave;
        UpdateUI();
    }

    public override void UpdateUI()
    {
        if (counter != null)
        { 
            if (slider.value + 1 <= slider.maxValue)
                counter.text = $"Волна {slider.value + 1}";
            else counter.text = $"Волна {slider.value}";
        }
    }
}
