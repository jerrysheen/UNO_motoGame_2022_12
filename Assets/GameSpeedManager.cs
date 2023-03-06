using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpeedManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject platform;
    public GameObject cutScene;
    public float moveSpeed = 8.0f;
    void Start()
    {
        if (!platform)
        {
            Debug.LogError("Please assign platform");
        }
    }

    // Update is called once per frame
    void Update()
    {
        platform.transform.position = platform.transform.position + Vector3.left * Time.deltaTime * moveSpeed;
    }
}
