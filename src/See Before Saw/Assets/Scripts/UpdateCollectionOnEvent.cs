using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCollectionOnEvent : MonoBehaviour
{
    public List<EventPublisherEvents> myevent;
    void Start()
    {
        new WaitForSeconds(1);
        foreach (var m in myevent)
        {
            EventPublisher.i.AddListener(m, () => StartCoroutine(ExampleCoroutine()));
        }
    }

    private void OnEnable()
    {
        StartCoroutine(ExampleCoroutine());
    }
    IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(1);
        GridObjectCollection oc = GetComponent<GridObjectCollection>();
        oc.UpdateCollection();
    }
}
