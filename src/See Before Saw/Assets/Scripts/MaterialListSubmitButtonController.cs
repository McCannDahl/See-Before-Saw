using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaterialListSubmitButtonController : MonoBehaviour
{
    public GameObject project;
    ProjectController p;
    GameObject button;
    List<ProjectTasks> showOnProjectTasks = new List<ProjectTasks>()
    {
        ProjectTasks.isMaterialListOrdered_Builder,
        ProjectTasks.isMaterialListFulfilled_Dealer,
        ProjectTasks.optimizeMaterials,
    };
    ProjectTasks currentTask;
    public GameObject youDontHaveEnoughInventoryPrefab;
    public GameObject inventoryCollection;
    InventoryController ic;
    private void Awake()
    {
        p = project.GetComponent<ProjectController>();
        ic = inventoryCollection.GetComponent<InventoryController>();
        button = transform.GetChild(0).gameObject;
        EventPublisher.i.ChangePermissions += (_) => UpdateButtonText();
        p.TasksUpdated += (_) => UpdateButtonText();
    }
    private void UpdateButtonText()
    {
        List<ProjectTask> tasks = p.getIncompleteTasksWithCompleteDependancies().Where(x => showOnProjectTasks.Contains(x.type)).ToList();
        if (tasks.Count > 0)
        {
            ProjectTask firstTask = tasks.First();
            ButtonConfigHelper script = button.GetComponent<ButtonConfigHelper>();
            script.MainLabelText = firstTask.name;
            currentTask = firstTask.type;
            button.SetActive(true);
        } else
        {
            button.SetActive(false);
        }
    }
    public void MaterialListButtonPressed()
    {
        switch (currentTask)
        {
            case ProjectTasks.isMaterialListFulfilled_Dealer:
                var missing = ic.subtractInventory(p.buildingFrame);
                if (missing.Count == 0)
                {
                    p.CompleteTask(ProjectTasks.isMaterialListOrdered_Dealer);
                    p.CompleteTasks(PermissionLevel.MILLS);
                    p.CompleteTask(ProjectTasks.isMaterialListFulfilled_Dealer);
                } else
                {
                    var missingTop3 = missing.OrderByDescending(i => i.qty).Take(3);
                    string missingString = string.Join("\n", missingTop3.Select(x => x.qty.ToString() + " " + x.dim + " " + x.name)) ;
                    Dialog.Open(youDontHaveEnoughInventoryPrefab, DialogButtonType.OK, "Not enough materials", "You don't have enough inventory to fultill this material list. Order more materials.\n" + missingString, true);
                }
                break;
            case ProjectTasks.optimizeMaterials:
                EventPublisher.i.OpenProjectForOptimization(project);
                break;
            default:
                p.CompleteTask(currentTask);
                break;
        }
    }
}
