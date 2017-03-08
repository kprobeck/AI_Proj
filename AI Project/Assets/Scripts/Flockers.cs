using UnityEngine;
using System.Collections;
using System;

public class Flockers : MonoBehaviour
{

    //-----------------------------------------------------------------------
    // Class Fields
    //-----------------------------------------------------------------------

    public Vector3 pos;
    [SerializeField] private GameObject seekerTarget;
    
    //movement
    protected Vector3 acceleration;
    protected Vector3 velocity;
    protected Vector3 desired;
    private Rigidbody rigBody;

    GameObject[] obstacles;
    

    public Vector3 Velocity
    {
        get { return velocity; }
    }

    //public for changing in Inspector
    //define movement behaviors
    public float maxSpeed = 6.0f;
    public float maxForce = 12.0f;
    public float mass = 1.0f;
    public float radius = 1.0f;

    //Access to GameManager script
    [SerializeField] private GameObject gameManger;
    private GameManager gm;

    //access to Character Controller component
    CharacterController charControl;

    public float seekWeight = 75.0f;
    public float safeDistance = 15.0f;

    //Seeker's steering force (will be added to acceleration)
    private Vector3 force;

    //WEIGHTING!!!!

    public float avoidObstacleWeight = 100.0f;

    public float seperationWeight = 75.0f;
    public float alignmentWeight = 25.0f;
    public float cohesionWeight = 10.0f;

    //the distance from another ghost
    public float tooClose = 5.0f;

    //-----------------------------------------------------------------------
    // Start - No Update
    //-----------------------------------------------------------------------
    // Call Inherited Start and then do our own
    public void Start()
    {
        pos = transform.position;
        gm = gameManger.GetComponent<GameManager>();
        rigBody = GetComponent<Rigidbody>();
        obstacles = GameObject.FindGameObjectsWithTag("obstacle");
        //initialize
        force = Vector3.zero;
    }

    void Update()
    {

        CalcSteeringForces();

        //add accel to vel
        velocity += acceleration * Time.deltaTime;
        rigBody.AddForce(acceleration);//makes other colliders work
        Debug.Log(gameObject.name + ": " + acceleration);
        //velocity.y = 0; //keeping us on same plane
                        //limit vel to max speed
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        transform.forward = velocity.normalized;
        //move the character based on velocity
        pos += velocity * Time.deltaTime;
        Debug.DrawLine(transform.position, transform.position+velocity, Color.yellow);
        Debug.DrawLine(transform.position, transform.position+acceleration, Color.blue);
        //reset acceleration to 0
        acceleration = Vector3.zero;

        Debug.Log(velocity);
    }

    //-----------------------------------------------------------------------
    // Class Methods
    //-----------------------------------------------------------------------

