using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    // properties - TODO: edit if they should be public or not, all public now for testing
        
        // affiliation, represents the actual influence the unit provides, scale form 1 - 4
    public int affiliation;

        // boolean to see if on red or green team, TRUE = RED, FALSE = GREEN
    public bool isRedTeam;

    public Vector2 location;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // TODO: determineTeam function, checks location to determine what team they are one
    void determineTeam() {
        // if locationX is 0 - 4, red
        isRedTeam = true;

        // if locationX is 5-9, green
        isRedTeam = false;
    }
}
