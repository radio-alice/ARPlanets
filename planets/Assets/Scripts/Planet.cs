using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Planet : MonoBehaviour {
    public float G = 10; //quickly modify gravitational strength
    public bool useGravityTrue; //whether or not plane is attracted to stars, updated by spawn script

    GameObject star; //the star to orbit
    Rigidbody getRigidbody; 
    float mass; //mass of planet (taken from rigidbody)

    void Awake () {
        star = GameObject.FindGameObjectWithTag("Star"); //star, rigidbody initialization
        getRigidbody = GetComponent<Rigidbody>();
        mass = getRigidbody.mass;
	}
	
	void Update () {
        if (useGravityTrue) UseGravity(); //interact w stars' gravity once spawn script says to (upon release of planet)
	}

    void UseGravity(){
        float starMass; //mass of  star
        Vector3 starLoc; //location of star
        float distance; //distance to star
        Vector3 direction; //direction to star

        if (star != null)
        {
            starMass = star.GetComponent<Rigidbody>().mass; //get star mass
            starLoc = star.transform.position; //get star pos
            distance = (transform.position - starLoc).magnitude; //get distance to star
            direction = (starLoc - transform.position).normalized; //get direction to star
            getRigidbody.AddForce((G * mass * starMass / (distance * distance)) * direction); //apply gravitational force of star to planet
        }           
    }

    public void AddRandomForce(float Force, Vector2 direction) //ensures orbiting, instead of just falling towards star
    { 
        Vector3 starDirection = (transform.position - star.transform.position).normalized; //get star direction
        Vector3 starTangent = Vector3.Cross(starDirection, Vector3.up); //find tangent to surface of star
        if (Mathf.Approximately(starTangent.sqrMagnitude, 0f)) starTangent = Vector3.Cross(Vector3.up, starDirection); //check if tangent plane is parallel to star and find another tangent if so

        float inputAngle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        Vector3 inputTangent = Quaternion.AngleAxis(inputAngle, starDirection) * starTangent.normalized; //calculate input direction in world space
        getRigidbody.AddForce(Force * inputTangent.normalized); //shoot planet along chosen tangent (causes orbit if all our gravity, force values are balanced) 

    }

	private void OnCollisionEnter(Collision collision)
	{
        if (collision.gameObject.tag == "Star"){
            Destroy(gameObject); //Destroy Planet on star collision
            //ADD EXPLOSION FX HERE
        }
	}
}

