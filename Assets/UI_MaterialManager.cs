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

    private Coroutine sliderA = null;
    private Coroutine sliderB = null;
    private Coroutine sliderC = null;
    private Coroutine sliderD = null;

    public Coroutine SliderA
    {
        get { return sliderA; }
        set { sliderA = value; }
    }

    public Coroutine SliderB
    {
        get { return sliderB; }
        set { sliderB = value; }
    }

    public Coroutine SliderC
    {
        get { return sliderC; }
        set { sliderC = value; }
    }

    public Coroutine SliderD
    {
        get { return sliderD; }
        set { sliderD = value; }
    }

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

    public IEnumerator SliderMatA()
    {
        float yieldSpeed = MaterialManager.Instance.MaterialStatsA.yieldSpeed;
        float yieldTime = MaterialManager.Instance.MaterialStatsA.yieldTime;

        sliderMaterialA.maxValue = yieldSpeed;
        sliderMaterialA.value = yieldTime;
        while (true)
        {
            if(yieldTime == yieldSpeed)
            {
                sliderMaterialA.value = 0;
                yieldTime = 0;
            } 
            else
            {
                sliderMaterialA.value += 0.1f;
                yieldTime += 0.1f;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator SliderMatB()
    {
        float yieldSpeed = MaterialManager.Instance.MaterialStatsB.yieldSpeed;
        float yieldTime = MaterialManager.Instance.MaterialStatsB.yieldTime;

        sliderMaterialB.maxValue = yieldSpeed;
        sliderMaterialB.value = yieldTime;
        while (true)
        {
            if (yieldTime == yieldSpeed)
            {
                sliderMaterialB.value = 0;
                yieldTime = 0;
            }
            else
            {
                sliderMaterialB.value += 0.1f;
                yieldTime += 0.1f;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator SliderMatC()
    {
        float yieldSpeed = MaterialManager.Instance.MaterialStatsC.yieldSpeed;
        float yieldTime = MaterialManager.Instance.MaterialStatsC.yieldTime;

        sliderMaterialC.maxValue = yieldSpeed;
        sliderMaterialC.value = yieldTime;
        while (true)
        {
            if (yieldTime == yieldSpeed)
            {
                sliderMaterialC.value = 0;
                yieldTime = 0;
            }
            else
            {
                sliderMaterialC.value += 0.1f;
                yieldTime += 0.1f;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator SliderMatD()
    {
        float yieldSpeed = MaterialManager.Instance.MaterialStatsD.yieldSpeed;
        float yieldTime = MaterialManager.Instance.MaterialStatsD.yieldTime;

        sliderMaterialD.maxValue = yieldSpeed;
        sliderMaterialD.value = yieldTime;
        while (true)
        {
            if (yieldTime == yieldSpeed)
            {
                sliderMaterialD.value = 0;
                yieldTime = 0;
            }
            else
            {
                sliderMaterialD.value += 0.1f;
                yieldTime += 0.1f;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

}
