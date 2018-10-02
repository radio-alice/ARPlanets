using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for arrow movement
/// </summary>

public class Arrow : MonoBehaviour {

    public float scalar;

    public void Activate(Vector2 touchStart)
    {
        transform.position = touchStart;//move arrow to touch position
    }

    public void ScaleArrow(Vector2 touchDisplace, Transform planetTransform) 
    {
        transform.position = planetTransform.position;

        transform.localScale = Vector3.one * touchDisplace.magnitude/scalar; //scale and rotate arrow by touch displacement
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(touchDisplace.x, -touchDisplace.y) * Mathf.Rad2Deg));
    }
}
