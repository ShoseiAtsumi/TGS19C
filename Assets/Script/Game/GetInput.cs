using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetInput : MonoBehaviour
{

    /* 
     * touch_phase[0] = タッチ開始時＆クリック開始時
     * touch_phase[1] = 離したとき
     * touch_phase[2] = 押してる間
     * 
     * それぞれtrueを返す
     * 
     * GetTouch()のkeyには実機ならタップしている指、エディターならleftclick、rightclick
     * numには取得したい状態、上のtouch_phaseに対応
     * 戻り値はbool
     * 
     * Position(key) は取得したい指
     * 戻り値はvecter3
     * マウスには関係なし、0でok
     */

    private bool[] touch_phase = new bool[3];

    public bool GetTouch(int key, int num)
    {
        touch_phase[num] = false;


        //エディター
        if (Input.GetMouseButtonDown(key))
        {
            touch_phase[0] = true;
        }
        if (Input.GetMouseButtonUp(key))
        {
            touch_phase[1] = true;
        }
        if (Input.GetMouseButton(key))
        {
            touch_phase[2] = true;
        }

#if UNITY_ANDROID

        // 実機
        if (Input.touchCount > key)
        {
            Touch touch = Input.GetTouch(key);

            if (touch.phase == TouchPhase.Began)
            {
                touch_phase[0] = true;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                touch_phase[1] = true;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                touch_phase[2] = true;
            }
        }

#endif
        return touch_phase[num];
    }

    //touchおよびマウスのpositionを取得
    public Vector3 Position(int key)
    {
        Vector3 position;


        position = Input.mousePosition;
#if UNITY_ANDROID
        Touch touch = Input.GetTouch(key);
        position = touch.position;
#endif
        return position;
    }
}
