using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneMenu : MonoBehaviour
{
    GameObject menuPanel;
    GameObject bagPanel;
    GameObject completed;
    Button continueBtn;
    Button restartBtn;
    Button quitBtn;
    bool bagActive;
    bool gameSceneActive;
    Animator anim;
    private void Awake()
    {
        completed = transform.GetChild(0).gameObject;
        bagPanel = transform.GetChild(1).gameObject;
        menuPanel = transform.GetChild(2).gameObject;
        anim = completed.GetComponent<Animator>();
        continueBtn = menuPanel.transform.GetChild(1).GetComponent<Button>();
        restartBtn = menuPanel.transform.GetChild(2).GetComponent<Button>();
        quitBtn = menuPanel.transform.GetChild(3).GetComponent<Button>();
    }

    private void Start()
    {
        continueBtn.onClick.AddListener(ContinueGame);
        restartBtn.onClick.AddListener(ReStartGame);
        quitBtn.onClick.AddListener(QuitGame);
    }
    private void Update()
    {
        if (!bagActive)
        {
            OpenGameScene();
        }
        if (!gameSceneActive)
        {
             OpenBag();
        }
    }
   public void CompletedBattle()
    {
        completed.SetActive(true);
        anim.SetTrigger("completed");
        StartCoroutine(CloseCompleted());
    }
    void ContinueGame()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1;
        KeyBoardInput.mouseEnable = true;
    }

    void ReStartGame()
    {

    }

    void QuitGame()
    {
        Time.timeScale = 1;
        KeyBoardInput.mouseEnable = false;
        SceneController.Instance.TransitionToMain();
    }

    void OpenGameScene()
    {
        gameSceneActive = menuPanel.active;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameSceneActive = !gameSceneActive;
            menuPanel.SetActive(gameSceneActive);
            Time.timeScale = gameSceneActive ? 0 : 1;
            KeyBoardInput.mouseEnable = gameSceneActive ? false : true;
        }
        
    }
    void OpenBag()
    {
        bagActive = bagPanel.active;
        if (Input.GetKeyDown(KeyCode.B))
        {
            bagActive = !bagActive;
            //´ò¿ªÊ±ÔÝÍ£
            bagPanel.SetActive(bagActive);
            Time.timeScale = bagActive ? 0 : 1;
            KeyBoardInput.mouseEnable = bagActive ? false : true;
        }
        
    }
    public void CloseBag()
    {
        bagActive = false;
        bagPanel.SetActive(bagActive);
        Time.timeScale = 1;
        KeyBoardInput.mouseEnable =true;
    }

    IEnumerator CloseCompleted()
    {
        yield return new WaitForSeconds(3f);
        completed.SetActive(false);
    }
}
