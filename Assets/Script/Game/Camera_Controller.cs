using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    private Vector3 tf;
    private Transform player_tf;
    Player player;
    private int state;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        player_tf = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤーのステートに応じて、移動を変更
        state = player.State();

        switch (state)
        {
            case 0: iTween.MoveUpdate(gameObject, new Vector3(0, player_tf.position.y, -10.0f), 2.0f); break;
            case 1: iTween.MoveUpdate(gameObject, new Vector3(0, player_tf.position.y - 2.0f, -10.0f), 2.0f); break;
            default: iTween.MoveUpdate(gameObject, new Vector3(0, player_tf.position.y + 3.5f, -10.0f), 2.0f); break;
        }
    }
}
