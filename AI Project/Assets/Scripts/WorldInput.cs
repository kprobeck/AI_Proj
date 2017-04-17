using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInput : MonoBehaviour {

    // properties - TODO: edit if they should be public or not, all public now for testing

    // units array, collection of all units in the world
    public Unit[] units;

    // unitToPlace, determines what type of unit to place (what their affiliation should be)
    public Unit unitToPlace;

    // Use this for initialization
    void Start () {
        unitToPlace = new Unit();
	}
	
	// Update is called once per frame
	void Update () {
        checkAffiliation();
	}

    // see what unit to place based on input, 1, 2, 3, 4 will provide what affiliation to have
    void checkAffiliation() {
        if (Input.GetKeyDown("1")) {
            unitToPlace.affiliation = 1;
            return;
        }

        if (Input.GetKeyDown("2"))
        {
            unitToPlace.affiliation = 2;
            return;
        }

        if (Input.GetKeyDown("3"))
        {
            unitToPlace.affiliation = 3;
            return;
        }

        if (Input.GetKeyDown("4"))
        {
            unitToPlace.affiliation = 4;
            return;
        }
    }
}
