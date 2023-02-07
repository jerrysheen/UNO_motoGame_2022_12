using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class LoadParallelBackground : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject camObj;
    private Camera cam;


    public GameObject[] midViewCanvas;
    public GameObject[] farViewCanvas;
    public GameObject[] backGroundViewCanvas;


    public Sprite[] midViewSprites;
    public Sprite[] farViewSprites;
    public Sprite[] backGroundViewSprites;

    [Header("Move Speed")] 
    [Space]
    public float midCanvasMoveSpeed = 3.0f;
    public float farCanvasMoveSpeed = 5.0f;    
    public float backGroundCanvasMoveSpeed = 5.0f;    

    [Space]
    [Header("Mount Point")] 
    [Space]
    public GameObject midMountPointGO;
    public GameObject farMountPointGO;
    public GameObject backGroundMountPointGO;
    
    

    [Space]
    [Header("Addressable Key Config")]
    [Space]
    public List<string> midViewSpritesKey;
    public List<string> farViewSpritesKey;
    public List<string> backGroundViewSpritesKey;
    public string canvasPrefabKey = "SingleBackGroundCanvas";

    AsyncOperationHandle<GameObject> opHandle;
    AsyncOperationHandle<Sprite> opSpriteHandle;

    /// <summary>
    /// 为什么这个地方要用ienumrator, 主要是为了初始化的时候能够开一个协程，等待载入，以及做出一些处理。
    /// Start函数没有这个功能。
    /// </summary>
    /// <returns></returns>
    public IEnumerator Start()
    {
        if (!camObj)
        {
            cam = Camera.current;
        }
        else
        {
            cam = camObj.GetComponent<Camera>();
        }

        // Load Canvas Prefab;
        midViewCanvas = new GameObject[3];
        farViewCanvas = new GameObject[3];
        backGroundViewCanvas = new GameObject[3];
        opHandle = Addressables.LoadAssetAsync<GameObject>(canvasPrefabKey);
        yield return opHandle;

        if (opHandle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject obj = opHandle.Result;
            //Instantiate(obj, transform);
            for (int i = 0; i < 3; i++)
            {
                midViewCanvas[i] = Instantiate(obj);
                farViewCanvas[i] = Instantiate(obj);
                backGroundViewCanvas[i] = Instantiate(obj);
            }
        }
        else
        {
            Debug.LogError("cant find ");
        }

        // Load Sprite Key:
        int tempCount;
        if (midViewSpritesKey == null || midViewSpritesKey.Count == 0)
        {
            tempCount = 0;
        }
        else
        {
            tempCount = midViewSpritesKey.Count;
        }

        midViewSprites = new Sprite[tempCount];
        for (int i = 0; i < tempCount; i++)
        {
            opSpriteHandle = Addressables.LoadAssetAsync<Sprite>(midViewSpritesKey[i]);
            yield return opSpriteHandle;

            if (opSpriteHandle.Status == AsyncOperationStatus.Succeeded)
            {
                midViewSprites[i] = opSpriteHandle.Result;
            }
            else
            {
                Debug.LogError("cant find" + opSpriteHandle.DebugName);
            }
        }

        // Assign random sprite to Canvas.
        for (int i = 0; i < 3; i++)
        {
            if (tempCount != 0)
            {
                int randNum = Random.Range(0,tempCount + 1);
                randNum = randNum == 4 ? 3 : randNum;
//                Debug.Log(randNum);
                midViewCanvas[i].GetComponent<SpriteRenderer>().sprite = midViewSprites[randNum];
            }
        }
        
        // set init place.   left / mid / right.

        float width = midViewCanvas[0].GetComponent<SpriteRenderer>().bounds.size.x / 2;
        if (!midMountPointGO)
        {
            Debug.LogError("Please assign mid mount point gameobj");
        }

        Vector3 midMountPoint = midMountPointGO.transform.position;
        midViewCanvas[0].transform.position = new Vector3(midMountPoint.x - 2 * width, midMountPoint.y, midMountPoint.z);
        midViewCanvas[1].transform.position = new Vector3(midMountPoint.x , midMountPoint.y, midMountPoint.z);
        midViewCanvas[2].transform.position = new Vector3(midMountPoint.x + 2 * width, midMountPoint.y, midMountPoint.z);


        // farview set:

        if (farViewSpritesKey == null || farViewSpritesKey.Count == 0)
        {
            tempCount = 0;
        }
        else
        {
            tempCount = farViewSpritesKey.Count;
        }

        farViewSprites = new Sprite[tempCount];
        for (int i = 0; i < tempCount; i++)
        {
            opSpriteHandle = Addressables.LoadAssetAsync<Sprite>(farViewSpritesKey[i]);
            yield return opSpriteHandle;

            if (opSpriteHandle.Status == AsyncOperationStatus.Succeeded)
            {
                farViewSprites[i] = opSpriteHandle.Result;
            }
            else
            {
                Debug.LogError("cant find" + opSpriteHandle.DebugName);
            }
        }

        // Assign random sprite to Canvas.
        for (int i = 0; i < 3; i++)
        {
            if (tempCount != 0)
            {
                int randNum = Random.Range(0, tempCount + 1);
                randNum = randNum == 4 ? 3 : randNum;
//                Debug.Log(randNum);
                farViewCanvas[i].GetComponent<SpriteRenderer>().sprite = farViewSprites[randNum];
            }
        }

        // set init place.   left / mid / right.

        width = farViewCanvas[0].GetComponent<SpriteRenderer>().bounds.size.x / 2;
        if (!farMountPointGO)
        {
            Debug.LogError("Please assign mid mount point gameobj");
        }

        Vector3 farMountPoint = farMountPointGO.transform.position;
        farViewCanvas[0].transform.position = new Vector3(farMountPoint.x - 2 * width, farMountPoint.y, farMountPoint.z);
        farViewCanvas[1].transform.position = new Vector3(farMountPoint.x, farMountPoint.y, farMountPoint.z);
        farViewCanvas[2].transform.position = new Vector3(farMountPoint.x + 2 * width, farMountPoint.y, farMountPoint.z);
        
        
        // backGroundview set:

        if (backGroundViewSpritesKey == null || backGroundViewSpritesKey.Count == 0)
        {
            tempCount = 0;
        }
        else
        {
            tempCount = backGroundViewSpritesKey.Count;
        }

        backGroundViewSprites = new Sprite[tempCount];
        for (int i = 0; i < tempCount; i++)
        {
            opSpriteHandle = Addressables.LoadAssetAsync<Sprite>(backGroundViewSpritesKey[i]);
            yield return opSpriteHandle;

            if (opSpriteHandle.Status == AsyncOperationStatus.Succeeded)
            {
                backGroundViewSprites[i] = opSpriteHandle.Result;
            }
            else
            {
                Debug.LogError("cant find" + opSpriteHandle.DebugName);
            }
        }

        // Assign random sprite to Canvas.
        for (int i = 0; i < 3; i++)
        {
            if (tempCount != 0)
            {
                int randNum = Random.Range(0, tempCount + 1);
                randNum = randNum == 4 ? 3 : randNum;
                Debug.Log(randNum);
                backGroundViewCanvas[i].GetComponent<SpriteRenderer>().sprite = backGroundViewSprites[randNum];
            }
        }

        // set init place.   left / mid / right.

        width = backGroundViewCanvas[0].GetComponent<SpriteRenderer>().bounds.size.x / 2;
        if (!backGroundMountPointGO)
        {
            Debug.LogError("Please assign mid mount point gameobj");
        }

        Vector3 backGroundMountPoint = backGroundMountPointGO.transform.position;
        backGroundViewCanvas[0].transform.position = new Vector3(backGroundMountPoint.x - 2 * width, backGroundMountPoint.y, backGroundMountPoint.z);
        backGroundViewCanvas[1].transform.position = new Vector3(backGroundMountPoint.x, backGroundMountPoint.y, backGroundMountPoint.z);
        backGroundViewCanvas[2].transform.position = new Vector3(backGroundMountPoint.x + 2 * width, backGroundMountPoint.y, backGroundMountPoint.z);
    }
    
    // void Start()
    // {
    //
    //
    // }

    // Update is called once per frame
    void Update()
    {

        
        // scrolling canvas here aloneside x axis, using mid point to decide whether curr canvas is out side of the screen .
        moveMidCanvas();
        // remove useless canvas, then active one inmediately..
        refreshCanvas();

    }
    
    void OnDestroy()
    {
        Addressables.Release(opHandle);
        Addressables.Release(opSpriteHandle);
    }

    /// <summary>
    /// the move direction will be right to left.
    /// </summary>
    void moveMidCanvas()
    {
        // mid canvas.
        for (int i = 0; i < midViewCanvas.Length; i++)
        {
            Vector3 tempPos = midViewCanvas[i].transform.position;
            midViewCanvas[i].transform.position = tempPos + Vector3.left * midCanvasMoveSpeed * Time.deltaTime;
        }

        // far canvas.
        for (int i = 0; i < farViewCanvas.Length; i++)
        {
            Vector3 tempPos = farViewCanvas[i].transform.position;
            farViewCanvas[i].transform.position = tempPos + Vector3.left * farCanvasMoveSpeed * Time.deltaTime;
        }
        
        for (int i = 0; i < backGroundViewCanvas.Length; i++)
        {
            Vector3 tempPos = backGroundViewCanvas[i].transform.position;
            backGroundViewCanvas[i].transform.position = tempPos + Vector3.left * backGroundCanvasMoveSpeed * Time.deltaTime;
        }
    }


    void refreshCanvas()
    {
        Camera camera = GameObject.Find("Main Camera")?.GetComponent<Camera>();
        Vector3 p0 = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        Vector3 p1 = camera.ViewportToWorldPoint(new Vector3(0, 1, camera.nearClipPlane));
        Vector3 p2 = camera.ViewportToWorldPoint(new Vector3(1, 0, camera.nearClipPlane));
        Vector3 p3 = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        float xMin = Mathf.Min(p0.x, p2.x);
        float xMax = Mathf.Max(p0.x, p2.x);
        
        // mid canvas:
        // [0] will always be the most left one.
        float width = midViewCanvas[0].GetComponent<SpriteRenderer>().bounds.size.x / 2;
        Vector3 tempPos = midViewCanvas[0].transform.position;
        if (tempPos.x + width < xMin)
        {
            // swap and move..
            GameObject temp = midViewCanvas[0];
            midViewCanvas[0] = midViewCanvas[1];
            midViewCanvas[1] = midViewCanvas[2];
            midViewCanvas[2] = temp;
            midViewCanvas[2].transform.position = midViewCanvas[1].transform.position + Vector3.right * 2.0f * width;
            
            // refresh canvas pic.
            int randNum = Random.Range(0, 100);
            randNum = randNum % midViewSpritesKey.Count;
//            Debug.Log(randNum);
            midViewCanvas[2].GetComponent<SpriteRenderer>().sprite = midViewSprites[randNum];
        }

        // far canvas:
        // [0] will always be the most left one.
        width = farViewCanvas[0].GetComponent<SpriteRenderer>().bounds.size.x / 2;
        tempPos = farViewCanvas[0].transform.position;
        if (tempPos.x + width < xMin)
        {
            // swap and move..
            GameObject temp = farViewCanvas[0];
            farViewCanvas[0] = farViewCanvas[1];
            farViewCanvas[1] = farViewCanvas[2];
            farViewCanvas[2] = temp;
            farViewCanvas[2].transform.position = farViewCanvas[1].transform.position + Vector3.right * 2.0f * width;

            // refresh canvas pic.
            int randNum = Random.Range(0, 100);
            randNum = randNum % farViewSpritesKey.Count;
            Debug.Log(randNum);
            farViewCanvas[2].GetComponent<SpriteRenderer>().sprite = farViewSprites[randNum];
        }
        
        // far canvas:
        // [0] will always be the most left one.
        width = backGroundViewCanvas[0].GetComponent<SpriteRenderer>().bounds.size.x / 2;
        tempPos = backGroundViewCanvas[0].transform.position;
        if (tempPos.x + width < xMin)
        {
            // swap and move..
            GameObject temp = backGroundViewCanvas[0];
            backGroundViewCanvas[0] = backGroundViewCanvas[1];
            backGroundViewCanvas[1] = backGroundViewCanvas[2];
            backGroundViewCanvas[2] = temp;
            backGroundViewCanvas[2].transform.position = backGroundViewCanvas[1].transform.position + Vector3.right * 2.0f * width;

            // refresh canvas pic.
            int randNum = Random.Range(0, 100);
            randNum = randNum % backGroundViewSpritesKey.Count;
            Debug.Log(randNum);
            backGroundViewCanvas[2].GetComponent<SpriteRenderer>().sprite = backGroundViewSprites[randNum];
        }

    }





#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Camera camera = GameObject.Find("Main Camera")?.GetComponent<Camera>();
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
       // Debug.Log(p3.x +" , " + p3.y + " , " +p3.z);
        
    }
    #endif
}
