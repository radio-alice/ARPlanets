using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlanet : MonoBehaviour {
    
    public GameObject arrow; //arrow to be displayed
    public GameObject planet;

    bool planetSpawnedNotStarted = false; //tells whether a planet has been spawned (on touch down), but not released (touch up)
    List<GameObject> planets; //list of planets
    GameObject newPlanet; //most recently spawned planet

    bool starSpawned; //has the star spawned

    Arrow getArrow;
    Vector2 touchStart; //variables to store touch position to calculate launch vector
    Vector2 touchEnd;
    Vector2 currentTouch;
    Vector2 touchDisplace;
    float dragDistance; //how far finger dragged

    void Awake()
    {
        planets = new List<GameObject>(); //intialize planets
        getArrow = arrow.GetComponent<Arrow>();
	}

	void Update () {
        currentTouch = Input.mousePosition; //update mouse position

        if (Input.GetMouseButtonDown(0))
        {
                SpawnPlant(); //spawn planet on touch down
        }

        if (Input.GetMouseButton(0) && planetSpawnedNotStarted)
        {
            touchDisplace = new Vector2(currentTouch.x - touchStart.x, currentTouch.y - touchStart.y);
            getArrow.ScaleArrow(touchDisplace);
        }

        if (Input.GetMouseButtonUp(0) && planetSpawnedNotStarted)
        {
            StartPlanet(); //release (add forces to) planet on touch up ONLY if the touch down spawned a planet (rather than destroying a planet)
        }

	}

    void SpawnPlant(){ //spawns a planet
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //initialize ray
        RaycastHit hit;
        LayerMask layerMask = (1 << 9) | (1 << 10) | (1 << 5); //only shoot rays at layers 5, 9 & 10 (ui, spawn points and planets)

        if (Physics.Raycast(ray, out hit, layerMask)) //cast ray
        {
            if (hit.transform.gameObject.layer == 9) //if ray hits spawn layer
            {
                newPlanet = PlanetPooler.SharedInstance.GetPooledPlanet();  //local reference for planet
                if (newPlanet != null){
                    newPlanet.SetActive(true);
                    newPlanet.transform.position = hit.point;
                }

                touchStart = Input.mousePosition;

                arrow.SetActive(true); //display arrow
                arrow.GetComponent<Arrow>().Activate(touchStart);

                planetSpawnedNotStarted = true;
            }

            else if (hit.transform.gameObject.layer == 10 && hit.transform.gameObject != null) //if click hits a planet

            {
                hit.transform.gameObject.SetActive(false); //destroy that MF
            }
        }
    }

    void StartPlanet(){ //add forces to planet
        touchEnd = Input.mousePosition;
        Vector2 dragDirection = (touchStart - touchEnd).normalized;
        dragDistance = Mathf.Abs((touchStart - touchEnd).magnitude)/300;

        Planet planetAction = newPlanet.GetComponent<Planet>(); //access planet script from most recently added planet
        planetAction.AddOrbitalForce(dragDistance, dragDirection); //add some orbital force in proportion to mass/drag distance

        arrow.SetActive(false); //remove arrow

        planetSpawnedNotStarted = false; //tell script that we released the planet that we just spawned
    }


}
