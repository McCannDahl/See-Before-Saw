using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public enum ProjectTasks
{
    timelineSet = 0,
    budgetSet = 1,
    architectureFinished = 2,
    isMaterialListFulfilled_Dealer = 3,
    isMaterialListOrdered_Builder = 4,
    optimizeMaterials = 5,
    cutMaterials = 6,
    shipMaterials_Mills = 7,
    shipMaterials_Dealer = 8,
    organize_builders = 9,
    isMaterialListOrdered_Dealer = 10,
    stackMaterials = 11
}


[System.Serializable]
public class ProjectTask
{
    public bool isComplete = false;
    public List<ProjectTask> dependancies = new List<ProjectTask>();
    public List<PermissionLevel> permissionLevelsThatCanCompleteTask = new List<PermissionLevel>();
    public string name;
    public ProjectTasks type;

    public bool dependanciesComplete()
    {
        bool complete = true;
        foreach(var dep in dependancies)
        {
            if (!dep.isComplete || !dep.dependanciesComplete())
            {
                complete = false;
            }
        }
        return complete;
    }
}

public class ProjectController : MonoBehaviour
{
    public GameObject inventory;
    InventoryController ic;
    public GameObject buildingFrame;
    public GameObject fireworksPrefab;

    public List<ProjectTasks> tasksToCompleteOnStart = new List<ProjectTasks>();
    public bool useTasksToCompleteOnStart = false;

    public List<ProjectTask> tasks = new List<ProjectTask>();
    public ProjectTask timelineSet;
    public ProjectTask budgetSet;
    public ProjectTask architectureFinished;
    public ProjectTask isMaterialListOrdered_Builder;
    public ProjectTask isMaterialListFulfilled_Dealer;
    public ProjectTask optimizeMaterials;
    public ProjectTask cutMaterials;
    public ProjectTask stackMaterials;
    public ProjectTask shipMaterials_Mills;
    public ProjectTask shipMaterials_Dealer;
    public ProjectTask organize_builders;
    public ProjectTask isMaterialListOrdered_Dealer;


    public delegate void TasksUpdatedHandler(ProjectTasks p);
    public delegate void OrganizeTeamButtonPressedHandler();
    public delegate void EndOrganizeTeamButtonPressedHandler();
    public delegate void StackMaterialsButtonPressedHandler();
    public delegate void EndStackMaterialsButtonPressedHandler();
    public delegate void EndStackMaterialsHandler();

    internal void CompleteTasks(PermissionLevel p)
    {
        foreach (var i in tasks.Where(t => t.permissionLevelsThatCanCompleteTask.Contains(p)))
        {
            CompleteTask(i);
        }
    }

    public event TasksUpdatedHandler TasksUpdated;
    public event OrganizeTeamButtonPressedHandler OrganizeTeamButtonPressed;
    public event EndOrganizeTeamButtonPressedHandler EndOrganizeTeamButtonPressed;
    public event StackMaterialsButtonPressedHandler StackMaterialsButtonPressed;
    public event EndStackMaterialsButtonPressedHandler EndStackMaterialsButtonPressed;
    public event EndStackMaterialsHandler EndStackMaterials;
    public void CallTasksUpdated(ProjectTasks p) {
        TasksUpdated?.Invoke(p);
        if (getIncompleteTasks().Count == 0)
        {
            Instantiate(fireworksPrefab,transform.position,Quaternion.Euler(0,0,0));
        }
    }

    public void CallOrganizeTeamButtonPressed()
    {
        OrganizeTeamButtonPressed?.Invoke();
    }
    public void CallEndOrganizeTeamButtonPressed()
    {
        EndOrganizeTeamButtonPressed?.Invoke();
        CompleteTask(organize_builders);
    }
    public void CallStackMaterialsButtonPressedPressed()
    {
        StackMaterialsButtonPressed?.Invoke();
    }
    public void CallEndStackMaterialsButtonPressed()
    {
        EndStackMaterialsButtonPressed?.Invoke();
    }
    public void CallEndStackMaterials()
    {
        EndStackMaterials?.Invoke();
        CompleteTask(stackMaterials);
    }


