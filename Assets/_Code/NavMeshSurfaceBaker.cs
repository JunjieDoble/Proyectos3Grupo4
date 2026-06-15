using UnityEditor;
using UnityEngine;
using Unity.AI.Navigation; 

public class NavMeshSurfaceBaker
{
    // Creates a new menu item with the hotkey Ctrl+N / Cmd+N
    [MenuItem("Tools/Bake All NavMesh Surfaces %n")]
    public static void BakeAllNavMeshes()
    {
        // Find all NavMeshSurface components currently loaded in the hierarchy
        NavMeshSurface[] surfaces = Object.FindObjectsOfType<NavMeshSurface>();

        if (surfaces.Length == 0)
        {
            Debug.LogWarning("No NavMeshSurfaces found in the scene.");
            return;
        }

        Debug.Log($"Baking {surfaces.Length} NavMesh Surface(s)...");

        // Iterate through each and bake it
        foreach (NavMeshSurface surface in surfaces)
        {
            surface.BuildNavMesh();
        }

        Debug.Log("NavMesh Surfaces baked successfully!");
    }
}