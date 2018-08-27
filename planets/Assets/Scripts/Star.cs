using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Light))]
public class Star : MonoBehaviour
{
    public bool starEnabled = false; //used to tell other scripts whether star is anchored and rendered
    public Pose starPose; //used to tell the planets where star center is

    Rigidbody getRigidbody; //yup
    MeshRenderer mRenderer; //get renderer (so we can enable and disable it)
    Material starMat; //material attached to star (so we can make sure light is same color)
    Anchor anchor; //anchors star in world
    DetectedPlane detectedPlane; //plane detected by ARCore
    float yOffset; //offset of star from plane
    GameObject spawnPlane; //plane for spawning planets

    void Awake()
    {
        getRigidbody = GetComponent<Rigidbody>(); //initialize rigidbody, mass, renderer, material, light color, freeze the position of star, set raycast layer
        getRigidbody.constraints = RigidbodyConstraints.FreezeAll;

        mRenderer = GetComponent<MeshRenderer>();
        starMat = mRenderer.material;
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
        //mRenderer.enabled = false; //disable rendering until it's placed and anchored
    }

	//void Update()
	//{

 //       //Google ARCore tutorial code for plane detection
 //       // The tracking state must be FrameTrackingState.Tracking
 //       // in order to access the Frame.
 //       if (Session.Status != SessionStatus.Tracking) return;

 //       if (detectedPlane == null) return;

 //       // Check for the plane being subsumed.
 //       // If the plane has been subsumed switch attachment to the subsuming plane.
 //       while (detectedPlane.SubsumedBy != null)
 //       {
 //           detectedPlane = detectedPlane.SubsumedBy;
 //       }

 //       // Move the position to stay in the same place relative to plane
 //       transform.position = new Vector3(transform.position.x,
 //                   detectedPlane.CenterPose.position.y + yOffset, transform.position.z);

 //       spawnPlane.transform.LookAt(Camera.main.transform, Vector3.up); //ensure collider  always faces camera
	//}
	
    //public void SetSelectedPlane(DetectedPlane detectedPlane)
    //{
    //    this.detectedPlane = detectedPlane; //select plane
    //    CreateAnchor(); //set anchor
    //}

    //void CreateAnchor()
    //{
    //    // Create the position of the anchor by raycasting a point towards
    //    // the top of the screen.
    //    Vector2 pos = new Vector2(Screen.width * .5f, Screen.height * .90f);
    //    Ray ray = Camera.main.ScreenPointToRay(pos);
    //    Vector3 anchorPosition = ray.GetPoint(5f) + (Vector3.up * .7f); //spawns star above plane, not on it

    //    // Create the anchor at that point.
    //    if (anchor != null)
    //    {
    //        Destroy(anchor);
    //    }

    //    starPose = new Pose(anchorPosition, Quaternion.identity);
    //    anchor = detectedPlane.CreateAnchor(starPose);

    //    // Attach the star to the anchor.
    //    transform.position = anchorPosition;
    //    transform.SetParent(anchor.transform);

    //    // Record the y offset from the plane.
    //    yOffset = transform.position.y - detectedPlane.CenterPose.position.y;

    //    mRenderer.enabled = true; // enable the renderer
    //    starEnabled = true; //tell other scripts that star has been anchored
    //}
}
