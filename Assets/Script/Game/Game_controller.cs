using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class Game_controller : MonoBehaviour
{
    [System.NonSerialized]
    public float time = 300.0f;
    private float seconds;
    private float minutes;
    public static float max_distance;

    private Text Timeleft;
    private Text Score;

    private float score = 0.0f;

    private bool freeze = false;
    public static bool pause;

    private Transform player_tf;
    GameObject main_cam;
    GameObject pause_cam;
    GameObject pause_menu;

    // Start is called before the first frame update
    void Start()
    {
        //初期化
        max_distance = 0.0f;
        Timeleft = GameObject.Find("Time").GetComponent<Text>();
        Score = GameObject.Find("Distance").GetComponent<Text>();
        player_tf = GameObject.Find("Player").GetComponent<Transform>();
        main_cam = GameObject.Find("Main Camera");
        pause_cam = GameObject.Find("PauseCam");
        pause_menu = GameObject.Find("Pause_menu");
        pause_cam.SetActive(false);
        pause_menu.SetActive(false);
        pause = false;

        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // タイムカウント
        // 残り秒数を60で割って分に変換
        minutes = Mathf.Floor(time / 60);
        // 残り秒数を60で割って余りを秒数に変換
        seconds = Mathf.Floor(time % 60);

        if (seconds == 60)
        {
            seconds = 0;
        }

        // 残り時間減算
        time -= Time.deltaTime;

        // シーン遷移
        // 残り時間が0になったらリザルトシーンへ遷移
        if (time <= 0.0f)
        {
            Load_result();
        }
        // シーン遷移

        // UI
        // テキストに残り時間を表示
        Timeleft.text = minutes.ToString("0") + "分" + seconds.ToString("0") + "秒";

        //距離測定
        Distance();

        // デバッグ用
        // Rキーでリロード
        if (Input.GetKey(KeyCode.R))
        {

            Reload();
        }

        // 時間減算停止
        if (Input.GetKeyDown(KeyCode.F))
        {
            freeze = !freeze;
        }
        if (freeze)
        {
            time += Time.deltaTime;
        }

        // スペースキーでリザルト遷移
        if (Input.GetKeyDown("space"))
        {
            Load_result();
        }
    }

    public void Load_result()
    {
        SceneManager.LoadScene("Result");
    }

    public void Load_title()
    {
        SceneManager.LoadScene("Title");
    }

    void Distance()
    {
        Vector2 pos1 = new Vector2(0.0f, -3.465946f);
        Vector2 pos2 = new Vector2(0.0f, player_tf.position.y);
        score = Vector2.Distance(pos1, pos2);
        if (max_distance < score)
        {
            Score.text = Vector2.Distance(pos1, pos2).ToString("f1") + "m";
            max_distance = score;
        }
    }

    public void Pause()
    {
        pause = !pause;

        if (pause)
        {
            Time.timeScale = 0;
            pause_cam.SetActive(true);
            main_cam.SetActive(false);
            pause_menu.SetActive(true);
            pause_cam.transform.position = main_cam.transform.position;
        }
        else
        {
            Time.timeScale = 1;
            main_cam.SetActive(true);
            pause_cam.SetActive(false);
            pause_menu.SetActive(false);
        }
    }

    public void Reload()
    {

        // 現在のシーン番号を取得
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        // 現在のシーンを再読込する
        SceneManager.LoadScene(sceneIndex);

        Time.timeScale = 1;

    }
}

