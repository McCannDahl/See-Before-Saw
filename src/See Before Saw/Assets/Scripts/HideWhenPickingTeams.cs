using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideWhenPickingTeams : HideOrShowOnSomething
{
    public ProjectController pc;
    void Awake()
    {
        pc.OrganizeTeamButtonPressed += Hide;
        pc.EndOrganizeTeamButtonPressed += Show ;
    }
}