    void CalcSteeringForces()
    {
        //reset value to (0, 0, 0)
        force = Vector3.zero;
        //ultimateForce = Vector3.zero;

<<<<<<< HEAD
        Vector3 seekPoint = seekerTarget.transform.position + -10 * seekerTarget.transform.forward;
=======
        Vector3 seekPoint = seekerTarget.transform.position + -5 * (seekerTarget.transform.forward);
>>>>>>> 1b5cfd2cd2cbb769ca8dbf02d3b08d7b0e5eba45

        force += Seek(seekPoint) * seekWeight;//seek the center
        force += Seperation();
        force += Alignment();
        force += Cohesion();

        for (int i = 0; i < obstacles.Length; i++)
        {
            force += AvoidObstacle(obstacles[i], safeDistance) * avoidObstacleWeight;
        }

        // avoid invisible walls
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position, fwd, out hit, 10))
        {
            if (hit.collider.gameObject.tag == "invisibleWall")
            {
                force += AvoidObstacle(hit.collider.gameObject, safeDistance) * 2 * avoidObstacleWeight;
                Debug.DrawLine(transform.position, hit.collider.gameObject.transform.position, Color.red);
            }
        }

        //limited the seeker's steering force
        force = Vector3.ClampMagnitude(force, maxForce);
        //ultimateForce = Vector3.ClampMagnitude (ultimateForce, maxForce);

        //applied the steering force to this Vehicle's acceleration (ApplyForce)
        ApplyForce(force);
        //ApplyForce (ultimateForce);
    }

    Vector3 AvoidObstacle(GameObject ob, float safe)
    {

        //reset desired velocity
        desired = Vector3.zero;
        //get radius from obstacle's script
        //float obRad = ob.GetComponent<ObstacleScript>().Radius;
        //get vector from vehicle to obstacle
        Vector3 vecToCenter = ob.transform.position - pos;
        //zero-out y component (only necessary when working on X-Z plane)
        vecToCenter.y = 0;
        //if object is out of my safe zone, ignore it
        if (Mathf.Abs(vecToCenter.magnitude) > safe)
        {
            return Vector3.zero;
        }
        //if object is behind me, ignore it
        if (Vector3.Dot(vecToCenter, transform.forward) < 0)
        {
            return Vector3.zero;
        }
        //if object is not in my forward path, ignore it
        if (Mathf.Abs(Vector3.Dot(vecToCenter, transform.right)) >  radius)
        {
            return Vector3.zero;
        }

        //if we get this far, we will collide with an obstacle!
        //object on left, steer right
        if (Vector3.Dot(vecToCenter, transform.right) < 0)
        {
            desired = transform.right * maxSpeed;
            //debug line to see if the dude is avoiding to the right
            Debug.DrawLine(pos, ob.transform.position, Color.red);
        }
        else
        {
            desired = transform.right * -maxSpeed;
            //debug line to see if the dude is avoiding to the left
            Debug.DrawLine(pos, ob.transform.position, Color.green);
        }
        return desired * (radius - vecToCenter.magnitude);
    }

    protected void ApplyForce(Vector3 steeringForce)
    {
        acceleration += steeringForce / mass;
    }

    //steer away from nearist neighbor when it is too close
    public Vector3 Seperation()
    {
        //finds the nearist neigbor
        float closeness = 10000f;
        int closest = -1;
        bool activate = false;
        for (int i = 0; i < gm.Flock.Length; i++)
        {
            float dis = Mathf.Abs(gm.Flock[i].GetComponent<Flockers>().pos.magnitude - pos.magnitude);
            if (dis < closeness && dis > 0.0f)
            {
                closeness = dis;
                closest = i;
                activate = true;
            }
        }
        //if it is too close
        if (activate)
        {
            if (gm.Flock[closest].GetComponent<Flockers>().pos.magnitude - pos.magnitude < tooClose)//error
            {
                //use the flee method to steer away
                return Flee(gm.Flock[closest].GetComponent<Flockers>().pos) * seperationWeight;
            }
        }
        return Vector3.zero;
    }

    //aligns self with the rest of the flock
    public Vector3 Alignment()
    { //Set desiredVelocity to a vector based on the average direction of the flock.
        return gm.FlockDirection * alignmentWeight;
    }

    //cohere self to center of the flock
    public Vector3 Cohesion()
    {
        //sum the position vectores of each member of the flock --> in GameManagerGO
        ///divide by the number of members  
        //seek this point
        return Seek(gm.CenterOfFlock) * cohesionWeight;
    }

    Vector3 Seek(Vector3 targetPos)
    {
        desired = targetPos - transform.position;
        desired = desired.normalized * maxSpeed;
        desired -= velocity;
        desired.y = 0;
        return desired;
    }

    Vector3 Flee(Vector3 targetPos)
    {
        return -Seek(targetPos);
    }

    Vector3 Pursue(Vector3 targetPos, Vector3 targetVel)
    {//figures out where the target will be and heads towards that
        Vector3 futurePos = targetPos + (targetVel);
        return Seek(futurePos);
    }

    Vector3 Evade(Vector3 targetPos, Vector3 targetVel)
    {//figures out where the target will be and heads away from that
        Vector3 futurePos = targetPos + (targetVel);
        return Flee(futurePos);
    }

}
