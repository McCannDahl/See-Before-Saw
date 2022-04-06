using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseGravity : MonoBehaviour
{
    private void Start()
    {
        gameObject.GetComponent<Rigidbody>().useGravity = false;
    }
    public void EnableGravity()
    {
        gameObject.GetComponent<Rigidbody>().useGravity = true;
    }
}
