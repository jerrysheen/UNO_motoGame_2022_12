using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToNextScene : MonoBehaviour
{
    public string NextSceneName;

    public Animator transition;

    public float transitionTime = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            //SceneManager.LoadScene(NextSceneName);
            LoadNextLevel(NextSceneName);
        }
    }

    public void LoadNextLevel(string NextSceneName)
    {
        StartCoroutine(LoadLevel(NextSceneName));
    }

    IEnumerator LoadLevel(string name)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(name);
    }
}
