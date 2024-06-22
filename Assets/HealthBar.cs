using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public Image healthBarImageRight;
    public Image healthBarImageLeft;
    public float fillSpeed;
    public Gradient healthBarGradient;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void UpdateHealth(float amount)
    {
        currentHealth += amount;
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        float targetFillAmount = currentHealth / maxHealth;
        //healthBarImageRight.fillAmount = targetFillAmount;
        //healthBarImageLeft.fillAmount = targetFillAmount;
        healthBarImageRight.DOFillAmount(targetFillAmount, fillSpeed);
        healthBarImageLeft.DOFillAmount(targetFillAmount, fillSpeed);
        healthBarImageRight.color = healthBarGradient.Evaluate(targetFillAmount);
        healthBarImageLeft.color = healthBarGradient.Evaluate(targetFillAmount);

    }
}
