using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [Header("== 基础设置 ==")]
    public GameObject healthBarPrefab;
    public Transform barPoint;
    public bool awaysVisiable;
    public float visiableTime;

    private float currentTime;
    Image healthSlider;
    Text levelText;
    Transform UIBar;
    Transform cam;

    CharacterStats state;
    private void Awake()
    {
        state = GetComponent<CharacterStats>();
        state.UpdateHealthBarOnAttack += UpdateUIBar;
    }
    private void OnEnable()
    {
        cam = Camera.main.transform;
        foreach (Canvas item in FindObjectsOfType<Canvas>())
        {
            if (item.renderMode==RenderMode.WorldSpace&&item.gameObject.CompareTag("HealthUI"))
            {
                UIBar = Instantiate(healthBarPrefab,item.transform).transform;
                healthSlider = UIBar.GetChild(0).GetComponent<Image>();
                levelText = UIBar.GetChild(1).GetComponent<Text>();
                //设置是否长久可见
                UIBar.gameObject.SetActive(awaysVisiable);
            }
        }
    }
    private void UpdateUIBar(int currentHealth, int maxHealth)
    {
        if (currentHealth<=0)
        {
            state.UpdateHealthBarOnAttack -= UpdateUIBar;
            Destroy(UIBar.gameObject);
        }
        UIBar.gameObject.SetActive(true);
        currentTime = visiableTime;
        float sliderPercent = (float)(1.0 * currentHealth) / maxHealth;
        healthSlider.fillAmount = sliderPercent;
        levelText.text = "Level："+state.level;
    }

    private void LateUpdate()
    {
        if (UIBar!=null)
        {
            UIBar.position = barPoint.position;
            UIBar.forward = -cam.forward;
            if (currentTime<=0&&!awaysVisiable)
            {
                UIBar.gameObject.SetActive(false);
            }
            else if(!awaysVisiable)
            {
                currentTime -= Time.deltaTime;
            }
        }
    }
}
