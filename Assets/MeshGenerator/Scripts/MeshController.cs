using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshController : MonoBehaviour {

    private Mesh mesh;
    private MeshRenderer meshRenderer;
    private Vector3[] vertices;
    private int[] triangles;
    private GameObject[] verticesGO;

    private void Awake() {
        mesh = GetComponent<MeshFilter>().mesh;
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = new Color(Random.value, Random.value, Random.value, 1.0f);
        triangles = null;
    }
    	
	// Update is called once per frame
	void Update () {
        if(triangles != null) {
            UpdateMesh();
        }
	}

    public void CreateMesh(Node[] cycle) {
        verticesGO = new GameObject[cycle.Length];
        vertices = new Vector3[cycle.Length];
        for (int i = 0; i < cycle.Length; i++) {
            verticesGO[i] = cycle[i].nodeGO;
        }
        int numTriangles = cycle.Length - 2;
        triangles = new int[numTriangles * 3];
        for (int i = 1; i <= numTriangles; i++){
            triangles[i * 3 - 3] = 0;
            if(IsUp(verticesGO[0].transform.position, verticesGO[i].transform.position, verticesGO[i+1].transform.position)) {
                triangles[i * 3 - 2] = i;
                triangles[i * 3 - 1] = i + 1;
            } else {
                triangles[i * 3 - 2] = i + 1;
                triangles[i * 3 - 1] = i;
            }
        }
    }

    private bool IsUp(Vector3 v1, Vector3 v2, Vector3 v3) {
        Vector3 centroide = (v1 + v2 + v3) / 3;
        return Vector3.Dot(Vector3.Cross(v1-centroide, v2-centroide), Vector3.up) > 0;
    }

    private void UpdateMesh() {
        for (int i = 0; i < verticesGO.Length; i++) {
            vertices[i] = verticesGO[i].transform.position;
        }
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
