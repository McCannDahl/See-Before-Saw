using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepUprightWhenProjectOpen : MonoBehaviour
{
    public GameObject project;
    bool projectIsOpen = false;

    void Awake()
    {
        EventPublisher.i.HouseGrabbed += Show;
        EventPublisher.i.CloseProjectButtonPressed += Hide;
    }
    void Update()
    {
        if (projectIsOpen || project == null)
        {
            Quaternion q = transform.rotation;
            q.eulerAngles = new Vector3(0, q.eulerAngles.y, 0);
            transform.rotation = q;
        }
    }
    void Show(GameObject p)
    {
        if (p == project)
        {
            projectIsOpen = true;
        }
    }
    void Hide(GameObject p)
    {
        if (p == project)
        {
            projectIsOpen = false;
        }
    }
}
