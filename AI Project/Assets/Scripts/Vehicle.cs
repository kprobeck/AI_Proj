using UnityEngine;
using System.Collections;

//use the Generic system here to make use of a Flocker list later on
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]

abstract public class Vehicle {
/*
    //-----------------------------------------------------------------------
    // Class Fields
    //-----------------------------------------------------------------------

    public GameObject seekerTarget;

    //movement
    protected Vector3 acceleration;
    protected Vector3 velocity;
    protected Vector3 desired;

    public Vector3 Velocity {
        get { return velocity; }
    }

    //public for changing in Inspector
    //define movement behaviors
    public float maxSpeed = 6.0f;
    public float maxForce = 12.0f;
    public float mass = 1.0f;
    public float radius = 1.0f;

	//Access to GameManager script
	//public GameManager gm;

    //access to Character Controller component
    CharacterController charControl;

    public float seekWeight = 75.0f;
    public float safeDistance = 5.0f;

    private RaycastHit info;//info about what it hit

    abstract protected void CalcSteeringForces();

    //-----------------------------------------------------------------------
    // Start and Update
    //-----------------------------------------------------------------------
	virtual public void Start(){
        //acceleration = new Vector3 (0, 0, 0);     
        acceleration = Vector3.zero;
        velocity = transform.forward;
        charControl = GetComponent<CharacterController>();
		//gm = GameObject.Find("GameManagerGO").GetComponent<GameManager>(); 
	}

	
	// Update is called once per frame
	protected void Update () {

		//calculate all necessary steering forces
		CalcSteeringForces();

        Debug.DrawLine(transform.position, velocity + transform.position, Color.black);//the wall vector, Debug

        if (Physics.Raycast(transform.position,transform.forward,out info, safeDistance))
        {
            
            for (int i = 0; i < gm.allWalls.Length; i++) {
                if(info.transform == gm.allWalls[i].transform)
                {
                    ApplyForce(Vector3.ClampMagnitude(WallFollowing(gm.allWalls[i]), maxForce*2));
                    break;
                }
            }
        }

		//add accel to vel
		velocity += acceleration * Time.deltaTime;
		velocity.y = 0; //keeping us on same plane
		//limit vel to max speed
		velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

		transform.forward = velocity.normalized;
		//move the character based on velocity
		charControl.Move(velocity * Time.deltaTime);
        //Debug.DrawLine(transform.position, transform.position+velocity, Color.yellow);
        //Debug.DrawLine(transform.position, transform.position+acceleration, Color.blue);
        //reset acceleration to 0
        acceleration = Vector3.zero;

	}


    //-----------------------------------------------------------------------
    // Class Methods
    //-----------------------------------------------------------------------

    protected void ApplyForce(Vector3 steeringForce) {
        acceleration += steeringForce / mass;
    }

    protected Vector3 Seek(Vector3 targetPos) {
        desired = targetPos - transform.position;
        desired = desired.normalized * maxSpeed;
        desired -= velocity;
        desired.y = 0;
        return desired;
    }

	protected Vector3 Flee(Vector3 targetPos) {
		desired = targetPos - transform.position;
		desired = desired.normalized * maxSpeed;
		desired -= velocity;
		desired.y = 0;
		return -desired;
	}

    protected Vector3 Pursue(Vector3 targetPos, Vector3 targetVel)
    {//figures out where the target will be and heads towards that
        Vector3 futurePos = targetPos + (targetVel);
        return Seek(futurePos);
    }

    protected Vector3 Evade(Vector3 targetPos, Vector3 targetVel)
    {//figures out where the target will be and heads away from that
        Vector3 futurePos = targetPos + (targetVel);
        return Flee(futurePos);
    }

    protected Vector3 WallFollowing(GameObject w)//advanced steering behavior
    {
        Vector3 wallVec = w.GetComponent<WallScript>().WallVector;
        Vector3 star = w.GetComponent<WallScript>().Start_Point.transform.position;
        //find the vehicles future position
        Vector3 future = (transform.forward * 3);
        //find the closest point on the wall
        Vector3 theClosest = Vector3.Project(future + (transform.position- star), wallVec) + star;

        Debug.DrawLine(transform.position, future + transform.position, Color.blue);//the future position, Debug
        Debug.DrawLine(transform.position,theClosest, Color.green); //from position to where the future is on the wall, Debug
        Debug.DrawLine(star, star + wallVec, Color.red);//the wall vector, Debug
        Debug.DrawLine(star, theClosest, Color.cyan);//the wall vector, Debug

        Vector3 targetVector = (transform.position+future)-theClosest;
        Debug.DrawLine(theClosest, future + transform.position, Color.grey); //from position to where the future is on the wall, Debug
        //get the normal of that wall at a determined length
        //seek that spot
        Vector3 targetVectorAbs = new Vector3(Mathf.Abs(targetVector.x), Mathf.Abs(targetVector.y), Mathf.Abs(targetVector.z));
        Debug.DrawLine(theClosest, theClosest + (targetVectorAbs.normalized * 4), Color.magenta); //the absolute vector, Debug
        //pacman on left
        if (Vector3.Dot(transform.position - w.transform.position, targetVectorAbs) < 0)
        {
            Debug.DrawLine(theClosest, theClosest + (targetVectorAbs.normalized * -3), Color.yellow); //from position to where the future is on the wall, Debug
            return Seek(theClosest + targetVectorAbs.normalized * -3) * seekWeight * seekWeight;
        }
        else//on right
        {
            Debug.DrawLine(theClosest, theClosest + (targetVectorAbs.normalized * 3), Color.yellow); //from position to where the future is on the wall, Debug
            return Seek(theClosest + targetVectorAbs.normalized * 3) * seekWeight * seekWeight;
        }
            
    }
    */
}
