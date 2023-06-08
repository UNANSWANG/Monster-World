using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public float fadeInDuration;
    public float fadeOutDuration;
    CanvasGroup canvasGroup;
    Text load;
   /* GameObject loadPanel;
    Slider slider;
    Text loadPercent;*/

    public void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        load = transform.GetChild(1).GetComponent<Text>();
        /*loadPanel = transform.GetChild(1).gameObject;
        slider = loadPanel.transform.GetChild(0).GetComponent<Slider>();
        loadPercent = loadPanel.transform.GetChild(1).GetComponent<Text>();*/
        DontDestroyOnLoad(this);
    }
    public IEnumerator FadeInOut()
    {
        yield return FadeIn(fadeInDuration);
        yield return FadeOut(fadeOutDuration);
    }

    public IEnumerator FadeOut(float time)
    {
        while (canvasGroup.alpha<1)
        {
            canvasGroup.alpha += Time.deltaTime / time;
            yield return null;
        }
    }
    public IEnumerator FadeIn(float time)
    {
        while (canvasGroup.alpha!=0)
        {
            canvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }
        Destroy(gameObject);
    }

    public IEnumerator LoadScene(string sceneName)
    {
        //异步场景滑动条
        /*loadPanel.SetActive(true);
        AsyncOperation opration = SceneManager.LoadSceneAsync(sceneName);
        while (!opration.isDone)
        {
            slider.value = opration.progress;
            loadPercent.text = opration.progress * 100 + " %";
            if (opration.progress>=0.8)
            {
                slider.value = 1;
                loadPercent.text = "100 %";
            }
            yield return null;
        }*/
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            if (load.text== "Game Load...")
            {
                load.text = "Game Load";
            }
            else
            {
                yield return new WaitForSeconds(0.2f);
                load.text += ".";
            }
            yield return null;
        }
    }
}
