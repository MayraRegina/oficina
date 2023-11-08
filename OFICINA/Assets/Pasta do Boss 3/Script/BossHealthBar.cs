using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public BossAtack boss; 

    private void Start()
    {
        healthSlider.maxValue = boss.LifeDemon;
    }

    private void Update()
    {
        healthSlider.value = boss.LifeDemon;
    }
}