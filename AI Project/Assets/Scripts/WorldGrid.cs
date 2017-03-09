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
    public GameObject startObject;
    public GameObject pathfinder;
    private Node startPoint;
    private Node endPoint;
    private Node[,] world; // used to represent the x,z node grid
    List<Vector3> inaccessibleLocations; // represent positions of nodes that can't be reached
    private int arrayDimensionSize;
    private int gridSpacing;

    // populate map when initialized
    void Start()
    {
        arrayDimensionSize = width / 20;
        gridSpacing = width / arrayDimensionSize;

        // initialize map array size
        world = new Node[arrayDimensionSize, arrayDimensionSize];
        startPoint = new Node(new Vector3(startObject.transform.position.x, representationHeight, startObject.transform.position.z));

        GetInaccessibleNodeList();

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
                Node currentNode = new Node(new Vector3(i * gridSpacing, representationHeight, j * gridSpacing)); // create the current node 
                
                // create a representation of the node at the node's position
                GameObject representation = GameObject.Instantiate(cellRepresentation, new Vector3(currentNode.position.x, currentNode.position.y, currentNode.position.z), Quaternion.identity) as GameObject;

                DetermineAccessibility(currentNode);                             
                representation.transform.parent = transform; // assign the parent transform
                world[j, i] = currentNode; // add node to the world array
            }
        }

        DetermineEndPoint(startPoint);
        AStar test = pathfinder.GetComponent<AStar>();
        test.CanStart();
    }

    public void DetermineEndPoint(Node start)
    {
        // determine end point
        do
        {
            endPoint = world[Random.Range(0, arrayDimensionSize - 1), Random.Range(0, arrayDimensionSize - 1)];
        }
        while (dist(start, endPoint) < 100 && endPoint.isAccessible);

        Debug.Log("END POINT: " + endPoint.position);
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

    public void GetInaccessibleNodeList()
    {
        // due to the grid being generated without knowledge of the terrain, this will be hard-coded for simplicity
        inaccessibleLocations = new List<Vector3>();
        inaccessibleLocations.Add(new Vector3(220, 110, 220));
        inaccessibleLocations.Add(new Vector3(220, 110, 200));
        inaccessibleLocations.Add(new Vector3(180, 110, 40));
        inaccessibleLocations.Add(new Vector3(160, 110, 40));
        inaccessibleLocations.Add(new Vector3(160, 110, 60));
        inaccessibleLocations.Add(new Vector3(160, 110, 80));
        inaccessibleLocations.Add(new Vector3(140, 110, 80));
        inaccessibleLocations.Add(new Vector3(120, 110, 80));
        inaccessibleLocations.Add(new Vector3(160, 110, 100));
        inaccessibleLocations.Add(new Vector3(140, 110, 100));
        inaccessibleLocations.Add(new Vector3(120, 110, 100));
        inaccessibleLocations.Add(new Vector3(100, 110, 100));
        inaccessibleLocations.Add(new Vector3(120, 110, 120));
        inaccessibleLocations.Add(new Vector3(100, 110, 120));
        inaccessibleLocations.Add(new Vector3(60, 110, 120));
        inaccessibleLocations.Add(new Vector3(40, 110, 120));
        inaccessibleLocations.Add(new Vector3(40, 110, 140));
        inaccessibleLocations.Add(new Vector3(60, 110, 140));
        inaccessibleLocations.Add(new Vector3(100, 110, 140));
    }

    public int GetDimensionSize()
    {
        return arrayDimensionSize;
    }

    public Node[,] GetMap() // used to return the node array
    {
        return world;
    }

    public Node GetStartPoint() // used to return start point
    {
        return startPoint;
    }

    public void SetStartPoint(Node pt)
    {
        startPoint = pt;
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

    private void DetermineAccessibility(Node n) // set node to inaccessible if it is located in areas that are impossible to reach
    { 
        if (n.position.x == 0 || n.position.z == 0 || n.position.x == 20 || n.position.z == 20 || inaccessibleLocations.Contains(n.position))
        {
            n.isAccessible = false;
        }
    }
    
    // node class for use with the world grid
    public class Node
    { // represents a node on the graph of the terrain, based on the priority item file

        public Vector3 position { get; set; } // the node's position in the world
        public bool isAccessible { get; set; } // flag if the node can be accessed by the a* AI
        private double costSoFar;
        private double estTotalCost;
        private int gridCost; // all nodes cost the same if accessible

        public Node(Vector3 pos)
        {
            position = pos;
            isAccessible = true;
            costSoFar = 0.0;
            estTotalCost = 0.0;
            gridCost = 1;
        }

        public Node(Vector3 pos, bool canAccess)
        {
            position = pos;
            isAccessible = canAccess;
            costSoFar = 0.0;
            estTotalCost = 0.0;
            gridCost = 1;
        }

        // properties for node costs
        public double CostSoFar
        {
            get { return costSoFar; }
            set { costSoFar = value; }
        }

        public double EstTotalCost
        {
            get { return estTotalCost; }
            set { estTotalCost = value; }
        }

        public int GridCost
        {
            get { return gridCost; }
        }

        // Method to compare location values of nodes
        public bool Equals(Node otherNode)
        {
            return this.position.Equals(otherNode.position);
        }
    }
}
