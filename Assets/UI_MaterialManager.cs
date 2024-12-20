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
    }

    private void OnDisable()
    {
        InventoryManager.OnChangesMaterialA -= UpdateMaterialText1;
        InventoryManager.OnChangesMaterialB -= UpdateMaterialText2;
        InventoryManager.OnChangesMaterialC -= UpdateMaterialText3;
        InventoryManager.OnChangesMaterialD -= UpdateMaterialText4;
    }

    private void SetCurrentValues()
    {
        materialText_1.text = InventoryManager.Instance.MaterialA.ToNumberSuffix(false);
        materialText_2.text = InventoryManager.Instance.MaterialB.ToNumberSuffix(false);
        materialText_3.text = InventoryManager.Instance.MaterialC.ToNumberSuffix(false);
        materialText_4.text = InventoryManager.Instance.MaterialD.ToNumberSuffix(false);
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
}
