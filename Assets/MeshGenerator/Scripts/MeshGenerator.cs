using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour {

    public GameObject meshPrefab;
    public GameObject meshContainer;

    private List<Node> nodes;           // object that storage the vertices and the edges.
    private Stack<Node> path;           // for recursive searching of cycles in the nodes.
    private List<Stack<Node>> cycles;   // cycles in the node graph.

	// Use this for initialization
	void Start () {
        DetectComponents();             // Get the original game objects: Nodes and Edges.      Output -> nodes (create)
        DetectCycles();                 // Get all the possible cycles in the node graph.       Output -> cycles (create)
        RemoveComposedCycles();         // Detect and remove all the redundant cycles.          Output -> cycles (updates)
        GenerateMeshes();               // Instanciate all the meshes needed.                   Output -> meshContainer (using meshPrefab)
    }

    // Build the graph
    private void DetectComponents() {
        Dictionary<GameObject, Node> gameObjectReference = new Dictionary<GameObject, Node>();
        nodes = new List<Node>();

        foreach (Point point in GetComponentsInChildren<Point>()) {
            GameObject pointGO = point.GetComponentInParent<Transform>().gameObject;
            Node node = new Node(pointGO);
            nodes.Add(node);
            gameObjectReference.Add(pointGO, node);
        }
        foreach (Edge edge in GetComponentsInChildren<Edge>()) {
            gameObjectReference[edge.point1.gameObject].AddLink(gameObjectReference[edge.point2.gameObject]);
            gameObjectReference[edge.point2.gameObject].AddLink(gameObjectReference[edge.point1.gameObject]);
        }
    }

    // Detect and save all the possible cycles.
    private void DetectCycles() {
        cycles = new List<Stack<Node>>();
        path = new Stack<Node>();
        foreach(Node node in nodes) {
            path.Clear();
            path.Push(node);
            ExploreLinks(node);
        }
    }

    // recursive method to build the temporalely path.
    private void ExploreLinks(Node start) {
        foreach(Node node in path.Peek().links) {
            if(node == start) {
                if (path.Count >= 3) {
                    if (IsNewCycle()) {
                        Stack<Node> newCycle = new Stack<Node>(path);
                        cycles.Add(newCycle);
                    }
                }
            } else {
                if (!path.Contains(node)) {
                    path.Push(node);
                    ExploreLinks(start);
                    path.Pop();
                }
            }
        }
    }

    // veryfy if a cycle does not exist in the list of cycles
    private bool IsNewCycle() {
        foreach(Stack<Node> cycle in cycles) {
            if(AreEquals(cycle, path)) {
                return false;
            }
        }
        return true;
    }

    // compare thw cycles to veryfy if they are the same
    private bool AreEquals(Stack<Node> cycle1, Stack<Node> cycle2) {
        if(cycle1.Count != cycle2.Count) {
            return false;
        }
        foreach(Node node in cycle1) {
            if(!cycle2.Contains(node)) {
                return false;
            }
        }
        foreach (Node node in cycle2) {
            if (!cycle1.Contains(node)) {
                return false;
            }
        }
        return true;
    }

    // Detect and remove all the unnecesary cycles
    private void RemoveComposedCycles() {
        List<Stack<Node>> backupCycles = new List<Stack<Node>>(cycles);
        foreach (Stack<Node> cycle1 in backupCycles) {
            foreach (Stack<Node> cycle2 in backupCycles) {
                if (cycle1 != cycle2) {
                    foreach (Stack<Node> cycle3 in backupCycles) {
                        if(cycle3.Count > cycle1.Count && cycle3.Count > cycle2.Count) {
                            if(IsSubCycle(ComposeCycle(cycle1, cycle2), cycle3)) {
                                cycles.Remove(cycle3);
                            }
                        }
                    }
                }
            }
        }
    }

    // combine two cycles in one
    private Stack<Node> ComposeCycle(Stack<Node> cycle1, Stack<Node> cycle2) {
        Stack<Node> composed = new Stack<Node>(cycle1);
        foreach(Node node in cycle2) {
            if(!composed.Contains(node)) {
                composed.Push(node);
            }
        }
        return composed;
    }

    // verify if one cycle is a subcycle of other
    private bool IsSubCycle(Stack<Node> cycle, Stack<Node> subCycle) {
        foreach (Node node in subCycle) {
            if (!cycle.Contains(node)) {
                return false;
            }
        }
        return true;
    }

    // instantiate the final meshes
    private void GenerateMeshes() {
        foreach(Stack<Node> cycle in cycles) {
            GameObject mesh = Instantiate(meshPrefab, Vector3.zero, Quaternion.identity, meshContainer.transform);
            mesh.GetComponent<MeshController>().CreateMesh(cycle.ToArray());
        }
    }


}
