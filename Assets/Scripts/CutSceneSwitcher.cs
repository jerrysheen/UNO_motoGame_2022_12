using System.Collections;
using System.Collections.Generic;using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
public class CutSceneSwitcher : MonoBehaviour
{
    public enum Story_Process 
    {
        CutScene00 = 0,
        Mission_CollectCoin = 1,
        CutScene01 = 2,
        Mission_DodgeMine = 3,
        CutScene02 = 4,
        Normal_Game = 5
    }

    public GameObject collectCoin_Platform;
    public GameObject dodgeMine_Platform;
    public GameObject normalGame_Platform;

    [Space] 
    [Header("Cut Scene")] 
    public GameObject[] cutSceneCanvas;
    public Sprite[] cutSceneSprites;
    public bool isCutScene = false;
    public float cutSceneCanvasMoveSpeed = 10.0f;
    private bool isSwitchingScene = false;
    private bool lastIsCutScene = false;
    public List<string> cutSceneSpritesKey;
    public GameObject cutSceneMountPointGO;
    public string canvasPrefabKey = "SingleBackGroundCanvas";
    public Story_Process currProcess;    
    
    AsyncOperationHandle<GameObject> opHandle;
    AsyncOperationHandle<Sprite> opSpriteHandle;
    /// <summary>
    /// 为什么这个地方要用ienumrator, 主要是为了初始化的时候能够开一个协程，等待载入，以及做出一些处理。
    /// Start函数没有这个功能。
    /// </summary>
    /// <returns></returns>
    public IEnumerator Start()
    {
        // if (!caObj)
        // {
        //     cam = Camera.current;
        // }
        // else
        // {
        //     cam = camObj.GetComponent<Camera>();
        // }
        cutSceneCanvas = new GameObject[3];
        opHandle = Addressables.LoadAssetAsync<GameObject>(canvasPrefabKey);
        yield return opHandle;

        if (opHandle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject obj = opHandle.Result;
            var oldOrder = obj.GetComponent<SpriteRenderer>().sortingOrder; 
            //Instantiate(obj, transform);
            for (int i = 0; i < 3; i++)
            {
                cutSceneCanvas[i] = Instantiate(obj);
                cutSceneCanvas[i].name = "cutSceneCanvas";
                cutSceneCanvas[i].GetComponent<SpriteRenderer>().sortingOrder = oldOrder + 10;
                int defaultLayer = LayerMask.NameToLayer("Default");
                cutSceneCanvas[i].layer = defaultLayer;
            }
        }
        else
        {
            Debug.LogError("cant find ");
        }

        #region Load Cut Scene OR Begin Play

        if (isCutScene)
        {
            collectCoin_Platform.SetActive(false);
            dodgeMine_Platform.SetActive(false);
            normalGame_Platform.SetActive(false);
            // Load Sprite Key:
            int tempCount;
            if (cutSceneSpritesKey == null || cutSceneSpritesKey.Count == 0)
            {
                tempCount = 0;
            }
            else
            {
                tempCount = cutSceneSpritesKey.Count;
            }

            cutSceneSprites = new Sprite[tempCount];
            for (int i = 0; i < tempCount; i++)
            {
                opSpriteHandle = Addressables.LoadAssetAsync<Sprite>(cutSceneSpritesKey[i]);
                yield return opSpriteHandle;

                if (opSpriteHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    cutSceneSprites[i] = opSpriteHandle.Result;
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
                    cutSceneCanvas[i].GetComponent<SpriteRenderer>().sprite = cutSceneSprites[randNum];
                }
            }
        
            // set init place.   left / mid / right.

            float width = cutSceneCanvas[0].GetComponent<SpriteRenderer>().bounds.size.x / 2;
            if (cutSceneMountPointGO == null)
            {
                Debug.LogError("Please assign cutSceneMountPointGO mount point gameobj");
            }

            var cutSceneMountPoint = cutSceneMountPointGO.transform.position;
            cutSceneCanvas[0].transform.position = new Vector3(cutSceneMountPoint.x - 2 * width, cutSceneMountPoint.y, cutSceneMountPoint.z);
            cutSceneCanvas[1].transform.position = new Vector3(cutSceneMountPoint.x , cutSceneMountPoint.y, cutSceneMountPoint.z);
            cutSceneCanvas[2].transform.position = new Vector3(cutSceneMountPoint.x + 2 * width, cutSceneMountPoint.y, cutSceneMountPoint.z);

            currProcess = Story_Process.CutScene00;
            lastIsCutScene = isCutScene;
        }

        isSwitchingScene = false;

        #endregion
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
        ScrollMidCanvas();
        
        SwitchCutScene();
        // remove useless canvas, then active one inmediately..
        refreshCanvas();

    }

