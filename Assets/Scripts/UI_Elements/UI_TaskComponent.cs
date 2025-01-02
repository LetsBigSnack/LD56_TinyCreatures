using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Data;

public class UI_TaskComponent : MonoBehaviour
{
    [Header("Task Text References")]
    [SerializeField] private TMP_Text taskProgressText;
    [SerializeField] private TMP_Text taskNeededText;
    [SerializeField] private TMP_Text taskText;

    // This method will be used to assign task data
    public void SetupRequirement(AchievementRequirement requirement)
    {
        // Set the text fields based on the task data
        taskProgressText.text = requirement.currentAmount.ToNumberSuffix(false);
        taskNeededText.text = requirement.neededAmount.ToNumberSuffix(false);
        taskText.text = requirement.type.ToString();                    
    }
}