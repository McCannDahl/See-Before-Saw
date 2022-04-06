using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepSameRelativeRotation : MonoBehaviour
{
    internal GameObject tableModel;
    public Vector3 offset = new Vector3(0, 180, 0);
    public bool isActive = true;
    public void LateUpdate()
    {
        if (isActive)
        {
            transform.rotation = tableModel.transform.rotation;
            transform.Rotate(offset);
        }
    }
}
