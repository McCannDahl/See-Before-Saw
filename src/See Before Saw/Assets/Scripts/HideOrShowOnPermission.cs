using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOrShowOnPermission : HideOrShowOnSomething
{
    public List<PermissionLevel> showOnPermissions;
    void Awake()
    {
        foreach (var a in showOnPermissions)
        {
            EventPublisher.i.ChangePermissions += HandlePermissionChange;
        }
    }
    void Start()
    {
        HandlePermissionChange(EventPublisher.i.permissionLevel);
    }
    void HandlePermissionChange(PermissionLevel n)
    {
        if (showOnPermissions.Contains(n))
        {
            Show();
        } else
        {
            Hide();
        }
    }
}
