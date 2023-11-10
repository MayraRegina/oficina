using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss1HealfBar : MonoBehaviour
{
    public Slider healthSlider;
    public Enemy boss; 

    private void Start()
    {
        healthSlider.maxValue = boss.Life;
    }

    private void Update()
    {
        healthSlider.value = boss.Life;
    }
}
