using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattleInventoryItem : MonoBehaviour
{
    [SerializeField] private CreatureBattleSlot type;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider shieldSlider;
    [SerializeField] private Slider timeSlider;

    [SerializeField] private GameObject sliderHolder;
    [SerializeField] private GameObject textHolder;
    [SerializeField] private bool isEnemy;

    public CreatureBattleSlot Type
    {
        get { return type; }
    }

    private void OnEnable()
    {
        ToggleActiveState();
    }

    public void ToggleActiveState()
    {
        if (InventoryManager.Instance.IsCreatureSlotEmpty(type) == null && !isEnemy || isEnemy && BattleManager.Instance.EnemyCreature == null)
        {

            textHolder.SetActive(true);
            sliderHolder.SetActive(false);
            return;
        }
        sliderHolder.SetActive(true);
        textHolder.SetActive(false);
    }

    public void UpdateHealthSlider(float amount)
    {
        healthSlider.value = amount;
        if(healthSlider.value <= 0)
        {
            ToggleActiveState();
        }
    }

    public void UpdateShieldSlider(float amount)
    {
        shieldSlider.value = amount;
    }

    public void UpdateTimeSlider(float amount)
    {
        timeSlider.value = amount;
    }

}
