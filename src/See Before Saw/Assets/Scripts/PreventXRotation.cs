using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventXRotation : MonoBehaviour
{
    float inital = 0;
    private void Start()
    {
        inital = transform.localEulerAngles.x;
    }
    void Update()
    {
        transform.localEulerAngles = new Vector3(inital, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
}
