using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseMatrixScript 
{
    static Vector3 mousePos;

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

    public static int[] GetMatrixIndex(Vector3 position)
    {
        int[] matrixIndex = new int[2];

        MatrixManager matrixManager = MatrixManager.instance; 

        //Truncate position and add (0.5, 0.5, 0) to match cell center
        Vector3 worldPosTruncated = new Vector3(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y), 0f);

        matrixIndex = matrixManager.FromWorldToMatrixIndex(worldPosTruncated + new Vector3(0.5f, 0.5f, 0f));
        if(matrixIndex == null) Debug.LogWarning("MouseMatrixScript: Out of matrix");

        return matrixIndex;
    }

    public static bool PointerOnSteppedCloud() => MatrixManager.instance.GetItemsLayoutMatrix()[GetMouseMatrixIndex()[0], GetMouseMatrixIndex()[1]] == 
                                                  PlayerBehavior.instance.GetItemUnderPj();

    public static void BlockPointer(){
        mousePos = Mouse.current.position.ReadValue();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public static void ReleasePointer(){
        Cursor.lockState = CursorLockMode.None;
        if (mousePos != null)
            Mouse.current.WarpCursorPosition(mousePos);
        Cursor.visible = true;
    }
    
}
