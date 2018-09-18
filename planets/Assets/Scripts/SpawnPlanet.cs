using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlanet : MonoBehaviour {
    
    public GameObject arrow; //arrow to be displayed
    public GameObject planet; //planet prefab

    bool planetSpawnedNotStarted = false; //tells whether a planet has been spawned (on touch down), but not released (touch up)
    GameObject newPlanet; //most recently spawned planet
    
    Arrow getArrow; //arrow script from object
    Vector2 touchStart; //variables to store touch position to calculate launch vector
    Vector2 touchEnd;
    Vector2 currentTouch;
    Vector2 touchDisplace;
    float dragDistance; //how far finger dragged

    void Start()
    {
        getArrow = arrow.GetComponent<Arrow>(); //get arrow script
	}

	void Update () {
        if (!Pause.paused && Star.starEnabled && Input.touchCount > 0) //only get input if game is not paused & star is enabled (& there is input to get)
        {
            currentTouch = Input.GetTouch(0).position; //update touch position

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                SpawnPlant(); //spawn planet on touch down
            }

            if (planetSpawnedNotStarted)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)
                {
                    touchDisplace = new Vector2(currentTouch.x - touchStart.x, currentTouch.y - touchStart.y); //get touch vector
                    getArrow.ScaleArrow(touchDisplace, newPlanet.transform); //scale arrow by touch vector
                }

                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    StartPlanet(); //release (add forces to) planet on touch up 
                }
            }
        }

	}

    void SpawnPlant(){ //spawns a planet
        Ray ray = Camera.main.ScreenPointToRay(currentTouch); //initialize ray
        RaycastHit hit;
        LayerMask layerMask = (1 << 9) | (1 << 10) | (1 << 5); //only shoot rays at layers 5, 9, 10 (ui, spawn plane and planets)

        if (Physics.Raycast(ray, out hit, layerMask)) //cast ray
        {
            if (hit.transform.gameObject.layer == 9) //if ray hits spawn layer
            {
                newPlanet = PlanetPooler.SharedInstance.GetPooledPlanet();  //local reference for planet

                if (newPlanet != null){
                    newPlanet.SetActive(true);
                    newPlanet.transform.position = hit.point;
                }

                touchStart = currentTouch; //set touch start point as current touch point

                arrow.SetActive(true); //display arrow
                getArrow.Activate(touchStart); //start arrow

                planetSpawnedNotStarted = true;
            }

            else if (hit.transform.gameObject.layer == 10 && hit.transform.gameObject != null) //if click hits a planet

            {
                hit.transform.gameObject.SetActive(false); //destroy that MF
            }
        }
    }

    void StartPlanet(){ //add forces to planet
        touchEnd = currentTouch;
        Vector2 dragDirection = (touchStart - touchEnd).normalized;
        dragDistance = Mathf.Abs((touchStart - touchEnd).magnitude)/300;

        Planet planetAction = newPlanet.GetComponent<Planet>(); //access planet script from most recently added planet
        planetAction.AddOrbitalForce(dragDistance, dragDirection); //add some orbital force in proportion to mass/drag distance

        arrow.SetActive(false); //remove arrow

        planetSpawnedNotStarted = false; //tell script that we released the planet that we just spawned
    }


}
