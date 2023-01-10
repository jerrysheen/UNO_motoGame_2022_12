using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadParallelBackground : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject camObj;
    private Camera cam;

    public List<GameObject> midViewObj; 
    public List<GameObject> farViewObj; 
    public List<GameObject> closeViewObj; 
    void Start()
    {
        if (!camObj)
        {
            cam = Camera.current;
        }
        else
        {
            cam = camObj.GetComponent<Camera>();
        }
        
        // 开始的时候， 在相机的位置初始化一些gameobj
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
