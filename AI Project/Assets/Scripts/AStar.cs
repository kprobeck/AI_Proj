using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{ // the behavior the character will follow, based on provided AStar file

    public GameObject gridObject; // holds the world grid object to pathfind in
    private WorldGrid grid;
    private bool hasArrived;
    private List<WorldGrid.Node> path;
    WorldGrid.Node[,] gridMap;
    private List<Vector3> blockedNodes;
    private int currentPathNum = 0; //node in list
    [SerializeField]
    private float nodeArriveDistance;


    // Use this for initialization
    void Start()
    {
        hasArrived = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasArrived && path != null)
        {
            drawPath();
            // Seek points in path
            /*Get direction to point, arrive in a box*/
            Vector3 Displace = new Vector3(path[currentPathNum].position.x - transform.position.x, path[currentPathNum].position.y - transform.position.y, path[currentPathNum].position.z - transform.position.z);
            Vector3 currTargetPos = path[currentPathNum].position;
            if ((transform.position.x <= currTargetPos.x + nodeArriveDistance && transform.position.x >= currTargetPos.x - nodeArriveDistance) && (transform.position.z <= currTargetPos.z + nodeArriveDistance && transform.position.z >= currTargetPos.z - nodeArriveDistance))//Displace.magnitude <= nodeArriveDistance)
            {
                if (currentPathNum >= path.Count - 1)
                {
                    // notify of reaching last point
                    HasReachedGoal();
                    return;
                }
                else
                {
                    //Debug.Log("Arrived at [" + path[currentPathNum].position.x + ", " + path[currentPathNum].position.y + ", " + path[currentPathNum].position.z + "]");
                    currentPathNum++;
                }

            }
            //Displace.y = 0;
            GetComponent<Rigidbody>().AddForce(Displace.normalized * 75);
        }
        else if (hasArrived) // if path complete, start new path
        {
            hasArrived = false; // set arrival flag to false
            grid.SetStartPoint(grid.GetEndPoint()); // old end point is new start point
            grid.DetermineEndPoint(grid.GetStartPoint()); // get a new end point
            path = FindPath(grid.GetStartPoint(), grid.GetEndPoint()); // find a new path
            //outputPath();
            currentPathNum = 0;
        }
    }

    public void CanStart()
    {
        grid = gridObject.GetComponent<WorldGrid>();
        gridMap = grid.GetMap();
        path = FindPath(grid.GetStartPoint(), grid.GetEndPoint());
        //outputPath();
    }

    public void HasReachedGoal()
    {
        hasArrived = true;
    }

    public List<WorldGrid.Node> FindPath(WorldGrid.Node start, WorldGrid.Node end)//A*
    {
        // open queue and setup for algorithm
        PriorityQueue open = new PriorityQueue();
        List<WorldGrid.Node> closed = new List<WorldGrid.Node>();

        // end of path cost
        double endNodeHeuristic = 0;
        WorldGrid.Node endNode = null;
        WorldGrid.Node endNodeRecord = null;


        // determine estimated cost (distance) for path
        start.EstTotalCost = GetEstCost(start, end);
        open.Add(start); // add starting point

        while (!open.isEmpty())
        {
            // grab first entry
            WorldGrid.Node current = open.Get(0);

            if (current.position.Equals(end.position)) // goal met
            {
                // end node is not in the returned list, add it here to get seeker to seek it
                if (!closed.Contains(current))
                {
                    closed.Add(current);
                }

                // start node is in the closed list, not needed for seeking, remove here
                if (closed.Contains(start))
                {
                    closed.Remove(start);
                }

                // send list back
                return closed;
            }

            // determine legal moves from current -- only 90 degree turns allowed for simplicity
            WorldGrid.Node[] potentialMoves = GetNextMoves(current);
            open.Clear();

            for (int i = 0; i < potentialMoves.Length; i++)
            {
                WorldGrid.Node currentMove = potentialMoves[i];

                if (currentMove.position.x > 0 && currentMove.position.z > 0 && currentMove.isAccessible)
                {
                    // get move as end node
                    endNode = currentMove;

                    // add the move's cost to the total
                    double endNodeCost = current.CostSoFar + endNode.GridCost; // each node's cost is the same

                    if (closed.Contains(endNode))
                    {
                        // get the index of the node
                        int index = closed.IndexOf(endNode);

                        if (index != -1) // should pass if closed contains endnode
                        {
                            if (closed[index].CostSoFar <= endNodeCost)
                            {
                                // current move is longer
                            }
                            else
                            {
                                // current move is shorter than node in closed list
                                endNodeRecord = closed[index];
                                closed.RemoveAt(index);
                                endNodeHeuristic = endNodeRecord.GridCost - endNodeRecord.CostSoFar; // keeps from being reset by estimation method
                            }
                        }
                    }
                    else
                    {
                        if (open.Contains(endNode))
                        {
                            int index = open.IndexOf(endNode);

                            if (index != -1)
                            {
                                if (open.Get(index).CostSoFar <= endNodeCost)
                                {
                                    // current move is longer
                                }
                            }

                            endNodeRecord = open.Get(index); // record last end Node
                            endNodeHeuristic = endNodeRecord.GridCost - endNodeRecord.CostSoFar; // keeps from being reset by estimation method
                        }
                        else // unvisited Node
                        {
                            endNodeRecord = currentMove; // record last end Node

                            // get cost estimate from potential move
                            endNodeHeuristic = GetEstCost(currentMove, end);

                            // update costs
                            endNodeRecord.CostSoFar = endNodeCost;
                            endNodeRecord.EstTotalCost = endNodeCost + endNodeHeuristic;

                            // add to open queue
                            if (!open.Contains(endNodeRecord))
                            {
                                open.Add(endNodeRecord);
                            }
                        }
                    }
                }

                // done evaluating current moves
                if (i == potentialMoves.Length - 1)
                {
                    open.Remove(current);
                    closed.Add(current);
                    endNode = current;
                }
            } // end for
        } // end while

        // check if reached end or error
        if (!endNode.position.Equals(end.position))
        {
            // if error, try to get new end point and path
            grid.DetermineEndPoint(start);
            List<WorldGrid.Node> newPath = FindPath(start, grid.GetEndPoint());
            return newPath;
        }
        else
        {
            // end node is not in the returned list, add it here to get seeker to seek it
            if (!closed.Contains(endNode))
            {
                closed.Add(endNode);
            }

            // start node is in the closed list, not needed for seeking, remove here
            if (closed.Contains(start))
            {
                closed.Remove(start);
            }
            return closed;
        }
    }

    private WorldGrid.Node[] GetNextMoves(WorldGrid.Node currNode) // determine possible nodes to move too from current node
    {
        WorldGrid.Node[] moves = new WorldGrid.Node[4];// create nodes array

        // get the index of the current node in the map array
        string indexStr = grid.GetIndexOfValue(currNode);

        // split returned string and parse values into indices
        string[] indicesStrs = indexStr.Split(',');
        int indexA = int.Parse(indicesStrs[0]);
        int indexB = int.Parse(indicesStrs[1]);

        // create default illegal node
        WorldGrid.Node illegalNode = new WorldGrid.Node(new Vector3(-1, -1, -1));
        illegalNode.isAccessible = false;
        
        // generate possible moves
        moves[0] = indexB > 0 ? gridMap[indexA, indexB - 1] : illegalNode;
        moves[1] = indexA > 0 ? gridMap[indexA - 1, indexB] : illegalNode;
        moves[2] = indexB < (grid.GetDimensionSize() - 2) ? gridMap[indexA, indexB + 1] : illegalNode; // dimension size - 2 because size - 1 = last entry and adding one would go out of bounds
        moves[3] = indexA < (grid.GetDimensionSize() - 2) ? gridMap[indexA + 1, indexB] : illegalNode;

        return moves;
    }

    private double GetEstCost(WorldGrid.Node current, WorldGrid.Node goal)
    {
        return Mathf.Sqrt(Mathf.Pow((current.position.x - goal.position.x), 2) + Mathf.Pow((current.position.z - goal.position.z), 2)) * 3;
    }

    public class PriorityQueue // priority queue implementation for use by the AStar script methods, based on the provided priority queue file
    {
        List<WorldGrid.Node> pQueue = new List<WorldGrid.Node>(); // list to hold node items

        public void Add(WorldGrid.Node item) // add the new node to the collection, ensure it is accessible
        {
            if (pQueue.Count == 0 && item.isAccessible) // add item only if accessible 
            {
                pQueue.Add(item);
                return;
            }

            // insert the new item in front of a value that is equivalent or more expensive
            for (int i = 0; i < pQueue.Count; i++)
            {
                WorldGrid.Node currentNode = pQueue[i];
                if (currentNode.EstTotalCost >= item.EstTotalCost)
                {
                    pQueue.Insert(i, item);
                    break;
                }
            }
        }

        public WorldGrid.Node Remove()
        {
            if (pQueue.Count < 1)
            {
                return null; // queue is empty, nothing to return
            }

            // get and return first value in queue
            WorldGrid.Node firstElem = pQueue[0];
            pQueue.RemoveAt(0); // remove from queue
            return firstElem;
        }

        // remove a specific Node value
        public void Remove(WorldGrid.Node node)
        {
            pQueue.Remove(node);
        }

        public bool isEmpty() // property to determine if queue has values
        {
            return pQueue.Count < 1;
        }

        // Determine if specific Node is in queue
        public bool Contains(WorldGrid.Node test)
        {
            return pQueue.Contains(test);
        }

        // get the value at defined index
        public WorldGrid.Node Get(int i)
        {
            return Remove();
        }

        // get index of specific Node
        public int IndexOf(WorldGrid.Node node)
        {
            return pQueue.IndexOf(node);
        }

        // clear list items for new move consideration
        public void Clear()
        {
            pQueue.Clear();
        }
    }

    private void outputPath() //for debugging
    {
        string nodePath = "";
        for (int i = 0; i < path.Count; i++)
        {
            WorldGrid.Node n = path[i];
            
            if (i == path.Count - 1)//end of path
            {
                nodePath += "[" + n.position.x + ", " + n.position.y + ", " + n.position.z + "]";
            }
            else
            {
                nodePath += "[" + n.position.x + ", " + n.position.y + ", " + n.position.z + "] --> ";
            }
        }

        Debug.Log(nodePath);
    }

    private void drawPath() //for debugging
    {
        for (int i = 0; i < path.Count; i++)
        {
            if (i != path.Count - 1)//end of path
            {
                Debug.DrawLine(path[i].position, path[i + 1].position, Color.red);
            }
        }
    }
}
