using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HideOrShowOnEvent : HideOrShowOnSomething
{
    public List<EventPublisherEvents> showEvent;
    public List<EventPublisherEvents> hideEvent;
    public bool hideOnStart = true;
    void Awake()
    {
        foreach (var a in showEvent)
        {
            EventPublisher.i.AddListener(a, Show);
        }
        foreach (var b in hideEvent)
        {
            EventPublisher.i.AddListener(b, Hide);
        }
    }
    void Start()
    {
        if (hideOnStart)
        {
            Hide();
        }
    }
}
