using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInput : MonoBehaviour {

    // properties - TODO: edit if they should be public or not, all public now for testing

    // units array, collection of all units in the world
    public List<Unit> units;

    // unitToPlace, determines what type of unit to place (what their affiliation should be)
    public Unit unitToPlace;
    public GameObject unitObject; // used to define the unit object
    public bool isPlacingRedTeam = true;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        checkInputs();
	}

    // see what unit to place based on input, 1, 2, 3, 4 will provide what affiliation to have
    void checkInputs() {
        // TODO: Capture mouse location
        Vector3 mouseLocation;

        if (Input.GetKeyDown("1")) {
            SpawnUnit(mouseLocation, 1);
            return;
        }

        if (Input.GetKeyDown("2"))
        {
            SpawnUnit(mouseLocation, 2);
            return;
        }

        if (Input.GetKeyDown("3"))
        {
            SpawnUnit(mouseLocation, 3);
            return;
        }

        if (Input.GetKeyDown("4"))
        {
            SpawnUnit(mouseLocation, 4);
            return;
        }

        // TODO: Input for switching team to place
    }

    void SpawnUnit(Vector3 location, int strength)
    {
        unitToPlace = new Unit(isPlacingRedTeam, 1, new Vector2(location.x, location.z), unitObject);
        units.Add(unitToPlace);
    }

    // TODO: delete unit function

    // TODO: reset map function

    // TODO: create map function
}
