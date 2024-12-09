using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_TaskComponent : MonoBehaviour
{
    [Header("Task Text References")]
    [SerializeField] private TMP_Text taskProgressText;
    [SerializeField] private TMP_Text taskNeededText;
    [SerializeField] private TMP_Text taskText;

    // This method will be used to assign task data
    public void SetupTask(Task task)
    {
        // Set the text fields based on the task data
        taskProgressText.text = task.collected.ToString();
        taskNeededText.text = task.totalRequired.ToString();
        taskText.text = task.taskText;                    
    }
}