using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseMatrixScript 
{
    public static  Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();

        //mousePos.z = Camera.main.farClipPlane * .5f;  I leave this here just in case I want to come back some day for 3D algorythm

        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePos);
        worldPoint.z = 0;

        return worldPoint;
    }

    public static int[] GetMouseMatrixIndex()
    {
        int[] mouseMatrixIndex = new int[2];

        MatrixManager matrixManager = MatrixManager.instance; 

        //Truncate position and add (0.5, 0.5, 0) to match cell center
        Vector3 mouseWorldPosTruncated = new Vector3(Mathf.FloorToInt(GetMouseWorldPos().x), Mathf.FloorToInt(GetMouseWorldPos().y), 0f);

        mouseMatrixIndex = matrixManager.FromWorldToMatrixIndex(mouseWorldPosTruncated + new Vector3(0.5f, 0.5f, 0f));
        if(mouseMatrixIndex == null) Debug.LogWarning("MouseMatrixScript: Out of matrix");

        return mouseMatrixIndex;
    }
}