    /// <summary>
    /// 切换的时候肯定分为两种，
    /// 1. 从cut -> 非cut， 这个简单，就是载入platform，以匀速运行。
    /// 2. 从非cut-> cut。应该是从非cut的最后一个child，在屏幕中，开始加载后面的几个。
    /// </summary>
    public void SwitchCutScene()
    {
        if (lastIsCutScene != isCutScene || isSwitchingScene)
        {
            // this means we are first frame switch our cutScnee;
            if (lastIsCutScene != isCutScene)
            {
                if (isCutScene)
                {
                    
                }
                else
                {
                    // 这里非常简单， 如果不是cutScene， 那么只需要把Platform的头接在这边就好了。
                    switch (currProcess)
                    {
                        case Story_Process.CutScene00:
                            
                            currProcess = Story_Process.Mission_CollectCoin;
                            break;
                        case Story_Process.CutScene01:
                            currProcess = Story_Process.Mission_DodgeMine;
                            break;
                        case Story_Process.CutScene02:
                            currProcess = Story_Process.Normal_Game;
                            break;
                    }
                }
            }
            isSwitchingScene = true;
            lastIsCutScene = isCutScene;
        }
    }

    void OnDestroy()
    {
        Addressables.Release(opHandle);
        Addressables.Release(opSpriteHandle);
    }

    /// <summary>
    /// the move direction will be right to left.
    /// </summary>
    void ScrollMidCanvas()
    {
        // mid canvas.

        if (isCutScene)
        {
            for (int i = 0; i < cutSceneCanvas.Length; i++)
            {
                // in case the canvas is on loading
                if (cutSceneCanvas[i] == null) continue;
                Vector3 tempPos = cutSceneCanvas[i].transform.position;
                cutSceneCanvas[i].transform.position = tempPos + Vector3.left * cutSceneCanvasMoveSpeed * Time.deltaTime;
                Debug.Log((Vector3.left * cutSceneCanvasMoveSpeed * Time.deltaTime).x);
            }
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
        float width = cutSceneCanvas[0].GetComponent<SpriteRenderer>().bounds.size.x / 2;
        Vector3 tempPos = cutSceneCanvas[0].transform.position;
        if (isCutScene )
        {
            if (tempPos.x + width < xMin)
            {
                // swap and move..
                GameObject temp = cutSceneCanvas[0];
                cutSceneCanvas[0] = cutSceneCanvas[1];
                cutSceneCanvas[1] = cutSceneCanvas[2];
                cutSceneCanvas[2] = temp;
                cutSceneCanvas[2].transform.position = cutSceneCanvas[1].transform.position + Vector3.right * 2.0f * width;
            
                // refresh canvas pic.
                int randNum = Random.Range(0, 100);
                randNum = randNum % cutSceneSpritesKey.Count;
//            Debug.Log(randNum);
                cutSceneCanvas[2].GetComponent<SpriteRenderer>().sprite = cutSceneSprites[randNum];
            }

        }

    }

    public void GoToNextProcess()
    {
        currProcess = (Story_Process) ((int) currProcess + 1);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CutSceneSwitcher))]
public class CutSceneSwitcherEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Goto Next"))
        {
            var Script = target as CutSceneSwitcher;
            Script.GoToNextProcess();
        }
    }
}
#endif
