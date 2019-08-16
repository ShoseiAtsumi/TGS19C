using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Submarine_Volcano : MonoBehaviour
{
    /// <summary>
    /// ステージギミック
    /// 海底火山
    /// 3～8秒ランダム待機
    /// 2秒間噴出
    /// </summary>
    private BoxCollider2D col;
    private SpriteRenderer sprite;
    public Game_controller  game_Controller;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        StartCoroutine("Eruption");
    }

    // コルーチンでギミックをループ
    IEnumerator Eruption()
    {
        while (true)
        {
            // 待機
            float cooltime = Random.Range(3.0f, 8.0f);
            yield return new WaitForSeconds(cooltime);

            // 噴出
            col.enabled = true;
            sprite.enabled = true;

            yield return new WaitForSeconds(2.0f);
            col.enabled = false;
            sprite.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 当たった場合制限時間を減算(-5秒)
        game_Controller.time -= 5.0f;
    }
}
