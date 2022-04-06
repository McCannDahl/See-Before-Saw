using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class LengthAndIndex
{
    public float length;
    public float actualLength;
    public int index;
}
public class OptimizeFrameController : MonoBehaviour
{
    public GameObject project;
    float optimizationConstraint = 0;
    float optimizationConstraintMin = 0;
    float optimizationConstraintMax = 0;
    ResetOritionalPosition resetScript;
    private Coroutine MoveBeamsIntoOptimizationPatternCoroutine;
    private Coroutine cutWoodCoroutine;
    public GameObject totalWoodTextGameObject;
    public GameObject excessWoodTextGameObject;
    public GameObject stockWoodTextGameObject;
    public GameObject hoveringSaw;
    List<List<LengthAndIndex>> logsToBeChopped = new List<List<LengthAndIndex>>();

    private void Awake()
    {
        EventPublisher.i.OptimizationSliderUpdated += UpdateOptimizationContraint;
        EventPublisher.i.StartOptimization += StartOptimizationCalculation;
        resetScript = gameObject.GetComponent<ResetOritionalPosition>();
        EventPublisher.i.ExitOptimizationButtonPressed += () => ResetEverything();
        EventPublisher.i.CutWoodButtonPressed += CutWood;
    }
    private void Start()
    {
        hoveringSaw.SetActive(false);
    }

    public void ResetEverything(Action callback = null)
    {
        resetScript.ResetPositions(callback);
    }
    private List<float> GetCutDimentions()
    {
        List<MaterialListRowData> rowDatas = sbsUtils.BuildingToRowData(gameObject);
        List<float> cutDimentions = rowDatas.Select(x => x.miniDim.z).ToList(); // we cut and align the beams on the longest side
        return cutDimentions;
    }
    private void StartOptimizationCalculation()
    {
        List<float> cutDimentions = GetCutDimentions();
        optimizationConstraintMin = cutDimentions.Max();
        optimizationConstraintMax = optimizationConstraintMin * 5; // TODO see if this is too big/small
        UpdateOptimizationContraintN(0);
    }

    private void UpdateOptimizationContraint(SliderEventData data)
    {
        if (data.OldValue != data.NewValue)
        {
            UpdateOptimizationContraintN(data.NewValue);
        }
    }
    private void UpdateOptimizationContraintN(float NewValue)
    {
        optimizationConstraint = (optimizationConstraintMax - optimizationConstraintMin) * NewValue + optimizationConstraintMin;
        CalculateOptimization();
    }

