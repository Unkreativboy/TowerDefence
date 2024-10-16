using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;

    private Slider healthBarSlider;

    private void Awake()
    {
        healthBarSlider = GetComponent<Slider>();
    }

    private void Start()
    {
        
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        UpdateHealthBar();
        UpdateHealthBarVisible();
    }
    private void OnDestroy()
    {
        healthSystem.OnDamaged -= HealthSystem_OnDamaged;
    }


    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        UpdateHealthBar();
        UpdateHealthBarVisible();
    }

    private void UpdateHealthBar()
    {
        healthBarSlider.value = healthSystem.GetHealthAmountNormalized();
    }
    private void UpdateHealthBarVisible()
    {
        
        if (healthSystem.IsHealthFull())
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
        
    }

}
