using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetOritionalPosition : MonoBehaviour
{
    private List<Vector3> origionalPositions = new List<Vector3>();
    private List<Quaternion> oritionalRotations = new List<Quaternion>();
    Coroutine ResetPositionsCoroutine;
    void Awake() // maybe this should go in start?
    {
        foreach (Transform child in transform)
        {
            origionalPositions.Add(sbsUtils.cloneVector(child.localPosition));
            oritionalRotations.Add(sbsUtils.cloneQuaternion(child.localRotation));
        }
    }
    public void ResetPositions(Action callback = null)
    {
        if (ResetPositionsCoroutine != null)
        {
            StopCoroutine(ResetPositionsCoroutine);
        }
        ResetPositionsCoroutine = StartCoroutine(ResetPositionsCoroutineFunction(callback));
    }

    private IEnumerator ResetPositionsCoroutineFunction(Action callback = null)
    {
        List<Vector3> startPositions = new List<Vector3>();
        List<Quaternion> startRotations = new List<Quaternion>();
        foreach (Transform beam in transform)
        {
            startPositions.Add(beam.localPosition);
            startRotations.Add(beam.localRotation);
        }

        //lerp
        float duration = 1f;
        float time = 0f;
        while (time < duration)
        {
            float tt = time / duration;
            tt = tt * tt * (3f - 2f * tt);
            for (var i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).localPosition = Vector3.Lerp(startPositions[i], origionalPositions[i], tt);
                transform.GetChild(i).localRotation = Quaternion.Lerp(startRotations[i], oritionalRotations[i], tt);
            }
            time += Time.deltaTime;
            yield return null; //Don't freeze Unity
        }

        int children = transform.childCount;
        for (int i = 0; i < children; ++i)
        {
            try
            {
                transform.GetChild(i).gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                transform.GetChild(i).gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                transform.GetChild(i).gameObject.GetComponent<Rigidbody>().useGravity = false;
            }
            catch (Exception err)
            {
                // its ok if the go doesnt have a ridget body
            }
        }
        if (callback != null)
        {
            callback();
        }
    }
}
