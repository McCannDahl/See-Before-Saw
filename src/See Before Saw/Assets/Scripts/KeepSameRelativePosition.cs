using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepSameRelativePosition : MonoBehaviour
{
    internal GameObject tableModel;
    public Vector3 offset = Vector3.up * 0.5f;
    public bool isActive = true;
    public void LateUpdate()
    {
        if (isActive)
        {
            transform.position = tableModel.transform.position + offset;
        }
    }
}
