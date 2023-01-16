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
        Debug.Log("test");

    }
    
    #if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Camera camera = GetComponent<Camera>();
        Vector3 p0 = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        Vector3 p1 = camera.ViewportToWorldPoint(new Vector3(0, 1, camera.nearClipPlane));
        Vector3 p2 = camera.ViewportToWorldPoint(new Vector3(1, 0, camera.nearClipPlane));
        Vector3 p3 = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(p0, 1F);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(p1, 1F);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(p2, 1F);
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(p3, 1F);
        Debug.Log(p3.x +" , " + p3.y + " , " +p3.z);
        
    }
    #endif
}
