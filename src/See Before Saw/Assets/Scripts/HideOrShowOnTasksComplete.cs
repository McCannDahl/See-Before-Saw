using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HideOrShowOnTasksComplete : HideOrShowOnSomething
{
    public List<ProjectTasks> showTasksComplete;
    public List<ProjectTasks> showTasksInComplete;
    public List<ProjectTasks> hideTasksComplete;
    public List<ProjectTasks> hideTasksInComplete;
    public GameObject project;
    public bool hideOnStart = false;
    ProjectController pc;
    void Awake()
    {
        pc = project.GetComponent<ProjectController>();
        foreach (var a in showTasksComplete)
        {
            pc.TasksUpdated += (_) => Process(Show, a, true);
        }
        foreach (var a in hideTasksComplete)
        {
            pc.TasksUpdated += (_) => Process(Hide, a, true);
        }
        foreach (var a in showTasksInComplete)
        {
            pc.TasksUpdated += (_) => Process(Show, a, false);
        }
        foreach (var a in hideTasksInComplete)
        {
            pc.TasksUpdated += (_) => Process(Hide, a, false);
        }
    }

    private void Process(Action a, ProjectTasks showOrHideTask, bool isComplete)
    {
        try
        {
            var task = pc.tasks.First(x => x.type == showOrHideTask && x.isComplete == isComplete);
            if (task != null)
            {
                a.Invoke();
            }
        } catch(Exception err)
        {

        }
    }

    void Start()
    {
        if (hideOnStart)
        {
            Hide();
        }
    }
}
