using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [Header("== UI��� ==")]
    public Text textLabel;
    public Image faceImage;
    [Header("== �ı��ļ� ==")]
    public TextAsset textFile;
    [Header("== ͷ�� ==")]
    public Sprite npcIm;
    public Sprite playerIm;
    [Header("== �������� ==")]
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
        //ֱ�����
        //textLabel.text = textList[index++];
        //Э�����
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
                //ֱ�����
                //textLabel.text = textList[index++];
                //Э�����
                StartCoroutine(SetTextUI());
            }
        }
        
    }

    void GetTextFormFile(TextAsset file)
    {
        textList.Clear();
        index = 0;
        //�Ի���Ϊ�ָ�������ȡ�ļ�
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
            case "���":
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
