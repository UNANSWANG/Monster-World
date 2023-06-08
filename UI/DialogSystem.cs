using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [Header("== UI组件 ==")]
    public Text textLabel;
    public Image faceImage;
    [Header("== 文本文件 ==")]
    public TextAsset textFile;
    [Header("== 头像 ==")]
    public Sprite npcIm;
    public Sprite playerIm;
    [Header("== 基础设置 ==")]
    public int index;
    public float textSpeed;
    List<string> textList = new List<string>();
    bool textFinishi;
    bool outFlag;

    private void Awake()
    {
        GetTextFormFile(textFile);
    }

    private void OnEnable()
    {
        //直接输出
        //textLabel.text = textList[index++];
        //协程输出
        //textFinishi = true;
        StartCoroutine(SetTextUI());
    }

    private void Update()
    {
        if (outFlag&&Input.GetKeyDown(KeyCode.F))
        {
            outFlag = false;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            if (index == textList.Count)
            {
                gameObject.SetActive(false);
                index = 0;
                return;
            }
            if (textFinishi)
            {
                //直接输出
                //textLabel.text = textList[index++];
                //协程输出
                StartCoroutine(SetTextUI());
            }
        }
        
    }

    void GetTextFormFile(TextAsset file)
    {
        textList.Clear();
        index = 0;
        //以换行为分隔符来读取文件
        var LineData = file.text.Split('\n');

        foreach (var line in LineData)
        {
            textList.Add(line);
        }
    }

    IEnumerator SetTextUI()
    {
        textLabel.text = "";
        textFinishi = false;
        outFlag = true;
        switch (textList[index].Trim().ToString())
        {
            case "NPC":
                faceImage.sprite = npcIm;
                index++;
                break;
            case "玩家":
                faceImage.sprite = playerIm;
                index++;
                break;
            default:
                break;
        }
        for (int i = 0; i < textList[index].Length; i++)
        {
            textLabel.text += textList[index][i];
            if (!outFlag)
            {
                textLabel.text = textList[index];
                break;
            }
            yield return new WaitForSeconds(textSpeed);
        }
        textFinishi = true;
        index++;
    }
}
