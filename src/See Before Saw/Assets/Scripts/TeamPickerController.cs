using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TeamPickerController : HideOrShowOnSomething
{
    public ProjectController pc;
    void Awake()
    {
        pc.OrganizeTeamButtonPressed += Show;
        pc.EndOrganizeTeamButtonPressed += Hide;
    }
    void Start()
    {
        Hide();
    }
}