    private void Awake()
    {
        ic = inventory.GetComponent<InventoryController>();
        timelineSet = new ProjectTask()
        {
            permissionLevelsThatCanCompleteTask = new List<PermissionLevel>() { PermissionLevel.BUILDER },
            name = "Set the project timeline",
            type = ProjectTasks.timelineSet,
        };
        budgetSet = new ProjectTask()
        {
            permissionLevelsThatCanCompleteTask = new List<PermissionLevel>() { PermissionLevel.BUILDER },
            name = "Set the project budget",
            type = ProjectTasks.budgetSet,
        };
        architectureFinished = new ProjectTask()
        {
            permissionLevelsThatCanCompleteTask = new List<PermissionLevel>() { PermissionLevel.ARCHITECT },
            name = "Complete the building design",
            type = ProjectTasks.architectureFinished,
        };
        isMaterialListOrdered_Builder = new ProjectTask()
        {
            permissionLevelsThatCanCompleteTask = new List<PermissionLevel>() { PermissionLevel.BUILDER },
            name = "Finalize material list",
            dependancies = new List<ProjectTask>() { timelineSet, budgetSet, architectureFinished },
            type = ProjectTasks.isMaterialListOrdered_Builder,
        };
        isMaterialListFulfilled_Dealer = new ProjectTask()
        {
            permissionLevelsThatCanCompleteTask = new List<PermissionLevel>() { PermissionLevel.DEALER_OR_DISTRIBUTOR },
            name = "Fulfill material list",
            dependancies = new List<ProjectTask>() { isMaterialListOrdered_Builder },
            type = ProjectTasks.isMaterialListFulfilled_Dealer,
        };
        isMaterialListOrdered_Dealer = new ProjectTask()
        {
            permissionLevelsThatCanCompleteTask = new List<PermissionLevel>() { PermissionLevel.DEALER_OR_DISTRIBUTOR },
            name = "Order materials",
            dependancies = new List<ProjectTask>() { isMaterialListOrdered_Builder },
            type = ProjectTasks.isMaterialListOrdered_Dealer,
        };
        optimizeMaterials = new ProjectTask()
        {
            permissionLevelsThatCanCompleteTask = new List<PermissionLevel>() { PermissionLevel.MILLS },
            name = "Optimize Materials",
            dependancies = new List<ProjectTask>() { isMaterialListOrdered_Dealer },
            type = ProjectTasks.optimizeMaterials,
        };
        cutMaterials = new ProjectTask()
        {
            permissionLevelsThatCanCompleteTask = new List<PermissionLevel>() { PermissionLevel.MILLS },
            name = "Cut Materials",
            dependancies = new List<ProjectTask>() { optimizeMaterials },
            type = ProjectTasks.cutMaterials,
        };
        stackMaterials = new ProjectTask()
        {
            permissionLevelsThatCanCompleteTask = new List<PermissionLevel>() { PermissionLevel.MILLS },
            name = "Stack Materials",
            dependancies = new List<ProjectTask>() { cutMaterials },
            type = ProjectTasks.stackMaterials,
        };
        shipMaterials_Mills = new ProjectTask()
        {
            permissionLevelsThatCanCompleteTask = new List<PermissionLevel>() { PermissionLevel.MILLS },
            name = "Ship Materials",
            dependancies = new List<ProjectTask>() { stackMaterials },
            type = ProjectTasks.shipMaterials_Mills,
        };
        shipMaterials_Dealer = new ProjectTask()
        {
            permissionLevelsThatCanCompleteTask = new List<PermissionLevel>() { PermissionLevel.DEALER_OR_DISTRIBUTOR },
            name = "Ship Materials",
            dependancies = new List<ProjectTask>() { shipMaterials_Mills },
            type = ProjectTasks.shipMaterials_Dealer,
        };
        organize_builders = new ProjectTask()
        {
            permissionLevelsThatCanCompleteTask = new List<PermissionLevel>() { PermissionLevel.BUILDER },
            name = "Organize Builders",
            dependancies = new List<ProjectTask>() { shipMaterials_Dealer },
            type = ProjectTasks.organize_builders,
        };

        tasks.Add(timelineSet);
        tasks.Add(budgetSet);
        tasks.Add(architectureFinished);
        tasks.Add(isMaterialListFulfilled_Dealer);
        tasks.Add(isMaterialListOrdered_Builder);
        tasks.Add(isMaterialListOrdered_Dealer);
        tasks.Add(optimizeMaterials);
        tasks.Add(cutMaterials);
        tasks.Add(shipMaterials_Mills);
        tasks.Add(shipMaterials_Dealer);
        tasks.Add(organize_builders);
        tasks.Add(stackMaterials);

        EventPublisher.i.FinalizeDesignButtonPressed += () => CompleteTask(ProjectTasks.architectureFinished);
    }
    private void Start()
    {
        if (useTasksToCompleteOnStart)
        {
            EventPublisher.i.projectCurrentlyBeingDesigned = gameObject;
            foreach (var t in tasksToCompleteOnStart)
            {
                CompleteTask(t);
            }
        }
    }
    void CompleteTask(ProjectTask t)
    {
        t.isComplete = true;
        CallTasksUpdated(t.type);
    }
    public void CompleteTask(ProjectTasks t)
    {
        switch (t)
        {
            case ProjectTasks.timelineSet:
                CompleteTask(timelineSet);
                break;
            case ProjectTasks.budgetSet:
                CompleteTask(budgetSet);
                break;
            case ProjectTasks.architectureFinished:
                if (EventPublisher.i.projectCurrentlyBeingDesigned == gameObject)
                {
                    CompleteTask(architectureFinished);
                }
                break;
            case ProjectTasks.isMaterialListFulfilled_Dealer:
                CompleteTask(isMaterialListFulfilled_Dealer);
                break;
            case ProjectTasks.isMaterialListOrdered_Builder:
                CompleteTask(isMaterialListOrdered_Builder);
                break;
            case ProjectTasks.isMaterialListOrdered_Dealer:
                CompleteTask(isMaterialListOrdered_Dealer);
                break;
            case ProjectTasks.optimizeMaterials:
                CompleteTask(optimizeMaterials);
                break;
            case ProjectTasks.cutMaterials:
                CompleteTask(cutMaterials);
                break;
            case ProjectTasks.shipMaterials_Mills:
                CompleteTask(shipMaterials_Mills);
                ic.AddMLToInventory(buildingFrame);
                break;
            case ProjectTasks.shipMaterials_Dealer:
                CompleteTask(shipMaterials_Dealer);
                break;
            case ProjectTasks.organize_builders:
                CompleteTask(organize_builders);
                break;
            case ProjectTasks.stackMaterials:
                CompleteTask(stackMaterials);
                break;
            default:
                break;
        }
    }

    public void CompleteTask_isMaterialListOrdered_Dealer()
    {
        isMaterialListFulfilled_Dealer.dependancies.Add(shipMaterials_Mills);
        CompleteTask(ProjectTasks.isMaterialListOrdered_Dealer);
    }


    public List<ProjectTask> getIncompleteTasks()
    {
        // get my tasks that this role can complete
        List<ProjectTask> mytasks = tasks.Where(x => x.permissionLevelsThatCanCompleteTask.Contains(EventPublisher.i.permissionLevel)).ToList();
        // get my incomplete tasks
        List<ProjectTask> mytasksIncomplete = mytasks.Where(x => x.isComplete == false).ToList();
        return mytasksIncomplete;
    }
    public List<ProjectTask> getIncompleteTasksWithCompleteDependancies()
    {
        // get my incomplete tasks that have dependancies complete
        List<ProjectTask> mytasksThatHaveCompleteDependancies = getIncompleteTasks().Where(x => x.dependanciesComplete()).ToList();
        return mytasksThatHaveCompleteDependancies;
}

}
