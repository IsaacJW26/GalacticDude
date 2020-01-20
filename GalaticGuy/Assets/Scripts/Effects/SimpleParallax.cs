using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleParallax : MonoBehaviour
{
    Vector3 startPos;
    [SerializeField]
    Transform cam;
    [SerializeField]
    [Range(0f, 1f)]
    float parallaxPercent;
    Vector3 zpos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = new Vector3(transform.position.x, transform.position.y);
        zpos = Vector3.forward * transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        var campos = new Vector3(cam.position.x, cam.position.y)+startPos;
        transform.position = startPos * (1f - parallaxPercent) + (campos * parallaxPercent) + zpos;
    }
}
