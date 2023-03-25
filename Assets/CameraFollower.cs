using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public GameObject target;
    public bool followPos = false;
    public bool followSize = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (followPos) 
        {
            this.transform.position = target.transform.position; 
        }

        if (followSize) 
        {
            this.GetComponent<Camera>().orthographicSize = target.GetComponent<Camera>().orthographicSize;
        }
    }
}
