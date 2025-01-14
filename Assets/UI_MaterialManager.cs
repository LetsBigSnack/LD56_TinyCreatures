using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Data;

public class UI_MaterialManager : MonoBehaviour
{
    public static UI_MaterialManager Instance;

    [SerializeField] private UI_MaterialSlotItem materialSlotA;
    [SerializeField] private UI_MaterialSlotItem materialSlotB;
    [SerializeField] private UI_MaterialSlotItem materialSlotC;
    [SerializeField] private UI_MaterialSlotItem materialSlotD;

    [SerializeField] private TextMeshProUGUI materialText_1;
    [SerializeField] private TextMeshProUGUI materialText_2;
    [SerializeField] private TextMeshProUGUI materialText_3;
    [SerializeField] private TextMeshProUGUI materialText_4;

    [SerializeField] private TextMeshProUGUI yieldText_1;
    [SerializeField] private TextMeshProUGUI yieldText_2;
    [SerializeField] private TextMeshProUGUI yieldText_3;
    [SerializeField] private TextMeshProUGUI yieldText_4;

    [SerializeField] private Slider sliderMaterialA;
    [SerializeField] private Slider sliderMaterialB;
    [SerializeField] private Slider sliderMaterialC;
    [SerializeField] private Slider sliderMaterialD;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SetCurrentValues();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        InventoryManager.OnChangesMaterialA += UpdateMaterialText1;
        InventoryManager.OnChangesMaterialB += UpdateMaterialText2;
        InventoryManager.OnChangesMaterialC += UpdateMaterialText3;
        InventoryManager.OnChangesMaterialD += UpdateMaterialText4;

        MaterialManager.OnChangesYieldAmountMat1 += UpdateMaterialYieldText1;
        MaterialManager.OnChangesYieldAmountMat2 += UpdateMaterialYieldText2;
        MaterialManager.OnChangesYieldAmountMat3 += UpdateMaterialYieldText3;
        MaterialManager.OnChangesYieldAmountMat4 += UpdateMaterialYieldText4;

        MaterialManager.OnChangesYieldTimeMat1 += UpdateMaterialSlider1;
        MaterialManager.OnChangesYieldTimeMat2 += UpdateMaterialSlider2;
        MaterialManager.OnChangesYieldTimeMat3 += UpdateMaterialSlider3;
        MaterialManager.OnChangesYieldTimeMat4 += UpdateMaterialSlider4;
    }

    private void OnDisable()
    {
        InventoryManager.OnChangesMaterialA -= UpdateMaterialText1;
        InventoryManager.OnChangesMaterialB -= UpdateMaterialText2;
        InventoryManager.OnChangesMaterialC -= UpdateMaterialText3;
        InventoryManager.OnChangesMaterialD -= UpdateMaterialText4;

        MaterialManager.OnChangesYieldAmountMat1 -= UpdateMaterialYieldText1;
        MaterialManager.OnChangesYieldAmountMat2 -= UpdateMaterialYieldText2;
        MaterialManager.OnChangesYieldAmountMat3 -= UpdateMaterialYieldText3;
        MaterialManager.OnChangesYieldAmountMat4 -= UpdateMaterialYieldText4;

        MaterialManager.OnChangesYieldTimeMat1 -= UpdateMaterialSlider1;
        MaterialManager.OnChangesYieldTimeMat2 -= UpdateMaterialSlider2;
        MaterialManager.OnChangesYieldTimeMat3 -= UpdateMaterialSlider3;
        MaterialManager.OnChangesYieldTimeMat4 -= UpdateMaterialSlider4;
    }

    private void SetCurrentValues()
    {
        materialText_1.text = InventoryManager.Instance.MaterialA.ToNumberSuffix(false);
        materialText_2.text = InventoryManager.Instance.MaterialB.ToNumberSuffix(false);
        materialText_3.text = InventoryManager.Instance.MaterialC.ToNumberSuffix(false);
        materialText_4.text = InventoryManager.Instance.MaterialD.ToNumberSuffix(false);
    }

    public void ResetMiningData()
    {

    }

    private void UpdateMaterialText1(BigDecimal amount) {

        materialText_1.text = amount.ToNumberSuffix(false);
    }


    private void UpdateMaterialText2(BigDecimal amount)
    {

        materialText_2.text = amount.ToNumberSuffix(false);
    }

    private void UpdateMaterialText3(BigDecimal amount)
    {

        materialText_3.text = amount.ToNumberSuffix(false);
    }

    private void UpdateMaterialText4(BigDecimal amount)
    {

        materialText_4.text = amount.ToNumberSuffix(false);
    }

    private void UpdateMaterialYieldText1(BigDecimal amount)
    {
        if(amount == 0)
        {
            yieldText_1.text = "NO DATA";
            sliderMaterialA.value = 0;
            return;
        }
        yieldText_1.text = amount.ToNumberSuffix(false) + "/" + MaterialManager.Instance.MaterialStatsA.yieldSpeed + "s";
    }

    private void UpdateMaterialYieldText2(BigDecimal amount)
    {
        if (amount == 0)
        {
            yieldText_2.text = "NO DATA";
            sliderMaterialB.value = 0;
            return;
        }
        yieldText_2.text = amount.ToNumberSuffix(false) + "/" + MaterialManager.Instance.MaterialStatsB.yieldSpeed + "s";
    }

    private void UpdateMaterialYieldText3(BigDecimal amount)
    {
        if (amount == 0)
        {
            yieldText_3.text = "NO DATA";
            sliderMaterialC.value = 0;
            return;
        }
        yieldText_3.text = amount.ToNumberSuffix(false) + "/" + MaterialManager.Instance.MaterialStatsC.yieldSpeed + "s";
    }

    private void UpdateMaterialYieldText4(BigDecimal amount)
    {
        if (amount == 0)
        {
            yieldText_4.text = "NO DATA";
            sliderMaterialD.value = 0;
            return;
        }
        yieldText_4.text = amount.ToNumberSuffix(false) + "/" + MaterialManager.Instance.MaterialStatsD.yieldSpeed + "s";
    }

    private void UpdateMaterialSlider1(float amount)
    {
        sliderMaterialA.maxValue = MaterialManager.Instance.MaterialStatsA.yieldSpeed;
        sliderMaterialA.value = amount;
    }

    private void UpdateMaterialSlider2(float amount)
    {
        sliderMaterialB.maxValue = MaterialManager.Instance.MaterialStatsB.yieldSpeed;
        sliderMaterialB.value = amount;
    }

    private void UpdateMaterialSlider3(float amount)
    {
        sliderMaterialC.maxValue = MaterialManager.Instance.MaterialStatsC.yieldSpeed;
        sliderMaterialC.value = amount;
    }

    private void UpdateMaterialSlider4(float amount)
    {
        sliderMaterialD.maxValue = MaterialManager.Instance.MaterialStatsD.yieldSpeed;
        sliderMaterialD.value = amount;
    }
}
