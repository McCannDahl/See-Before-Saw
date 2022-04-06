using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandOrNearMenuController : MonoBehaviour
{
    public bool isAttachedToHand = true;
    SolverHandler sh;
    GameObject pinButton;
    GameObject handButton;
    GameObject nearMenuChildrenGO;
    void Awake()
    {
        sh = gameObject.GetComponent<SolverHandler>();
        nearMenuChildrenGO = gameObject.transform.Find("NearMenuChildren").gameObject;
        pinButton = nearMenuChildrenGO.transform.Find("ButtonPin").gameObject;
        handButton = nearMenuChildrenGO.transform.Find("ButtonHand").gameObject;
    }
    void Start()
    {
        if (!isAttachedToHand)
        {
            nearMenuChildrenGO.SetActive(true);
        }
        SetProps();
    }

    public void Grab()
    {
        isAttachedToHand = false;
        SetProps();
    }
    public void GoBackToHand()
    {
        isAttachedToHand = true;
        SetProps();
    }

    public void Toggle()
    {
        isAttachedToHand = !isAttachedToHand;
        SetProps();
    }

    void SetProps()
    {
        sh.TrackedTargetType = isAttachedToHand ? Microsoft.MixedReality.Toolkit.Utilities.TrackedObjectType.HandJoint : Microsoft.MixedReality.Toolkit.Utilities.TrackedObjectType.Head;
        pinButton.SetActive(!isAttachedToHand);
        handButton.SetActive(!isAttachedToHand);
    }
}
