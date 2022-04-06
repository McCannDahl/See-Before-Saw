using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOrShowWhenStacking : HideOrShowOnSomething
{
    public ProjectController pc;
    private void Awake()
    {
        pc.EndStackMaterials += Show;
        pc.StackMaterialsButtonPressed += Hide;
    }
}
