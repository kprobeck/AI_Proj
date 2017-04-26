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
        units = new List<Unit>();
    }
	
	// Update is called once per frame
	void Update () {
        checkInputs();
	}

    // see what unit to place based on input, 1, 2, 3, 4 will provide what affiliation to have
    void checkInputs() {
        //Capture mouse location
        Vector3 mouseLocation;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000))//mouse hit terrain
        {
            mouseLocation = hit.point;
        }
        else //mouse didnt hit anything
        {
            return;
        }


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

        if(Input.GetKeyDown(KeyCode.R))//reset
        {
            //removes all nodes from maps
            GameObject[] onScreenUnits = GameObject.FindGameObjectsWithTag("unit");
            foreach (GameObject u in onScreenUnits)
            {
                Destroy(u);
            }

            units = new List<Unit>();
        }

        if (Input.GetMouseButtonDown(1) && hit.collider.gameObject.tag == "unit")//right mouse down, remove
        {
            units.Remove(hit.collider.gameObject.GetComponent<Unit>());
            Destroy(hit.collider.gameObject);
        }

        // Input for switching team to place
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isPlacingRedTeam = !isPlacingRedTeam;
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ShowMap();
        }
    }

    void SpawnUnit(Vector3 location, int strength)
    {
        Debug.Log("spawn: " + strength);
        //unitToPlace = gameObject.AddComponent<Unit>();
        //unitToPlace.InitUnit(isPlacingRedTeam, strength, new Vector3(location.x, location.y + .5f, location.z), unitObject);
        //units.Add(unitToPlace);
        GameObject obj = Instantiate(unitObject, new Vector3(location.x, location.y + .5f, location.z), new Quaternion());
        obj.GetComponent<Unit>().InitUnit(isPlacingRedTeam, strength, new Vector3(location.x, location.y + .5f, location.z), unitObject);
    }

    // create map function
    void ShowMap()
    {
        foreach (Unit u in units)
        {
            u.GetNodesInRange();
        }
    }
}
