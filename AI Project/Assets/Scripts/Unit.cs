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
    Material mat;

	// Use this for initialization
	public void InitUnit(bool isRedTeam, int strength, Vector3 position, GameObject physicalRep) 
    {
        affiliation = strength;
        this.isRedTeam = isRedTeam;
        location = position;
        displayObj = physicalRep;
        CreateObject(strength, position, isRedTeam);
    }
	
	// Update is called once per frame
	void Update () {

        // get what color the unit should be
        switch (affiliation)
        {
            case 1:
                mat = Resources.Load("White") as Material;
                break;

            case 2:
                mat = Resources.Load("Blue") as Material;
                break;

            case 3:
                mat = Resources.Load("Yellow") as Material;
                break;

            case 4:
                mat = Resources.Load("Black") as Material;
                break;

            default:
                break;
        }

        // set color to unit
        this.GetComponent<Renderer>().material = mat;

    }
    
    void CreateObject(int strength, Vector3 pos, bool redTeamorNot)
    {
        // TODO: either use Vec3 or lock y position

        // ADD NEEDED INFO FOR UNTIS
        displayObj.GetComponent<Unit>().affiliation = strength;
        displayObj.GetComponent<Unit>().influenceRadius = influenceRadius;
        displayObj.GetComponent<Unit>().location = pos;
        displayObj.GetComponent<Unit>().isRedTeam = redTeamorNot;

        // after all info is given, NOW instantiate object
        GameObject.Instantiate(displayObj, location, Quaternion.identity);

    }

    public void GetNodesInRange()
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
