﻿using System.Collections;
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
        //displayObj = physicalRep;
        //CreateObject(strength, position, isRedTeam);
        worldGrid = GameObject.FindWithTag("grid");
        // set color to unit
        this.GetComponent<Renderer>().material = GetCorrectDisplayMaterial(affiliation);

        //show the team the unit is on
        if (isRedTeam)
        {
            this.gameObject.transform.GetChild(0).GetComponent<Renderer>().material = Resources.Load("Red") as Material;
        }
        else
        {
            this.gameObject.transform.GetChild(0).GetComponent<Renderer>().material = Resources.Load("Green") as Material;
        }
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

    private Material GetCorrectDisplayMaterial(int affilNumber)
    {
        // get what color the unit should be
        switch (affilNumber)
        {
            case 1:
                return Resources.Load("White") as Material;
            case 2:
                return Resources.Load("Blue") as Material;
            case 3:
                return Resources.Load("Yellow") as Material;
            case 4:
                return Resources.Load("Black") as Material;
            default:
                return null;
        }
    }
}
