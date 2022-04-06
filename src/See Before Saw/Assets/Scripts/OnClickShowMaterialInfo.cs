#nullable enable
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class OnClickShowMaterialInfo : MonoBehaviour
{
    public string materialName = "SBS 87F-V2/DF";
    public GameObject materialPerfab;
    public GameObject materialInfoPerfab;
    private TextMeshPro? dimensionTMP;
    public int pricePerLength = 50;
    public bool materialInfoOpen = false;
    public float GetPrice()
    {
        var size = GetComponent<Renderer>().bounds.size;
        List<float> nums = new List<float> { size.x, size.y, size.z };
        nums.Sort();
        return nums.Last()* pricePerLength;
    }
    // Start is called before the first frame update
    public void OnClickShowMaterialInfoFunction()
    {
        if (!materialInfoOpen && isActiveAndEnabled)
        {
            var go = Instantiate(materialInfoPerfab, new Vector3(0, 0, 0), Quaternion.identity);
            materialInfoOpen = true;
            // set texts
            var titleAndSubtitle = go.GetComponentsInChildren<TextMeshPro>();
            titleAndSubtitle[0].text = materialName;
            dimensionTMP = titleAndSubtitle[1];
            updateDimentionString();
            // set solver
            var InBetweenScript = go.GetComponent<InBetween>();
            InBetweenScript.SecondTransformOverride = gameObject.transform;
            // set parent var
            var MaterialInfoDisplayHnadlerScript = go.GetComponent<MaterialInfoDisplayHnadler>();
            MaterialInfoDisplayHnadlerScript.materialParent = gameObject;
        }
    }
    public void updateDimentionString()
    {
        if (dimensionTMP != null)
        {
            var sizeString = getDimentionString();
            dimensionTMP.text = sizeString;
        }
    }
    public string getDimentionString()
    {
        var nums = getActualDimentions();
        List<string> numsStr = new List<string> { "0","0","0" };
        for (int i = 0; i < numsStr.Count; i++)
        {
            if (nums[i] < 1)
            {
                numsStr[i] = (nums[i] * 12.0f).ToString("F1") + "in";
            }
            else
            {
                numsStr[i] = nums[i].ToString("F1") + "ft";
            }
        }
        var sizeString = numsStr[0]+" x " + numsStr[1]+ " x " + numsStr[2];
        return sizeString;
    }
    public Vector3 getActualDimentions()
    {
        float frameScale = 1 / transform.parent.localScale.x * 0.9f * (3f/4.4f); // this last number is just a fudge factor to get it correct
        return getMiniDimentions() * 3.28084f * frameScale;
    }
    public Vector3 getMiniDimentions()
    {
        var size = GetComponent<Renderer>().bounds.size;
        size = transform.localScale;
        List<float> nums = new List<float> { size.x, size.y, size.z };
        nums.Sort();
        return new Vector3(nums[0], nums[1], nums[2]);
    }
}
