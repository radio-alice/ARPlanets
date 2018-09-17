using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Planet : MonoBehaviour {
    public float G = 10; //quickly modify gravitational strength
    public float forceScalar = 25; //quickly scale orbital force 
    public Transform star; //the star to orbit

    bool released; //whether or not plane has been released and is affected by forces

    Rigidbody getRigidbody; 
    float mass; //mass of planet (taken from rigidbody)
    SphereCollider getCollider;
    TrailRenderer getTrail;
    Material getMaterial;

    float t = 0f; //counter for size scaling
    float starMass;


	void Awake () 
    {
        star = GameObject.FindGameObjectWithTag("Star").transform;
        starMass = star.GetComponent<Rigidbody>().mass;
        transform.parent = star;// set parent to star

        getRigidbody = GetComponent<Rigidbody>();
        mass = getRigidbody.mass;

        getCollider = GetComponent<SphereCollider>();
        getTrail = GetComponent<TrailRenderer>();
        getMaterial = GetComponent<MeshRenderer>().material;
    }

    void OnEnable()
	{
        getCollider.enabled = false; //disable collisions until released
        released = false; //tell script planet is not released
        getRigidbody.constraints = RigidbodyConstraints.FreezeAll; //freeze movement
        getTrail.Clear(); //clear previous trail

        getMaterial.color = Random.ColorHSV(0, 1, .2f, 1, .3f, 1, 1, 1);//random color
        ScaleSize(t); //scale up and down (so there's no blip when scale size kicks in in Update())
	}

	void Update()
    {
        if (!released)
        {
            t += Time.deltaTime; //increment t every frame
            ScaleSize(t); //scale size up and down
        }
    }

	void FixedUpdate()
	{
        if (released) UseGravity(); //interact w stars' gravity once spawn script says to (upon release of planet)
	}

	void UseGravity(){
        float distance; //distance to star
        Vector3 direction; //direction to star

        if (star != null)
        {
            distance = (transform.position - star.position).magnitude; //get distance to star
            direction = (star.position - transform.position).normalized; //get direction to star
            getRigidbody.AddForce((G * mass * starMass / (distance * distance)) * direction); //apply gravitational force of star to planet
            if (distance > 50) gameObject.SetActive(false); //remove if it gets too far away
        }           
    }

    public void AddOrbitalForce(float dragDistance, Vector2 direction) //ensures orbiting, instead of just falling towards star
    {
        float starDistance = (transform.position - star.transform.position).magnitude;
        float force = dragDistance * forceScalar * Mathf.Sqrt(mass) * 1.87f/Mathf.Sqrt(starDistance); //scale force by finger drag, mass, distance to star

        Vector3 starDirection = (transform.position - star.transform.position).normalized; //get star direction
        Vector3 starTangent = Vector3.Cross(direction.normalized, starDirection); //find tangent to surface of star
        if (starTangent.magnitude < .15f) starTangent = Vector3.forward * direction.normalized.magnitude; //check if tangent plane is parallel to star, if so, find another tangent 

        float inputAngle = Mathf.Acos(Vector3.Dot(direction.normalized, starDirection.normalized)) * Mathf.Rad2Deg; //return angle between normal, input direction
        Vector3 inputTangent = Quaternion.AngleAxis(inputAngle, starDirection) * starTangent.normalized; //calculate input direction in world space

        getRigidbody.constraints = RigidbodyConstraints.None; //unfreeze planet
        getRigidbody.AddForce(force * inputTangent.normalized); //shoot planet along chosen tangent (causes orbit)
        released = true; //tell script it's released
        getCollider.enabled = true; //enable collisions
        t = 0f; //reset t
    }

    void OnCollisionEnter(Collision collision)
	{
        if (collision.gameObject.tag == "Star"){
            gameObject.SetActive(false); //remove planet on star collision
            //ADD EXPLOSION FX HERE
        }
	}

	void ScaleSize(float counter)
	{
        transform.localScale = (Vector3.one + (Vector3.one * (Mathf.Sin(counter - Mathf.PI / 2)) * .6f)) * .4f; //scale up and down starting small
        mass = Mathf.Pow((transform.localScale.x / 2), 2f) * 10; //adjust mass with volume
    }
}

