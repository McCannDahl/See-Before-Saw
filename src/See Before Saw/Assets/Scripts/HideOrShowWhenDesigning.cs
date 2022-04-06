using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOrShowWhenDesigning : HideOrShowOnSomething
{
    public GameObject project;
    void Awake()
    {
        EventPublisher.i.DesignButtonPressed += showIfProject;
        EventPublisher.i.ExitDesignButtonPressed += Hide;
        EventPublisher.i.FinalizeDesignButtonPressed += Hide;
        EventPublisher.i.StartOptimization += () => showIfProject(null);
        EventPublisher.i.CutWoodFinished += Hide;
        EventPublisher.i.ExitOptimizationButtonPressed += Hide;
        var pc = project.GetComponent<ProjectController>();
        pc.StackMaterialsButtonPressed += Show;
        pc.EndStackMaterials += Hide;
    }
    private void Start()
    {
        Hide();
    }
    void showIfProject(GameObject p)
    {
        if (p == null)
        {
            p = EventPublisher.i.projectCurrentlyBeingOptimized;
        }
        if (project == p)
        {
            Show();
        }else
        {
            Hide();
        }
    }
}
