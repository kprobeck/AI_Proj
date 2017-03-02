using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Good supplemental information on the Algorithm in a Unity context http://blog.playmedusa.com/verbose-astar-pathfinding-algorithm-in-c-for-unity3d/
public class WorldGrid : MonoBehaviour // represents the grid of nodes in the terrain, based on the provided Map file
{
    public int width; // width and height of map should be equal
    public int height;
    public GameObject cellRepresentation; // used to visually debug the nodes on the grid
    public float representationHeight;
    public Node startPoint;
    public Node endPoint;
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
                Node currentNode = new Node(new Vector3(i * arrayDimensionSize, representationHeight, j * arrayDimensionSize)); // Populate the world array with Nodes
                GameObject representation = GameObject.Instantiate(cellRepresentation, new Vector3(currentNode.position.x, currentNode.position.y, currentNode.position.z), Quaternion.identity) as GameObject;
                cellRepresentation.transform.parent = transform;
            }
        }
    }

    // distanct calculation method
    int dist(Node a, Node b)
    {
        return (int)Mathf.Ceil(Mathf.Sqrt(Mathf.Pow(a.position.x - b.position.x, 2) + Mathf.Pow(a.position.z - b.position.z, 2)));
    }
}
