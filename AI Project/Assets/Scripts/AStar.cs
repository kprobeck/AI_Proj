using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour { // the behavior the character will follow, based on provided AStar file

    [SerializeField]
    GameObject character; // used to determine object to utilize behavior
    public WorldGrid grid; // holds the world grid object to pathfind in
    private bool hasArrived;
    private List<WorldGrid.Node> path;

    // Use this for initialization
    void Start () {
        hasArrived = false;
        path = FindPath(grid.GetStartPoint(), grid.GetEndPoint());
    }
	
	// Update is called once per frame
	void Update () {
        
        if (!hasArrived)
        {
            // TODO: Seek points in path

        }
        else // if path complete, start new path
        {
            hasArrived = false; // set arrival flag to false
            grid.SetStartPoint(grid.GetEndPoint()); // old end point is new start point
            grid.DetermineEndPoint(grid.GetStartPoint()); // get a new end point
            FindPath(grid.GetStartPoint(), grid.GetEndPoint()); // find a new path
        }
	}

    public List<WorldGrid.Node> FindPath(WorldGrid.Node start, WorldGrid.Node end)
    {
        // open queue and setup for algorithm
        PriorityQueue open = new PriorityQueue();
        List<WorldGrid.Node> closed = new List<WorldGrid.Node>();
        WorldGrid.Node endNode = null;
        WorldGrid.Node endNodeRecord = null;

        // only 90 degree turns allowed for simplicity
        open.Add(grid.GetStartPoint()); // add starting point

        while (!open.isEmpty())
        {
            // grab first entry
            WorldGrid.Node current = open.Get(0);

            if (current.position.Equals(end)) // goal met
            {
                hasArrived = true; // flag arrival

                // send list back
                return closed;
            }

            // determine legal moves from current
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

                if (closed.Contains(endNode))
                {
                    // get the index of the node
                    int index = closed.IndexOf(endNode);

                    if (index != -1) // should pass if closed contains endnode
                    {
                        // would determine if costs are different here
                        // TODO: Determine if node would be closest to end point
                        continue;
                    }

                    endNodeRecord = closed[index]; // record last end Node
                }
                else
                {
                    if (open.Contains(endNode))
                    {
                        int index = open.IndexOf(endNode);

                        if (index != -1)
                        {
                            // would determine if costs are different here
                            // TODO: Determine if node would be closest to end point
                            continue;
                        }

                        endNodeRecord = open.Get(index); // record last end Node
                    }
                    else // unvisited Node
                    {
                        endNodeRecord = currentMove; // record last end Node

                        // add to open queue
                        if (!open.Contains(endNodeRecord))
                        {
                            open.Add(endNodeRecord);
                        }
                    }
                }

                // done with current move
                open.Remove(currentMove);
                closed.Add(currentMove);
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

    public class PriorityQueue // priority queue implementation for use by the AStar script methods, based on the provided priority queue file
    {
        List<WorldGrid.Node> pQueue; // list to hold node items

        void Start ()
        {
            pQueue = new List<WorldGrid.Node>();
        }

        public void Add(WorldGrid.Node item) // add the new node to the collection, ensure it is accessible
        {
            if (item.isAccessible) // add item only if accessible 
            {
                pQueue.Add(item);
                return;
            }

            // no costs associated with nodes, would handle here
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
