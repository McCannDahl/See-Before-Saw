using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOrShowOnSomething : MonoBehaviour
{
    virtual public void Show()
    {
        ShowerAndHider.show(gameObject);
    }
    virtual public void Hide()
    {
        ShowerAndHider.hide(gameObject);
    }
}
