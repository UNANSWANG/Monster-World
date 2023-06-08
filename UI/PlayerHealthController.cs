using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    public Text level;
    public Image healthBarSlider;
    public Image expBarSlider;
     
    private void Update()
    {
        level.text = GameManager.Instance.playerStats.character_SO.currentLevel.ToString();
        UpdateHealth();
        UpdateExp();
    }

    void UpdateHealth()
    {
        float sliderPercent = (float)GameManager.Instance.playerStats.CurrentHealth / GameManager.Instance.playerStats.MaxHealth;
        healthBarSlider.fillAmount = sliderPercent;
    }

    void UpdateExp()
    {
        float sliderPercent = (float)GameManager.Instance.playerStats.character_SO.currentExp / GameManager.Instance.playerStats.character_SO.baseExp;
        expBarSlider.fillAmount = sliderPercent;
    }
}
