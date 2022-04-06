using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideIfTasksComplete :HideOrShowOnSomething
{
    public GameObject project;
    ProjectController p;
    void Awake()
    {
        EventPublisher.i.ChangePermissions += (_) => hideIfTasksComplete();
        p = project.GetComponent<ProjectController>();
        p.TasksUpdated += (_) => hideIfTasksComplete();
    }
    void hideIfTasksComplete()
    {
        if (p.getIncompleteTasks().Count == 0)
        {
            Hide();
        } else
        {
            Show();
        }
    }
}