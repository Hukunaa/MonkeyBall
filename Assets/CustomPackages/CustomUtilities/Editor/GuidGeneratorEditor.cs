using CustomUtilities;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GuidGenerator))]
public class GuidGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GuidGenerator guidGenerator = (GuidGenerator)target;

        if (GUILayout.Button("Copy"))
        {
            guidGenerator.CopyToClipboard();
        }

        if (GUILayout.Button("Generate GUID"))
        {
            guidGenerator.GenerateGuid();
        }
    }
}
