using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    Button newBtn;
    Button continueBtn;
    Button quitBtn;
    PlayableDirector pd;

    private void Awake()
    {
        newBtn = transform.GetChild(1).GetComponent<Button>();
        continueBtn = transform.GetChild(2).GetComponent<Button>();
        quitBtn = transform.GetChild(3).GetComponent<Button>();

        //添加监听
        newBtn.onClick.AddListener(PlayTimeLine);
        continueBtn.onClick.AddListener(ContinueGame);
        quitBtn.onClick.AddListener(QuitGame);
        //todo 导演类增加函数
        pd = FindObjectOfType<PlayableDirector>();
        pd.stopped += NewGame;
    }
    void PlayTimeLine()
    {
        pd.Play();
    }
    void NewGame(PlayableDirector director)
    {
        PlayerPrefs.DeleteAll();
        //转换场景
        SceneController.Instance.TransitionFirstLevel();
    }
    
    public void ContinueGame()
    {
        if (SaveManager.Instance.SceneName!="")
        {
            //转换场景读取进度
            SceneController.Instance.TransitionToLoadGame();
            KeyBoardInput.mouseEnable = true;
        }
        else
        {
            Debug.Log("没有开始过游戏");
        }
    }
    public void QuitGame()
    {
        Debug.Log("退出");
        Application.Quit();
    }
}
