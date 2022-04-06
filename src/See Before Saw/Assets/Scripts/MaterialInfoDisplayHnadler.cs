#nullable enable
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialInfoDisplayHnadler : MonoBehaviour
{
    public GameObject? materialParent;
    public void OnCloseMaterialInfoDisplayHnadler()
    {
        var script = materialParent?.GetComponent<OnClickShowMaterialInfo>();
        if (script != null)
        {
            script.materialInfoOpen = false;
        }
        Destroy(gameObject);
    }
    public void OnDuplicateMaterialInfoDisplayHnadler()
    {
        if (materialParent != null)
        {
            var script = materialParent.GetComponent<OnClickShowMaterialInfo>();
            var go = Instantiate(script.materialPerfab, materialParent.transform.position + Camera.main.transform.forward * -0.3f, materialParent.transform.rotation);
            go.transform.parent = materialParent.transform.parent;
            go.transform.localScale = materialParent.transform.localScale;
            var script2 = go.GetComponent<OnClickShowMaterialInfo>();
            script2.materialInfoOpen = false;
            EventPublisher.i.CallBuildingFrameUpdated(materialParent);
        }
    }
    public void OnDiscardMaterialInfoDisplayHnadler()
    {
        if (materialParent != null)
        {
            EventPublisher.i.CallBuildingFrameUpdated(materialParent, true);
            Destroy(materialParent);
        }
        OnCloseMaterialInfoDisplayHnadler();
    }
}
