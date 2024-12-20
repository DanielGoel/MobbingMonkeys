using UnityEngine;

public class CombineMeshes : MonoBehaviour
{
    [ContextMenu("Combine Meshes")]
    public void Combine()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        Material[] materials = new Material[meshRenderers.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false); // Hide original objects
            materials[i] = meshRenderers[i].sharedMaterial; // Save material references
        }

        MeshFilter mf = gameObject.AddComponent<MeshFilter>();
        MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();

        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine, false); // `false` keeps submeshes for multiple materials
        mf.mesh = combinedMesh;

        mr.materials = materials; // Assign multiple materials to the parent object

        Debug.Log("Meshes combined with multiple submeshes!");
    }
}
