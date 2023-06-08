using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : SingleTon<SceneController>, IEndGameObserve
{
    public SceneFader sceneFaderPrefab;
    public GameObject playerPrefab;
    bool fadeFinished;
    GameObject player;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        GameManager.Instance.AddObserve(this);
        fadeFinished = true;
    }
    
    public void TransitonToDestination(TransitonPoint transitonPoint)
    {
        switch (transitonPoint.transitionType)
        {
            case TransitonPoint.TransitionType.SameScene:
                StartCoroutine(Transition(SceneManager.GetActiveScene().name,transitonPoint.destinationTag));
                break;
            case TransitonPoint.TransitionType.DifferentScene:
                StartCoroutine(Transition(transitonPoint.sceneName,transitonPoint.destinationTag));
                break;
            default:
                break;
        }
    }
    public void TransitionFirstLevel()
    {
        StartCoroutine(LoadLevel("MonsterWorld"));
    }

    public void TransitionToLoadGame()
    {
        //���ص�֮ǰ�˳�ʱ����ĳ���ȥ
        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));
    }

    public void TransitionToMain()
    {
        StartCoroutine(LoadMain());
        //��������
        SaveManager.Instance.SavePlayerData();
    }
    IEnumerator Transition(string sceneName, TransitonDestination.DestinationTag destinationTag)
    {
        //��������
        SaveManager.Instance.SavePlayerData();
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            //todo ���Լӵ���Ч

            yield return SceneManager.LoadSceneAsync(sceneName);
            //����ļ���
            yield return Instantiate(playerPrefab, GetTransionDestination(destinationTag).transform.position, GetTransionDestination(destinationTag).transform.rotation);
            //��ȡ����
            SaveManager.Instance.LoadPlayerData();
            yield break;
        }
        else
        {
            player = GameManager.Instance.playerStats.gameObject;
            player.transform.SetPositionAndRotation(GetTransionDestination(destinationTag).transform.position,GetTransionDestination(destinationTag).transform.rotation);
            yield return null;
        }
    }
    IEnumerator LoadLevel(string sceneName)
    {
        SceneFader fader = Instantiate(sceneFaderPrefab);
        if (sceneName!="")    
        {
            yield return StartCoroutine(fader.FadeOut(1f));
            //yield return SceneManager.LoadSceneAsync(sceneName);
            yield return StartCoroutine(fader.LoadScene(sceneName));
            yield return player = Instantiate(playerPrefab,GameManager.Instance.GetEntrence().position,GameManager.Instance.GetEntrence().rotation);

            yield return StartCoroutine(fader.FadeIn(1f));
            //��������
            SaveManager.Instance.SavePlayerData();
            yield break;
        }
    }

    IEnumerator LoadMain()
    {
        SceneFader fader = Instantiate(sceneFaderPrefab);
        yield return StartCoroutine(fader.FadeOut(1f));
        yield return SceneManager.LoadSceneAsync("Main");
        yield return StartCoroutine(fader.FadeIn(1f));
        yield break;
    }
    private TransitonDestination GetTransionDestination(TransitonDestination.DestinationTag destinationTag)
    {
        var destinations = FindObjectsOfType<TransitonDestination>();
        foreach (TransitonDestination item in destinations)
        {
            if (item.destinationTag == destinationTag)
            {
                return item;
            }
        }
        return null;
    }
    public void EndNotify()
    {
        if (fadeFinished)
        {
            fadeFinished = false;
            //StartCoroutine(LoadMain());
        }
    }
}
