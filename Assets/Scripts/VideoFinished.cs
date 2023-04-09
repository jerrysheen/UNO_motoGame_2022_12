using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VideoFinished : MonoBehaviour
{
    // Start is called before the first frame update

    public string nextSceneName = "CodeTest 1";
    public float jumpDelayTime = 11.0f;
    void Start()
    {
        StartCoroutine(JumpToNextScene(jumpDelayTime));
    }


    IEnumerator JumpToNextScene(float delayTime) 
    {
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene(nextSceneName);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
