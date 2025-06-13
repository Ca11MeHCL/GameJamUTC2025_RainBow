using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    [SerializeField] private GameObject row1, row2, row3, row4;

    public List<float> rowPositions = new List<float>();

    
    private void Start()
    {
        if(row1== null || row2 == null || row3 == null || row4 == null)
        {
            Debug.LogError("One or more rows are not assigned in the BoardController.");     
        }
       
        rowPositions.Add(row1.transform.position.y);
        rowPositions.Add(row2.transform.position.y);
        rowPositions.Add(row3.transform.position.y);
        rowPositions.Add(row4.transform.position.y);
        
    }
}