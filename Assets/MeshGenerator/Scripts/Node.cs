using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
    public List<Node> links;
    public GameObject nodeGO;   // the game object of the original node

    public Node (GameObject go) {
        this.nodeGO = go;
        links = new List<Node>();
    }

    public void AddLink(Node node) {
        links.Add(node);
    }

}
