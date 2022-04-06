using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverIfProjectOpen : MonoBehaviour
{
    public GameObject project;
    bool projectIsOpen = false;
    public float hoverRange = 0.0002f;
    float hoverSpeed = 5f;
    float turningSpeed = 50f;
    float time = 0;
    float origionalY = 0;
    public bool shouldRotate = true;

    void Awake()
    {
        EventPublisher.i.HouseGrabbed += Show;
        EventPublisher.i.CloseProjectButtonPressed += Hide;
        origionalY = transform.localPosition.y;
    }

    void OnEnable()
    {
        transform.localPosition = new Vector3(transform.localPosition.x,origionalY, transform.localPosition.z);
    }
    void Update()
    {
        if (projectIsOpen || project == null)
        {
            time += Time.deltaTime;
            transform.localPosition += transform.up * (Mathf.Cos(time * hoverSpeed) * hoverRange);
            if (shouldRotate)
            {
                transform.Rotate(Vector3.up * Time.deltaTime * turningSpeed);
            }
        }
    }
    void Show(GameObject p)
    {
        if (project != null && p == project)
        {
            projectIsOpen = true;
        }
    }
    void Hide(GameObject p)
    {
        if (project != null && p == project)
        {
            projectIsOpen = false;
        }
    }
}
