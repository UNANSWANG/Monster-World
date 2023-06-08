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

        //��Ӽ���
        newBtn.onClick.AddListener(PlayTimeLine);
        continueBtn.onClick.AddListener(ContinueGame);
        quitBtn.onClick.AddListener(QuitGame);
        //todo ���������Ӻ���
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
        //ת������
        SceneController.Instance.TransitionFirstLevel();
    }
    
    public void ContinueGame()
    {
        if (SaveManager.Instance.SceneName!="")
        {
            //ת��������ȡ����
            SceneController.Instance.TransitionToLoadGame();
            KeyBoardInput.mouseEnable = true;
        }
        else
        {
            Debug.Log("û�п�ʼ����Ϸ");
        }
    }
    public void QuitGame()
    {
        Debug.Log("�˳�");
        Application.Quit();
    }
}
