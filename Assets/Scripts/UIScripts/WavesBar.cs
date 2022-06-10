using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesBar : Bar
{
    public override void UpdateUI()
    {
        if (counter != null) counter.text = $"Волна {slider.value + 1}";
    }
}
