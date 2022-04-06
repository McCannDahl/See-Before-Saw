using Microsoft.MixedReality.Toolkit.Experimental.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BudgetController : MonoBehaviour
{
    public GameObject budget;
    public GameObject cost;
    int budgetNumber = 0;
    float costNumber = 0;
    public GameObject frame;
    public GameObject project;
    ProjectController projectController;
    private void Awake()
    {
        EventPublisher.i.BuildingFrameUpdated += UpdateCost;
        projectController = project.GetComponent<ProjectController>();
    }
    private void Start()
    {
        costNumber = 0;
        foreach (Transform beam in frame.transform)
        {
            costNumber += beam.gameObject.GetComponent<OnClickShowMaterialInfo>().GetPrice();
        }
        UpdateCostString();
        budget.GetComponent<TextMeshProUGUI>().text = budgetNumber.ToString();
    }
    public void KeyboardUpdate(string inputText)
    {
        if (Int32.TryParse(inputText, out budgetNumber))
        {
            UpdateBudgetColor();
            projectController.CompleteTask(ProjectTasks.budgetSet);
        }
    }
    private void UpdateCost(GameObject woodBeam, bool willBeDestroyed = false)
    {
        float beamPrice = woodBeam.GetComponent<OnClickShowMaterialInfo>().GetPrice();
        if (willBeDestroyed)
        {
            beamPrice = beamPrice * -1;
        }
        costNumber += beamPrice;
        UpdateCostString();
    }
    private void UpdateCostString()
    {
        cost.GetComponent<TextMesh>().text = costNumber.ToString("c0");
        UpdateBudgetColor();
    }
    private void UpdateBudgetColor()
    {
        cost.GetComponent<TextMesh>().color = costNumber > budgetNumber ? Color.red : Color.black;
    }
}
