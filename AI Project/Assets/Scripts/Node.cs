using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour { // represents a node on the graph of the terrain, based on the priority item file

    public Vector3 position { get; set; } // the node's position in the world
    public bool isAccessible { get; set; } // flag if the node can be accessed by the a* AI

    public Node(Vector3 pos)
    {
        position = pos;
    }

	public virtual bool isObstacle()
    {
        return !isAccessible;
    }

    // Method to compare location values of nodes
    public bool Equals(Node otherNode)
    {
        return this.position.Equals(otherNode.position);
    }
}
