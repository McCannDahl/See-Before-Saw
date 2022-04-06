using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedModelChildController : MonoBehaviour
{
    Vector3 origionalPosition;
    Quaternion origionalRotation;
    public GameObject project;
    void Awake()
    {
        EventPublisher.i.CloseProjectButtonPressed += CloseProject;
        origionalPosition = sbsUtils.cloneVector(project.transform.localPosition);
        origionalRotation = sbsUtils.cloneQuaternion(project.transform.localRotation);
    }
    void CloseProject(GameObject houseModel)
    {
        if (houseModel == project)
        {
            houseModel.transform.parent = transform;
            houseModel.transform.localPosition = sbsUtils.cloneVector(origionalPosition);
            houseModel.transform.localRotation = sbsUtils.cloneQuaternion(origionalRotation);
        }
    }
}
