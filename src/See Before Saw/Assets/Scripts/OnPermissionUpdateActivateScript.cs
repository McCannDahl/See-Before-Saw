using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPermissionUpdateActivateScript : MonoBehaviour
{
    public List<PermissionLevel> showOnPermissions;
    public MonoBehaviour scriptToActivate;
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
            scriptToActivate.enabled = true;
        }
        else
        {
            scriptToActivate.enabled = false;
        }
    }
}
