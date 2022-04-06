using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject InventoryCollectionChildPrefab;
    public List<MaterialListRowData> rowDatas = new List<MaterialListRowData>();

    private void Start()
    {
        BuildInventoryChildren();
    }
    public void AddMLToInventory(GameObject buildingFrame)
    {
        foreach (MaterialListRowData rowData in sbsUtils.BuildingToRowData(buildingFrame))
        {
              addRowDataToInventory(rowData);
        }
        BuildInventoryChildren();
    }

    private void addRowDataToInventory(MaterialListRowData rowData)
    {
        if (rowDatas.Any(x => x.name == rowData.name && x.dim == rowData.dim))
        {
            var f = rowDatas.First(x => x.name == rowData.name && x.dim == rowData.dim);
            f.qty += rowData.qty;
        }
        else
        {
            rowDatas.Add(rowData);
        }
    }

    private void BuildInventoryChildren()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        foreach (var rowData in rowDatas)
        {
            GameObject go = Instantiate(InventoryCollectionChildPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            go.transform.parent = transform;
            Transform beam = go.transform.GetChild(0);
            beam.localScale = rowData.actualDim.normalized * 0.8f; // change this to fit inside the box
            Transform materialListRow = go.transform.GetChild(1);
            // update the texts of the prefab
            materialListRow.GetChild(0).GetChild(0).GetComponent<TextMesh>().text = rowData.qty.ToString();
            materialListRow.GetChild(0).GetChild(1).GetComponent<TextMesh>().text = rowData.dim;
            materialListRow.GetChild(0).GetChild(2).GetComponent<TextMesh>().text = rowData.name;
            go.name = "InventoryCollectionChildPrefab " + rowData.qty.ToString();
        }
        gameObject.GetComponent<GridObjectCollection>().UpdateCollection();
    }

    internal List<MaterialListRowData> subtractInventory(GameObject buildingFrame)
    {
        List<MaterialListRowData> rs = sbsUtils.BuildingToRowData(buildingFrame);
        List<MaterialListRowData> missingAnyInventory = new List<MaterialListRowData>();
        foreach (MaterialListRowData r in rs)
        {
            if(!rowDatas.Any(x => x.dim == r.dim && x.name == r.name && x.qty >= r.qty))
            {
                if (!rowDatas.Any(x => x.dim == r.dim && x.name == r.name))
                {
                    missingAnyInventory.Add(new MaterialListRowData(r.qty, r.dim, r.name, r.actualDim, r.miniDim));
                } else
                {
                    MaterialListRowData t = rowDatas.First(x => x.dim == r.dim && x.name == r.name);
                    missingAnyInventory.Add(new MaterialListRowData(r.qty - t.qty,r.dim,r.name,r.actualDim, r.miniDim));
                }
            }
        }
        if (missingAnyInventory.Count > 0)
        {
            return missingAnyInventory;
        }
        // subtract inventory
        foreach (MaterialListRowData r in rs)
        {
            MaterialListRowData t = rowDatas.First(x => x.dim == r.dim && x.name == r.name && x.qty >= r.qty);
            t.qty -= r.qty;
        }
        BuildInventoryChildren();
        return missingAnyInventory;
    }
}
