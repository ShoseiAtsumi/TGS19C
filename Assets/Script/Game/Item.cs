using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float time_num;

    //当たり判定
    void OnTriggerEnter2D(Collider2D collider)
    {
        //プレイヤーかどうか
        if (collider.gameObject.tag == "Player")
        {
            var game_Controller = GameObject.Find("GameController").GetComponent<Game_controller>();
            //残り時間加算
            game_Controller.time += time_num;
            //自分を削除
            Destroy(gameObject);
        }
    }
}