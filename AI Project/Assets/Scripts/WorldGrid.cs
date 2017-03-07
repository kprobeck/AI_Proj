using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// references include source code from Professor Bierre and this article with examples in a Unity context: http://blog.playmedusa.com/verbose-astar-pathfinding-algorithm-in-c-for-unity3d/
public class WorldGrid : MonoBehaviour // represents the grid of nodes in the terrain, based on the provided Map file
{
    public int width; // width and height of map should be equal
    public int height;
    public GameObject cellRepresentation; // used to visually debug the nodes on the grid
    public float representationHeight;
    public Node startPoint;
    private Node endPoint;
    private Node[,] world; // used to represent the x,z node grid
    private int arrayDimensionSize;

    // populate map when initialized
    void Start()
    {
        arrayDimensionSize = width / 20;

        // initialize map array size
        world = new Node[arrayDimensionSize, arrayDimensionSize];

        CreateWorldMap();
    }

    void CreateWorldMap()
    {
        // cycle through world Node array and place the cell representation at each 
        for (int i = 0; i < arrayDimensionSize; i++)
        {
            for (int j = 0; j < arrayDimensionSize; j++)
            {
                // Populate the world array with Nodes
                Node currentNode = new Node(new Vector3(i * arrayDimensionSize, representationHeight, j * arrayDimensionSize)); // create the current node 

                // create a representation of the node at the node's position
                GameObject representation = GameObject.Instantiate(cellRepresentation, new Vector3(currentNode.position.x, currentNode.position.y, currentNode.position.z), Quaternion.identity) as GameObject;
                
                representation.AddComponent<Node>(); // add a node to the representation
                representation.transform.parent = transform; // assign the parent transform
                Node node = representation.GetComponent<Node>(); // get the node on the representation
                node.position = currentNode.position; // set its position to the current node
                world[j, i] = node; // add node to the world array
            }
        }

        DetermineEndPoint(startPoint);
    }

    public void DetermineEndPoint(Node start)
    {
        // determine end point
        do
        {
            endPoint = world[Random.Range(0, arrayDimensionSize - 1), Random.Range(0, arrayDimensionSize - 1)];
        }
        while (dist(start, endPoint) < 100);
    }

    public string GetIndexOfValue(Node node)
    {
        // cycle through world Node array and find given value 
        for (int i = 0; i < arrayDimensionSize; i++)
        {
            for (int j = 0; j < arrayDimensionSize; j++)
            {
                if (world[j, i].Equals(node))
                {
                    return "" + j + ", " + i + "";
                }
            }
        }

        return "1, -1";
    }

    public int GetDimensionSize()
    {
        return arrayDimensionSize;
    }

    public Node[,] GetMap() // used to return the node array
    {
        return world;
    }

    public Node GetEndPoint() // used to return goal point
    {
        return endPoint;
    }

    // distanct calculation method
    int dist(Node a, Node b)
    {
        return (int)Mathf.Ceil(Mathf.Sqrt(Mathf.Pow(a.position.x - b.position.x, 2) + Mathf.Pow(a.position.z - b.position.z, 2)));
    }
    
    // node class for use with the world grid
    public class Node : MonoBehaviour
    { // represents a node on the graph of the terrain, based on the priority item file

        public Vector3 position { get; set; } // the node's position in the world
        public bool isAccessible { get; set; } // flag if the node can be accessed by the a* AI

        public Node(Vector3 pos)
        {
            position = pos;
        }

        public Node(Vector3 pos, bool canAccess)
        {
            position = pos;
            isAccessible = canAccess;
        }

        // Method to compare location values of nodes
        public bool Equals(Node otherNode)
        {
            return this.position.Equals(otherNode.position);
        }
    }
}
