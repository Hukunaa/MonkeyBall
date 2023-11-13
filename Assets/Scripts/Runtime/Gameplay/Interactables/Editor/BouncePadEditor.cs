using System;
using CustomUtilities;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BouncePad))]
public class BouncePadEditor : Editor
{
    private void OnSceneGUI()
    {
        /*BouncePad obj = (BouncePad)target;
        Handles.color = Color.red;
        var rot = Handles.Disc(Quaternion.identity, obj.transform.position, Vector3.up, 15, false, 1);

        if (rot != Quaternion.identity)
        {
            var val = MathCalculation.ConvertAngleToDirection(rot.y);
            obj.BounceDirection = new Vector3(val.x, 0, val.y);
        }*/
    }
}