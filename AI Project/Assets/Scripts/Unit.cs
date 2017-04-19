using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    // properties - TODO: edit if they should be public or not, all public now for testing
        
    // affiliation, represents the actual influence the unit provides, scale form 1 - 4
    public int affiliation;

    // boolean to see if on red or green team, TRUE = RED, FALSE = GREEN
    public bool isRedTeam;
    public int influenceRadius = 10;
    public Vector3 location;
    public GameObject displayObj;
    public GameObject worldGrid; // reference for the world grid

	// Use this for initialization
	public Unit(bool isRedTeam, int strength, Vector3 position, GameObject physicalRep) 
    {
        affiliation = strength;
        this.isRedTeam = isRedTeam;
        location = position;
        displayObj = physicalRep;
        CreateObject();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    void CreateObject()
    {
        // TODO: either use Vec3 or lock y position
        GameObject.Instantiate(displayObj, location, Quaternion.identity);
    }

    void GetNodesInRange()
    {
        WorldGrid map = worldGrid.GetComponent<WorldGrid>();
        WorldGrid.Node[,] mapNodes = map.GetMap();
        foreach (WorldGrid.Node n in mapNodes)
        {
            if (Mathf.Abs(n.position.x - location.x) <= influenceRadius && Mathf.Abs(n.position.z - location.z) <= influenceRadius)
            {
                // if in range add influence
                n.AddInfluence(this);
            }
        } 
    }
}
