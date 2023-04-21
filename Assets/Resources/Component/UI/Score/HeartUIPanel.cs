using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using TMPro;
using UI;
using UnityEngine;

public class HeartUIPanel : MonoBehaviour,IHomeUI
{
    private TextMeshProUGUI currText;

    private int currScore;


    // Start is called before the first frame update
    void Start()
    {
        if (UIManager.getInstance != null)
        {
            UIManager.getInstance.AddUI(this);
        }
        GameManager.getInstance.OnScoreValueChange += OnScoreChange;
        currText = GetComponent<TextMeshProUGUI>();
        currText.text = 4.ToString();
        currScore = 4;
    }
    
    private void OnDestroy()
    {
        if (UIManager.getInstance != null)
        {
            UIManager.getInstance.RemoveUI(this);
            if (GameManager.getInstance ) 
            {
                GameManager.getInstance.OnScoreValueChange -= OnScoreChange;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnScoreChange(int score)
    {
        // mo ren ruguo dayu 0, ze buhui shi xueliang 
        if (score > 0) return;
        currScore += score;
        currText.text = currScore.ToString();
    }

    public void OnOpen(params object[] datas)
    {
        Show(datas);
    }

    public void OnClose()
    {
        Hide();
    }
    
    private void Show(params object[] datas)
    {
        this.gameObject.SetActive(true);
    }

    private void Hide()
    {
        GameManager.getInstance.OnScoreValueChange -= OnScoreChange;
        this.gameObject.SetActive(false);
    }
}
