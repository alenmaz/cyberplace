using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Bar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI counter;

    private void Start()
    {
        counter = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetMaxValue(float health)
    {
        slider.maxValue = health;
        slider.value = health;
        UpdateUI();
    }

    public void SetValue(float health)
    {
        slider.value = health;
        UpdateUI();
    }

    public virtual void UpdateUI()
    {
      if (counter != null) counter.text = $"{slider.value}/{slider.maxValue}";
    }
}
