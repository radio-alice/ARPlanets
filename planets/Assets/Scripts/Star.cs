using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Light))]
public class Star : MonoBehaviour
{
    public bool starEnabled = false; //used to tell other scripts whether star is anchored and rendered

    Rigidbody getRigidbody; //yup
    MeshRenderer mRenderer; //get renderer (so we can enable and disable it)
    Material starMat; //material attached to star (so we can make sure light is same color)

    GameObject spawnPlane; //plane for spawning planets

    void Awake()
    {
        getRigidbody = GetComponent<Rigidbody>(); //initialize rigidbody, mass, renderer, material, light color, freeze the position of star, set raycast layer
        getRigidbody.constraints = RigidbodyConstraints.FreezeAll;

        starMat = GetComponent<MeshRenderer>().material;
        //if (starMat.color != null) GetComponent<Light>().color = starMat.color;

        gameObject.layer = 9; //set star layer to same as spawn plane

        spawnPlane = new GameObject(); //create empty transform for raycast collider for planet spawning
        spawnPlane.transform.position = transform.position; //set position = star position
        spawnPlane.transform.parent = transform; //set parent = star
        spawnPlane.gameObject.layer = 9; //set layer to planet spawn raycast layer
        BoxCollider sPCollider = spawnPlane.AddComponent<BoxCollider>(); //add a collider
        sPCollider.isTrigger = true;  //make it a trigger
        sPCollider.size = new Vector3(10, 10, 0); //make the collider a (big) plane intersecting the star and facing the camera
    }

    void Start()
    {
        gameObject.tag = "Star"; //make sure this mf is tagged as a star
    }
}
