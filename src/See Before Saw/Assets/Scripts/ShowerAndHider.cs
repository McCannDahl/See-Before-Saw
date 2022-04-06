using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowerAndHider
{
    public static void show(GameObject go)
    {
        hideOrShow(go, true);
    }
    public static void hide(GameObject go)
    {
        hideOrShow(go, false);
    }
    private static bool hideOrShow(GameObject go, bool val) // this is a small util to help with the performance of showing/hiding things
    {
        // TODO TODO TODO make this a coroutine to not freez unity
        if (go.transform.childCount > 20)
        {
            bool oneOfMyChildrenDoesntHaveARenderer = false;
            foreach (Transform child in go.transform)
            {
                oneOfMyChildrenDoesntHaveARenderer = oneOfMyChildrenDoesntHaveARenderer || hideOrShow(child.gameObject, val);
            }
            Renderer r = go.GetComponent<Renderer>();
            if (r)
            {
                r.enabled = val;
                return false;
            }
            else
            {
                if (oneOfMyChildrenDoesntHaveARenderer || go.transform.childCount == 0)
                {
                    go.SetActive(val);
                    return true;
                }
            }
            return oneOfMyChildrenDoesntHaveARenderer;
        }
        else
        {
            go.SetActive(val);
            return true;
        }
    }
}