using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Planet : MonoBehaviour {
    public float G = 10; //quickly modify gravitational strength
    public float forceScalar = 25; //quickly scale orbital force 
    public bool released; //whether or not plane has been released and is affected by forces
    public Transform star; //the star to orbit

    Rigidbody getRigidbody; 
    float mass; //mass of planet (taken from rigidbody)
    SphereCollider getCollider;
    float t = 0f; //counter for size scaling
    float starMass;


	void Awake () {
        star = GameObject.FindGameObjectWithTag("Star").transform;
        starMass = star.GetComponent<Rigidbody>().mass;
        transform.parent = star;// set parent to star

        getRigidbody = GetComponent<Rigidbody>();
        mass = getRigidbody.mass;

        getCollider = GetComponent<SphereCollider>();
        getCollider.enabled = false; //disable collisions until released
        }
	
	void Update () {
        if (released)
        {
            UseGravity(); //interact w stars' gravity once spawn script says to (upon release of planet)
            getCollider.enabled = true;
            t = 0f; //reset t
        } else {
            t += Time.deltaTime; //increment t every frame
            ScaleSize(t); //scale size up and down
        }

	}

    void UseGravity(){
        Vector3 starLoc; //location of star
        float distance; //distance to star
        Vector3 direction; //direction to star

        if (star != null)
        {
            starLoc = star.position; //get star pos
            distance = (transform.position - starLoc).magnitude; //get distance to star
            direction = (starLoc - transform.position).normalized; //get direction to star
            getRigidbody.AddForce((G * mass * starMass / (distance * distance)) * direction); //apply gravitational force of star to planet
            if (distance > 50) Destroy(gameObject); //destroy if it gets too far away
        }           
    }

    public void AddOrbitalForce(float dragDistance, Vector2 direction) //ensures orbiting, instead of just falling towards star
    {
        float force = dragDistance * forceScalar * Mathf.Sqrt(mass); //scale force by finger drag, mass
        Vector3 starDirection = (transform.position - star.transform.position).normalized; //get star direction
        Vector3 starTangent = Vector3.Cross(starDirection, Vector3.up); //find tangent to surface of star
        if (Mathf.Approximately(starTangent.sqrMagnitude, 0f)) starTangent = Vector3.Cross(Vector3.up, starDirection); //check if tangent plane is parallel to star and find another tangent if so
        float inputAngle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;

        Vector3 inputTangent = Quaternion.AngleAxis(inputAngle, starDirection) * starTangent.normalized; //calculate input direction in world space
        getRigidbody.AddForce(force * inputTangent.normalized); //shoot planet along chosen tangent (causes orbit if all our gravity, force values are balanced)
        released = true;
    }

    void OnCollisionEnter(Collision collision)
	{
        if (collision.gameObject.tag == "Star"){
            Destroy(gameObject); //Destroy Planet on star collision
            //ADD EXPLOSION FX HERE
        }
	}

	void ScaleSize(float counter)
	{
        transform.localScale = (Vector3.one + (Vector3.one * (Mathf.Sin(counter - Mathf.PI / 2)) * .6f)) * .4f; //scale up and down starting small
        mass = Mathf.Pow((transform.localScale.x / 2), 2f) * 20; //adjust mass with volume
    }
}

