using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driving : MonoBehaviour
{
    [SerializeField] Nodes nodes;
    [SerializeField] float maxSteeringAngle = 45f;
    [SerializeField] WheelCollider[] wheelColliders = new WheelCollider[4];

    public bool collisionDetected;

    List<Transform> nodeList = new List<Transform>();
    int currentNode;
    bool firstNode;
    bool atTL;
    float maxTorque = 90f;
    float maxBrakeTorque = 180f;
    [SerializeField] public float currentSpeed;
    [SerializeField] float maxSpeed = 50f;
    float initMaxSpeed;
    Vector3 startPos;
    public float brakeAmt;

    public float BrakeMultiplier { get; set; }
    public bool IsBraking { get; set; }

    void Awake()
    {
        initMaxSpeed = maxSpeed;
    }

    void Start()
    {
        //Grab each node and add to a list.
        foreach(Transform i in nodes.nodes)
        {
            if(i != nodes.transform)
                nodeList.Add(i);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (collisionDetected)
        {
            Traffic();
            currentSpeed = 2 * Mathf.PI * wheelColliders[2].radius * wheelColliders[2].rpm * 60 / 1000;
            return;
        }

        if (atTL)
        {
            Traffic();
            currentSpeed = 2 * Mathf.PI * wheelColliders[2].radius * wheelColliders[2].rpm * 60 / 1000;
            TrafficLightCheck();
            return;
        }

        CheckNode();
        Drive();
        Steering();
        Braking();
        TrafficLightCheck();
    }

    void Drive()
    {
        //Work out the current speed.
        currentSpeed = 2 * Mathf.PI * wheelColliders[2].radius * wheelColliders[2].rpm * 60 / 1000;

        //If we haven't hit the top speed then add torque to the wheels.
        if(currentSpeed < maxSpeed && !IsBraking)
        {
            wheelColliders[2].motorTorque = wheelColliders[3].motorTorque = maxTorque;
        }
        //Else don't add torque.
        else
            wheelColliders[2].motorTorque = wheelColliders[3].motorTorque = 0;

    }

    void Traffic()
    {
        wheelColliders[2].brakeTorque = wheelColliders[3].brakeTorque = brakeAmt = maxBrakeTorque * 20;
        wheelColliders[2].motorTorque = wheelColliders[3].motorTorque = 0;
    }

    void Steering()
    {
        //Convert the world space location of the node to local space.
        Vector3 direction = transform.InverseTransformPoint(nodeList[currentNode].position);
        //Work out the angle to the next point based on the nodes location.
        float newSteerAngle = (direction.x / direction.magnitude) * maxSteeringAngle;
        //Rotate the front wheels to go towards the next point.
        wheelColliders[0].steerAngle = wheelColliders[1].steerAngle = newSteerAngle;
    }

    void Braking()
    {
        //Work out a stopping distance based on the cars speed.
        float stopDistance = currentSpeed * 0.9f;

        //Check if the next node is a corner.
        if (nodes.nodes[currentNode + 1].gameObject.GetComponent<NodeHandler>().isCorner)
        {
            //Work out the distance to the next node relative to the cars current position.
            float DistToNext = Vector3.Distance(transform.position, nodeList[currentNode].position);
            //Work out how far along the car is to the next point based on the previous point or the cars starting position.
            float DistDiv = DistToNext / Vector3.Distance(startPos, nodeList[currentNode].position);

            if (maxSpeed > 8 && DistToNext < stopDistance)
            {
                maxSpeed = initMaxSpeed * DistDiv;
            }

            //Set the stop distance to be atleast 1 so the car brakes before a turn.
            if (stopDistance - 4 < 0)
                stopDistance = 6;

            if (currentSpeed > maxSpeed && DistToNext < stopDistance - 4)
            {
                wheelColliders[2].brakeTorque = wheelColliders[3].brakeTorque = brakeAmt = maxBrakeTorque;
            }
            else
                wheelColliders[2].brakeTorque = wheelColliders[3].brakeTorque = brakeAmt = 0;
        }
    }

    void CheckNode()
    {
        if (!firstNode)
        {
            float closestNode = Vector3.Distance(transform.position, nodeList[0].position);
            currentNode = 0;
            int counter = 0;
            foreach (Transform i in nodeList)
            {
                if (Vector3.Distance(transform.position, i.position) < closestNode)
                {
                    currentNode = counter;
                    closestNode = Vector3.Distance(transform.position, i.position);
                } 
                counter++;
            }
            firstNode = true;
        }
        else
        {
            if (Vector3.Distance(transform.position, nodeList[currentNode].position) < 0.5f && !atTL)
            {
                maxSpeed = initMaxSpeed;
                startPos = transform.position;
                wheelColliders[2].brakeTorque = wheelColliders[3].brakeTorque = brakeAmt = 0;
                if (currentNode + 1 == nodeList.Count)
                    currentNode = 0;
                else
                    currentNode++;
            }
        }

        initMaxSpeed = nodes.nodes[currentNode + 1].GetComponent<NodeHandler>().RoadSpeed;
    }

    public void ResetBrakes()
    {
        wheelColliders[2].brakeTorque = wheelColliders[3].brakeTorque = brakeAmt = 0;
    }

    void TrafficLightCheck()
    {
        if(!nodes.nodes[currentNode + 1].GetComponent<NodeHandler>().canGo)
        {
            maxSpeed = 10f;
        }

        if (Vector3.Distance(transform.position, nodeList[currentNode].position) < 2f && !nodes.nodes[currentNode + 1].GetComponent<NodeHandler>().canGo)
        {
            atTL = true;
        }

        else if (nodes.nodes[currentNode + 1].GetComponent<NodeHandler>().canGo && atTL)
        {
            ResetBrakes();
            atTL = false;
        }
    }
}