    private void CalculateOptimization()
    {
        // get beams by longest to shortest
        List<LengthAndIndex> beamLengths = new List<LengthAndIndex>();
        for (var i = 0; i < transform.childCount; i++)
        {
            var rowData = sbsUtils.BeamToMaterialListRowData(transform.GetChild(i).gameObject);
            beamLengths.Add(new LengthAndIndex()
            {
                length = rowData.miniDim.z,
                actualLength = rowData.actualDim.z,
                index = i
            });
        }
        beamLengths = beamLengths.OrderByDescending(x => x.length).ToList();

        // put them into groups
        logsToBeChopped = new List<List<LengthAndIndex>>();
        foreach (var beamLength in beamLengths)
        {
            bool beamFitsIntoExistingLog = false;
            foreach (var logToBeChopped in logsToBeChopped)
            {
                if (!beamFitsIntoExistingLog)
                {
                    if (logToBeChopped.Sum(x => x.length) + beamLength.length < optimizationConstraint)
                    {
                        logToBeChopped.Add(beamLength);
                        beamFitsIntoExistingLog = true;
                    }
                }
            }
            if (!beamFitsIntoExistingLog)
            {
                logsToBeChopped.Add(new List<LengthAndIndex>() { beamLength });
            }
        }
        if (MoveBeamsIntoOptimizationPatternCoroutine != null)
        {
            StopCoroutine(MoveBeamsIntoOptimizationPatternCoroutine);
        }
        if (isActiveAndEnabled)
        {
            MoveBeamsIntoOptimizationPatternCoroutine = StartCoroutine(MoveBeamsIntoOptimizationPattern(3f, logsToBeChopped));
        }
    }
    private IEnumerator MoveBeamsIntoOptimizationPattern(float duration, List<List<LengthAndIndex>> logsToBeChopped)
    {
        // get start positions
        List<Vector3> startPositions = new List<Vector3>();
        List<Quaternion> startRotations = new List<Quaternion>();
        foreach (Transform beam in transform)
        {
            startPositions.Add(beam.localPosition);
            startRotations.Add(beam.localRotation);
        }

        // get end positions
        List<Vector3> endPositions = new List<Vector3>();
        List<Quaternion> endRotations = new List<Quaternion>();
        for (var i = 0; i < transform.childCount; i++)
        {
            Transform beam = transform.GetChild(i);
            LengthAndIndex beamLengthAndIndex = logsToBeChopped.SelectMany(x => x).First(y => y.index == i);
            int row = logsToBeChopped.IndexOf(logsToBeChopped.First(x => x.Select(y => y.index).Contains(i)));
            int column = logsToBeChopped[row].IndexOf(logsToBeChopped[row].First(x => x.index == i)) + 1;
            float y = (logsToBeChopped[row].Take(column).Sum(x => x.length) - beamLengthAndIndex.length / 2) * 1.1f - 2.5f; // fundge number
            float x = (row - logsToBeChopped.Count / 2) * 0.12f; // fundge number
            float z = 0;
            endPositions.Add(new Vector3(x, y, z));

            endRotations.Add(Quaternion.Euler(0, 0, 0));
        }

        // set total wood text
        var scaleFactor = (logsToBeChopped[0][0].actualLength / logsToBeChopped[0][0].length);
        var stockWoodLength = optimizationConstraint * scaleFactor;
        stockWoodTextGameObject.GetComponent<TextMesh>().text = "Stock Wood Length: " + stockWoodLength.ToString("F1") + "ft";
        var totalWood = logsToBeChopped.SelectMany(x => x).Sum(x => x.actualLength);
        totalWoodTextGameObject.GetComponent<TextMesh>().text = "Total Wood Cut: " + totalWood.ToString("F1") + "ft";
        var totalWastedWood = (logsToBeChopped.Count * optimizationConstraint) * scaleFactor - totalWood;
        excessWoodTextGameObject.GetComponent<TextMesh>().text = "Excess Wood: " + totalWastedWood.ToString("F1") + "ft";


        //lerp
        float time = 0f;
        while (time < duration)
        {
            float tt = time / duration;
            tt = tt * tt * (3f - 2f * tt);
            for (var i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).localPosition = Vector3.Lerp(startPositions[i], endPositions[i], tt);
                transform.GetChild(i).localRotation = Quaternion.Lerp(startRotations[i], endRotations[i], tt);
            }
            time += Time.deltaTime;
            yield return null; //Don't freeze Unity
        }
    }
    public void CalculateTruckStacking()
    {
        float truckStackingConstraint = GetCutDimentions().Max();
        // get beams by longest to shortest
        List<LengthAndIndex> beamLengths = new List<LengthAndIndex>();
        for (var i = 0; i < transform.childCount; i++)
        {
            var rowData = sbsUtils.BeamToMaterialListRowData(transform.GetChild(i).gameObject);
            beamLengths.Add(new LengthAndIndex()
            {
                length = rowData.miniDim.z,
                actualLength = rowData.actualDim.z,
                index = i
            });
        }
        beamLengths = beamLengths.OrderByDescending(x => x.length).ToList();

        // put them into groups
        logsToBeChopped = new List<List<LengthAndIndex>>();
        foreach (var beamLength in beamLengths)
        {
            bool beamFitsIntoExistingLog = false;
            foreach (var logToBeChopped in logsToBeChopped)
            {
                if (!beamFitsIntoExistingLog)
                {
                    if (logToBeChopped.Sum(x => x.length) + beamLength.length < truckStackingConstraint)
                    {
                        logToBeChopped.Add(beamLength);
                        beamFitsIntoExistingLog = true;
                    }
                }
            }
            if (!beamFitsIntoExistingLog)
            {
                logsToBeChopped.Add(new List<LengthAndIndex>() { beamLength });
            }
        }
        if (MoveBeamsIntoOptimizationPatternCoroutine != null)
        {
            StopCoroutine(MoveBeamsIntoOptimizationPatternCoroutine);
        }
        if (isActiveAndEnabled)
        {
            MoveBeamsIntoOptimizationPatternCoroutine = StartCoroutine(MoveBeamsIntoTruckStackingPattern(3f, logsToBeChopped));
        }
    }
    private IEnumerator MoveBeamsIntoTruckStackingPattern(float duration, List<List<LengthAndIndex>> logsToBeChopped)
    {
        // get start positions
        List<Vector3> startPositions = new List<Vector3>();
        List<Quaternion> startRotations = new List<Quaternion>();
        foreach (Transform beam in transform)
        {
            startPositions.Add(beam.localPosition);
            startRotations.Add(beam.localRotation);
        }

        // get end positions
        List<Vector3> endPositions = new List<Vector3>();
        List<Quaternion> endRotations = new List<Quaternion>();
        int numberPerWidthOfTruck = 10;
        for (var i = 0; i < transform.childCount; i++)
        {
            Transform beam = transform.GetChild(i);
            LengthAndIndex beamLengthAndIndex = logsToBeChopped.SelectMany(x => x).First(y => y.index == i);
            int row = logsToBeChopped.IndexOf(logsToBeChopped.First(x => x.Select(y => y.index).Contains(i)));
            int truckWidthIndex = row % numberPerWidthOfTruck;
            int truckHightIndex = row / numberPerWidthOfTruck;
            int column = logsToBeChopped[row].IndexOf(logsToBeChopped[row].First(x => x.index == i)) + 1;
            float x = (logsToBeChopped[row].Take(column).Sum(x => x.length) - beamLengthAndIndex.length / 2) * 1.1f - 2 - 0.5f; // fundge number
            float z = (truckWidthIndex - logsToBeChopped.Count / 2) * 0.12f + 6f - 0.9f; // fundge number
            float y = truckHightIndex * 0.03f - 2.3f;
            endPositions.Add(new Vector3(x, y, z));

            endRotations.Add(Quaternion.Euler(180, 0, 90));
        }


        //lerp
        float time = 0f;
        while (time < duration)
        {
            float tt = time / duration;
            tt = tt * tt * (3f - 2f * tt);
            for (var i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).localPosition = Vector3.Lerp(startPositions[i], endPositions[i], tt);
                transform.GetChild(i).localRotation = Quaternion.Lerp(startRotations[i], endRotations[i], tt);
            }
            time += Time.deltaTime;
            yield return null; //Don't freeze Unity
        }
    }
    private void CutWood()
    {
        if (cutWoodCoroutine == null)
        {
            StartCoroutine(CutWoodCoroutineFunction());
        }
    }
    private IEnumerator CutWoodCoroutineFunction()
    {
        // animate the saw cutting the wood
        hoveringSaw.SetActive(true);
        List<LengthAndIndex> indexesInOrder = logsToBeChopped.SelectMany(x => x.Select(y => y)).ToList();
        foreach (var i in indexesInOrder)
        {
            float duration = 0.01f;
            float time = 0f;
            Vector3 startPos = hoveringSaw.transform.position;
            while (time < duration)
            {
                float tt = time / duration;
                tt = tt * tt * (3f - 2f * tt);
                hoveringSaw.transform.position = Vector3.Lerp(startPos, new Vector3(transform.GetChild(i.index).position.x, startPos.y, startPos.z), tt);
                time += Time.deltaTime;
                yield return null; //Don't freeze Unity
            }
        }
        hoveringSaw.SetActive(false);

        // reset everything
        ResetEverything(CutWoodFinal);
    }
    private void CutWoodFinal()
    {
        ProjectController pc = project.GetComponent<ProjectController>();
        pc.CompleteTask(ProjectTasks.optimizeMaterials);
        pc.CompleteTask(ProjectTasks.cutMaterials);
        EventPublisher.i.CallCutWoodFinished();
    }
}