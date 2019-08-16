using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using MiniJSON;
using UniRx;

public class ScoreboardDB : MonoBehaviour
{
    #region インスペクター表示フィールド
    [SerializeField]
    private Text inputTextArea;

    [SerializeField]
    private string ServerAddress;

    [SerializeField]
    private GameObject ErrorLog;
    [SerializeField]
    private Text[] scoreboad = new Text[10];

    [SerializeField]
    private GameObject send_button, go_result_button;
    #endregion

    private void Start()
    {
        send_button.GetComponent<Button>().OnClickAsObservable()
            .Subscribe(_ => StartCoroutine("Access"))
            .AddTo(gameObject);
    }

    // Dictionaryにpostするデータをセットしてスタートコルーチンpost
    private IEnumerator Access()
    {
        var dic = new Dictionary<string, string>();

        // インプットフィールドに入力された名前
        dic.Add("name", inputTextArea.text);
        // 今回のゲームでの最高到達点
        dic.Add("score", Game_controller.max_distance.ToString("F2"));

        StartCoroutine(Post(ServerAddress, dic));  // POST

        yield return 0;
    }

    // post
    private IEnumerator Post(string url, Dictionary<string, string> post)
    {
        // フォームを作りUnityWebRequest.postで投げる
        WWWForm form = new WWWForm();
        foreach (KeyValuePair<string, string> post_arg in post)
        {
            form.AddField(post_arg.Key, post_arg.Value);
        }

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            // Errorが出た場合Errorを表示、なければGetJsonへ
            if (www.isNetworkError)
            {
                Debug.Log(www.error);
                ErrorLog.SetActive(true);
            }
            else
            {
                yield return StartCoroutine(GetJson(url));
            }
        }   
    }

    // jsonの受け取りとリストへの格納
    private IEnumerator GetJson(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        /* 
         * Errorが出た場合Errorを表示、なければ変えてきたjsonをリストに入れ
         * ShowRankにリストを渡しスタートコルーチン
         */
        if (request.isNetworkError)
        {
            Debug.Log(request.error);
            ErrorLog.SetActive(true);
        }
        else
        {
            if (request.responseCode == 200)
            {
                var json = request.downloadHandler.text;
                var ranklist = (IList)Json.Deserialize(json);
                StartCoroutine(ShowRank(ranklist));
            }
        }
    }

    // ランキングの表示
    private IEnumerator ShowRank(IList list)
    {
        int i = 0;
        foreach (IDictionary rank in list)
        {
            string name = (string)rank["name"];
            string score = (string)rank["score"];

            // Textに文字を挿入
            scoreboad[i].text = i + 1 + "位" + "  " + name + "  " + score + "M";

            i++;
        }

        // sendボタンを消し
        // titleに戻るボタンを表示
        send_button.SetActive(false);
        go_result_button.SetActive(true);
        yield return null;
    }
}
