using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public enum STATE
{
    START,
    RUN,
    UPLOAD,
    FINISHED
}

public class MyGameManager : MonoBehaviour
{
    public AudioClip clipToPlay;
    public Text txt;
    public int lineNum = 1;
    public List<int> answers;
    public STATE state;

    private AudioSource asource;

    void Awake()
    {
        asource = GetComponent<AudioSource>();
        if (asource == null)
        {
            asource = gameObject.AddComponent<AudioSource>();
        }
        asource.spatialBlend = 0;

        answers = new List<int>();
    }

    void Update()
    {
        if (state == STATE.START)
        {

        }
        else if (state == STATE.RUN)
        {
            Update2();
        }
        else if (state == STATE.UPLOAD)
        {
            txt.text = "資料上傳中";

            StartCoroutine(SendToBackend());
            state = STATE.FINISHED;
        }
        else if (state == STATE.FINISHED)
        {

        }
    }

    IEnumerator SendToBackend() {

        StringBuilder builder = new StringBuilder();

        foreach (int answer in answers) {
            builder.Append(answer.ToString());
        }

        UnityWebRequest uwr = UnityWebRequest.Get("http://www.memoryabc.com/nangang/?param=" + builder.ToString());  
        yield return uwr.SendWebRequest();
        if (uwr.isHttpError || uwr.isNetworkError)
        {
            txt.text = "上傳失敗" + uwr.error;
        }
        else
        {
            txt.text = "上傳成功" + uwr.downloadHandler.text;
        }
    }


    void Update2() {

        if (lineNum == 1)
        {
            txt.text = state + ":" + "這是第一題:太陽的英文是甚麼? A. Sun B. Moon C. Star";
        }
        else if (lineNum == 2)
        {
            txt.text = state + ":" + "這是第二題";
        }
        else if (lineNum == 3)
        {
            txt.text = state + ":" + "這是第三題";
        }
        else if (lineNum == 4)
        {
            txt.text = state + ":" + "這是第四題";
        }
        else if (lineNum == 5)
        {
            txt.text = state + ":" + "這是第五題";
        }
        else if (lineNum == 6)
        {
            state = STATE.UPLOAD;
        }
    }


    public void OnButtonClicked(int choice) {
        if (clipToPlay != null && asource != null)
        {
            asource.clip = clipToPlay;
            asource.Play();
        }

        lineNum = lineNum + 1;
        answers.Add(choice);
    }

    public void OnTargetFound() {
        state = STATE.RUN;
        lineNum = 1;
        answers.Clear();
    }
}
