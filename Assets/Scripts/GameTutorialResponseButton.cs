using Manager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTutorialResponseButton : MonoBehaviour
{
    // ����ط�Ϊ��ʱ�䷽����д��
    // Start is called before the first frame update
    string buttonA00 = "֪��";
    string buttonB00 = "��Ȼ��";
    string buttonA01 = "����";
    string buttonB01 = "Ruby������";

    TextMeshProUGUI AnswerText1;
    TextMeshProUGUI AnswerText0;
    Animator currAnimator;


    void Start()
    {
        GameManager.getInstance.OnStoryLineChange += _OnStroyLineChange;
        AnswerText1 = GameObject.Find("AnswerText1")?.GetComponent<TextMeshProUGUI>();
        AnswerText0 =  GameObject.Find("AnswerText0")?.GetComponent<TextMeshProUGUI>();
        if (AnswerText0 == null || AnswerText1 == null) 
        {
            Debug.LogError("AnswerText component not find");
        }

        currAnimator = GetComponent<Animator>();


    }


    private void OnDestroy()
    {
        GameManager.getInstance.OnStoryLineChange -= _OnStroyLineChange;
    }


    void _OnStroyLineChange(string name) 
    {
        Debug.Log(name);
        switch (name)
        {
            // "֪��"; "��Ȼ��";
            case "CutScene00":
                AnswerText0.text = buttonA00;
                AnswerText1.text = buttonA01;
                currAnimator.SetTrigger("StartDialogue");
                break;

            //"����";  "Ruby������";
            case "CutScene02":
                AnswerText0.text = buttonA01;
                AnswerText1.text = buttonA01;
                currAnimator.SetTrigger("DialogueOver");
                break;

                break;
            default:
                break;
        }



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
