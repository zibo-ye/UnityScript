using UnityEngine;
using System.Collections;
using UnityEditor;

//Origin Source: https://answers.unity.com/questions/1178760/how-could-i-count-my-poly-tri.html
//Add recursion and some options

public class MeshInfo : EditorWindow
{
    bool isRecursive = true;
    private bool includeDisabledObjects = false;
    
    private int vertexCountAll;
    private int submeshCountAll;
    private int triangleCountAll;

    [MenuItem("Tools/Mesh Info")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        MeshInfo window = (MeshInfo) EditorWindow.GetWindow(typeof(MeshInfo));
        window.titleContent.text = "Mesh Info";
    }

    void OnSelectionChange()
    {
        Repaint();
    }

    public void RecursiveCount(GameObject root, ref int vertexCount, ref int triangleCount, ref int submeshCount)
    {
        if (root && root.GetComponent<MeshFilter>())
        {
            vertexCount += root.GetComponent<MeshFilter>().sharedMesh.vertexCount;
            triangleCount += root.GetComponent<MeshFilter>().sharedMesh.triangles.Length / 3;
            submeshCount += root.GetComponent<MeshFilter>().sharedMesh.subMeshCount;
        }

        if (isRecursive)
        {
            foreach (Transform child in root.transform)
            {
                if (child.gameObject.activeInHierarchy || includeDisabledObjects)
                {
                    RecursiveCount(child.gameObject, ref vertexCount, ref triangleCount, ref submeshCount);
                }
            }
        }
    }

    void OnGUI()
    {
        isRecursive = EditorGUILayout.Toggle("Recursive Count", isRecursive);
        includeDisabledObjects = EditorGUILayout.Toggle("Include Disabled Objects", includeDisabledObjects);
        
        vertexCountAll = 0;
        submeshCountAll = 0;
        triangleCountAll = 0;
        foreach (GameObject g in Selection.gameObjects)
        {
            RecursiveCount(g, ref vertexCountAll, ref triangleCountAll, ref submeshCountAll);
        }

        EditorGUILayout.LabelField("Vertices: ", vertexCountAll.ToString());
        EditorGUILayout.LabelField("Triangles: ", triangleCountAll.ToString());
        EditorGUILayout.LabelField("SubMeshes: ", submeshCountAll.ToString());
    }
}