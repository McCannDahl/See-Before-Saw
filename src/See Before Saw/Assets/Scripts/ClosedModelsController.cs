using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ClosedModelsController : MonoBehaviour
{
    public GameObject openModels;
    void Awake()
    {
        EventPublisher.i.HouseGrabbed += OpenProject;
    }
    void OpenProject(GameObject houseModel)
    {
        houseModel.transform.parent = openModels.transform;
        gameObject.SetActive(false);
    }
}
