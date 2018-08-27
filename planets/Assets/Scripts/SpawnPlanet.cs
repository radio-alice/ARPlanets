using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore;

#if UNITY_EDITOR
using Input = GoogleARCore.InstantPreviewInput; //Get input from phone if instant preview is available
#endif

public class SpawnPlanet : MonoBehaviour {

    public float randomForceStrength = 25; //strength of random force applied to plane to make it orbit
    public Material arSpec; //materials
    public Material arDiff;
    public GameObject arrow; //arrow to be displayed

    bool planetSpawnedNotStarted = false; //tells whether a planet has been spawned (on touch down), but not released (touch up)
    List<GameObject> planets; //list of planets
    float t = 0f; //counter for planet size

    Star star; //the star
    bool starSpawned = false; //has the star spawned


    Vector2 touchStart; //variables to store touch position to calculate launch vector
    Vector2 touchEnd;
    Vector2 currentTouch;
    float dragDistance; //how far finger dragged

    void Awake()
    {
        planets = new List<GameObject>(); //intialize planets
        star = GameObject.FindWithTag("Star").GetComponent<Star>(); //get star component
	}

	void Update () {
        starSpawned = star.starEnabled; //update local star bool with star state

        if (starSpawned)
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    SpawnPlant(); //spawn planet on touch down
                    if (planetSpawnedNotStarted) ChangePlanetSize(t); //scale planet size if touch spawned planet, not destroyed one
                   
                }

                if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)
                {
                    t += Time.deltaTime; //increment t
                    ChangePlanetSize(t); //scale planet
                    DisplayArrow();
                }

                if (Input.GetTouch(0).phase == TouchPhase.Ended && planetSpawnedNotStarted == true)
                {
                    StartPlanet(); //release (add forces to) planet on touch up ONLY if the touch down spawned a planet (rather than destroying a planet)
                }

                currentTouch = Input.GetTouch(0).position; //update touch position
            }
        }
	}

    void SpawnPlant(){ //spawns a planet
        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);//initialize ray
        RaycastHit hit;
        LayerMask layerMask = (1 << 9) | (1 << 10) | (1 << 5); //only shoot rays at layers 5, 9 & 10 (ui, spawn points and planets)

        if (Physics.Raycast(ray, out hit, layerMask)) //cast ray
        {
            if (hit.transform.gameObject.layer == 9) //if ray hits spawn layer
            {
                planets.Add(GameObject.CreatePrimitive(PrimitiveType.Sphere)); //add planet to list 
                GameObject planet = planets[planets.Count - 1];  //local reference for planet
                planet.transform.parent = star.transform; //set parent to star
                planet.transform.position = hit.point; //spawn planet at touch point 
                planet.layer = 10; //add planet to PlanetLayer

                planet.AddComponent<Rigidbody>(); //add a rigidbody
                planet.GetComponent<Rigidbody>().useGravity = false; //disable Unity gravity
                planet.AddComponent<Planet>(); //add planet script
                planet.GetComponent<MeshRenderer>().material = arSpec; //set material to AR compatible
                planet.GetComponent<SphereCollider>().enabled = false; //disable colliders until released

                arrow.SetActive(true); //display arrow
                arrow.transform.localPosition = Camera.main.ScreenToWorldPoint(touchStart);//move arrow to touch position
                arrow.transform.localPosition += Vector3.forward * 30;

                touchStart = Input.GetTouch(0).position;
		planetSpawnedNotStarted = true; 
            }

            else if (hit.transform.gameObject.layer == 10 && hit.transform.gameObject != null) //if click hits a planet

            {
                Destroy(hit.transform.gameObject); //destroy that MF
            }
        }
    }

    void StartPlanet(){ //add forces to planet
        touchEnd = Input.GetTouch(0).position;
        Vector2 dragDirection = (touchStart - touchEnd).normalized;
        dragDistance = Mathf.Abs((touchStart - touchEnd).magnitude) / 300;

        Planet planet = planets[planets.Count - 1].GetComponent<Planet>(); //access planet script from most recently added planet
        planet.AddRandomForce(dragDistance * randomForceStrength * Mathf.Sqrt(planet.GetComponent<Rigidbody>().mass), dragDirection); //add some orbital force in proportion to mass/drag distance
        planet.useGravityTrue = true; //add some gravity

        planet.GetComponent<SphereCollider>().enabled = true; //enable collisions

        arrow.SetActive(false); //remove arrow

        planetSpawnedNotStarted = false; //tell script that we released the planet that we just spawned
        t = 0f; //reset t
    }

    void ChangePlanetSize(float t){ //scale planet as player holds down finger
        Transform planet = planets[planets.Count - 1].GetComponent<Transform>(); //get planet transform
        planet.localScale = (Vector3.one + (Vector3.one * (Mathf.Sin(t - Mathf.PI / 2)) * .6f)) * .4f; //scale up and down starting small
        planet.GetComponent<Rigidbody>().mass = Mathf.Pow((planet.localScale.x/2),2f) * (4/3) * Mathf.PI; //adjust mass with volume
    }

    void DisplayArrow(){
        Vector2 touchDisplace = new Vector2(currentTouch.x - touchStart.x, currentTouch.y - touchStart.y);

        arrow.transform.localScale = Vector3.one * touchDisplace.magnitude;
        arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(-touchDisplace.x, -touchDisplace.y) * Mathf.Rad2Deg));
    }
}
