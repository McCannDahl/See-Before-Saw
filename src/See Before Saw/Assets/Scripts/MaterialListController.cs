using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MaterialListRowData
{
    public int qty;
    public string dim;
    public string name;
    public Vector3 actualDim;
    public Vector3 miniDim;
    public MaterialListRowData(int q, string d, string n, Vector3 a, Vector3 m)
    {
        qty = q;
        dim = d;
        name = n;
        actualDim = a;
        miniDim = m;
    }
}

public class MaterialListController : MonoBehaviour
{
    public GameObject woodFrame;
    List<MaterialListRowData> rowDatas = new List<MaterialListRowData>();
    public GameObject materialListRowPrefab;
    int numSkipGameObjects = 4;
    int numSkipGameObjectsEnd = 2;
    int maxPerPage = 10;
    int pageIndex = 0;
    public GameObject previousPageButton;
    public GameObject nextPageButton;
    void Awake()
    {
        EventPublisher.i.BuildingFrameUpdated += UpdateMaterialList;
    }
    void Start()
    {
        SetRowDatas();
    }

    public void UpdateMaterialList(GameObject beam, bool willBeDestroyed)
    {
        if (beam.transform.parent.gameObject == woodFrame)
        {
            if (willBeDestroyed)
            {
                MaterialListRowData rowToDelete = sbsUtils.BeamToMaterialListRowData(beam);
                foreach (MaterialListRowData rowData in rowDatas)
                {
                    if (rowData.name == rowToDelete.name && rowData.dim == rowToDelete.dim)
                    {
                        rowData.qty -= 1;
                    }
                }
            } else
            {
                MaterialListRowData newData = sbsUtils.BeamToMaterialListRowData(beam.gameObject);
                foreach (MaterialListRowData rowData in rowDatas)
                {
                    if (rowData != null && rowData.name != null && rowData.dim != null && newData != null && newData.dim != null && newData.name != null)
                    {
                        if (rowData.name == newData.name && rowData.dim == newData.dim)
                        {
                            rowData.qty += 1;
                            newData = null;
                        }
                    }
                }
                if (newData != null)
                {
                    rowDatas.Add(newData);
                }
            }
            updateUI();
        }
        else
        {
            Debug.Log("ya");
        }
    }

    public void SetRowDatas()
    {
        rowDatas = new List<MaterialListRowData>();
        foreach (Transform beam in woodFrame.transform)
        {
            if (beam != null && beam.gameObject != null)
            {
                UpdateMaterialList(beam.gameObject, false);
            }
        }
        Debug.Log(rowDatas.Count);
    }

    public void NextPage()
    {
        pageIndex++;
        updateUI();
    }
    public void PreviousPage()
    {
        pageIndex--;
        updateUI();
    }

    public void updateUI()
    {
        // remove total row
        if (transform.childCount > numSkipGameObjectsEnd + numSkipGameObjects)
        {
            DeleteRow(transform.childCount - 2);
        }
        // remove all material list rows (keep end rows)
        for (int i = transform.childCount - (numSkipGameObjectsEnd+1); i > numSkipGameObjects-1; i--)
        {
            DeleteRow(i);
        }
        // add row datas
        rowDatas = rowDatas.OrderBy(x => x.qty).ToList();
        for (int i = pageIndex*maxPerPage; i < Math.Min(rowDatas.Count, (pageIndex+1) * maxPerPage); i++)
        {
            InstantiateRow(rowDatas[i]);
        }
        // add total row
        InstantiateRow(new MaterialListRowData(rowDatas.Sum(item => item.qty), "", "Total", Vector3.zero, Vector3.zero), 1);
        // hide/show buttons
        previousPageButton.SetActive(pageIndex > 0);
        nextPageButton.SetActive((pageIndex+1) * maxPerPage < rowDatas.Count);
        // reorder the collection
        gameObject.GetComponent<GridObjectCollection>().UpdateCollection();
    }
    public void InstantiateRow(MaterialListRowData rowData, int additionalRowIndex = 0)
    {
        // add the material list prefabs as children (they have to be in the right order. Add them as 2nd to last elements)
        GameObject go = Instantiate(materialListRowPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        go.transform.parent = transform;
        go.transform.SetSiblingIndex(transform.childCount - (numSkipGameObjectsEnd+1) + additionalRowIndex);
        // update the texts of the prefab
        go.transform.GetChild(0).GetChild(0).GetComponent<TextMesh>().text = rowData.qty.ToString();
        go.transform.GetChild(0).GetChild(1).GetComponent<TextMesh>().text = rowData.dim;
        go.transform.GetChild(0).GetChild(2).GetComponent<TextMesh>().text = rowData.name;
    }

    public void DeleteRow(int i)
    {
        var rowToDestroy = transform.GetChild(i).gameObject;
        rowToDestroy.transform.parent = null;
        Destroy(rowToDestroy);
    }
}
