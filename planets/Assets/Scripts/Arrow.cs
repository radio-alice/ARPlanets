using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
    
    public void Activate(Vector2 touchStart){
        GetComponent<RectTransform>().position = touchStart;//move arrow to touch position
    }

    public void ScaleArrow(Vector2 touchDisplace, Transform planetTransform)
    {
        transform.position = planetTransform.position;

        transform.localScale = Vector3.one * touchDisplace.magnitude/200; //scale and rotate arrow by touch displacement
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(touchDisplace.x, -touchDisplace.y) * Mathf.Rad2Deg));
    }
}
