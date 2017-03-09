using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{ // the behavior the character will follow, based on provided AStar file

    [SerializeField]
    GameObject character; // used to determine object to utilize behavior
    public GameObject gridObject; // holds the world grid object to pathfind in
    private WorldGrid grid;
    private bool hasArrived;
    private List<WorldGrid.Node> path;
    private bool readyToStart = false;

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
            // TODO: Seek points in path
            /* ScriptWithSeeking seek = character.GetComponent<ScriptWithSeeking>();
               seek.Seek(path); -- this function could take the whole path and handle seeking each point 
               
               In seek script, once character reaches goal:
               AStar pathfinder = character.GetComponent<AStar>(); // the seek script would need a public GameObject Reference to the player (identical to this script)
               pathfinder.HasReachedGoal(); // handles setting a new target, reruns A*, returns new path for seeking
            */ 
        }
        else if (hasArrived) // if path complete, start new path
        {
            hasArrived = false; // set arrival flag to false
            grid.SetStartPoint(grid.GetEndPoint()); // old end point is new start point
            grid.DetermineEndPoint(grid.GetStartPoint()); // get a new end point
            path = FindPath(grid.GetStartPoint(), grid.GetEndPoint()); // find a new path
        }
    }

    public void CanStart()
    {
        grid = gridObject.GetComponent<WorldGrid>();
        path = FindPath(grid.GetStartPoint(), grid.GetEndPoint());
        readyToStart = true;
    }

    public void HasReachedGoal()
    {
        hasArrived = true;
    }

    public List<WorldGrid.Node> FindPath(WorldGrid.Node start, WorldGrid.Node end)
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

            if (current.position.Equals(end)) // goal met
            {
                // send list back
                return closed;
            }

            // determine legal moves from current -- only 90 degree turns allowed for simplicity
            WorldGrid.Node[] potentialMoves = GetNextMoves(current);

            for (int i = 0; i < potentialMoves.Length; i++)
            {
                WorldGrid.Node currentMove = potentialMoves[i];
                if (currentMove.position.x < 0 || currentMove.position.y < 0 || !currentMove.isAccessible)
                {
                    continue; // move to next as this node is illegal
                }

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
                            continue; // current move is longer
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
                                continue; // current move is longer
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

                // done evaluating current moves
                if (i == potentialMoves.Length - 1)
                {
                    open.Remove(current);
                    closed.Add(current);
                }
            } // end for
        } // end while

        // check if reached end or error
        if (!endNode.position.Equals(end.position))
        {
            return null;
        }
        else
        {
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

        WorldGrid.Node[,] gridMap = grid.GetMap();

        // generate possible moves
        moves[0] = indexB > 0 ? gridMap[indexA, indexB - 1] : illegalNode;
        moves[1] = indexA > 0 ? gridMap[indexA - 1, indexB] : illegalNode;
        moves[2] = indexB < (grid.GetDimensionSize() - 2) ? gridMap[indexA, indexB + 1] : illegalNode; // dimension size - 2 because size - 1 = last entry and adding one would go out of bounds
        moves[3] = indexA < (grid.GetDimensionSize() - 2) ? gridMap[indexA + 1, indexB] : illegalNode;

        return moves;
    }

    private double GetEstCost(WorldGrid.Node current, WorldGrid.Node goal)
    {
        return Mathf.Sqrt(Mathf.Pow((current.position.x - goal.position.x), 2) + Mathf.Pow((current.position.y - goal.position.y), 2)) * 3;
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
            return pQueue[i];
        }

        // get index of specific Node
        public int IndexOf(WorldGrid.Node node)
        {
            return pQueue.IndexOf(node);
        }
    }
}
