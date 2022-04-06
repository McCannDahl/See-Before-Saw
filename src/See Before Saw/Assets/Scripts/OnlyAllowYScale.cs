using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyAllowYScale : MonoBehaviour
{
    Vector3 origionalScale;
    // Start is called before the first frame update
    void Start()
    {
        origionalScale = sbsUtils.cloneVector(transform.localScale);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(origionalScale.x, transform.localScale.y, origionalScale.z);
    }
}
