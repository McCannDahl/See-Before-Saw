using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckStackingController : HideOrShowOnSomething
{
    public ProjectController pc;
    public OptimizeFrameController ofc;
    private void Awake()
    {
        pc.StackMaterialsButtonPressed += Show;
        pc.EndStackMaterialsButtonPressed += Hide;
    }
    private void Start()
    {
        base.Hide();
    }
    public override void Show()
    {
        base.Show();
        ofc.gameObject.transform.parent.gameObject.SetActive(true);
        ofc.CalculateTruckStacking();
    }
    public override void Hide()
    {
        ofc.ResetEverything(() => {
            base.Hide();
            pc.CallEndStackMaterials();
        });
    }
}
