using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    #region インスペクター表示
    [SerializeField]
    private Vector2 speed;
    #endregion

    #region private
    private Rigidbody2D rb;
    private GameObject arrow;
    private Animator animator;
    private GetInput getInput;

    private Vector2 touch_start;
    private Vector2 touch_end;
    private Vector2 force;
    

    private float rad;
    private float distance;

    private bool force_enter;
    private bool on_stay;
    private bool move;
    private bool isCeiling;
    private bool isfall;
    private bool on_move;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        getInput = GameObject.Find("GameController").GetComponent<GetInput>();
        arrow = GameObject.Find("Arrow");

        on_stay = true;
        force_enter = false;
        move = false;
        isCeiling = false;
        isfall = false;
        on_move = false;

        arrow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        arrow.transform.position = transform.position;

        State();

        if (!isCeiling && rb.velocity == Vector2.zero)
        {
            if (getInput.GetTouch(0, 0))
            {
                touch_start = getInput.Position(0);
                arrow.SetActive(true);
            }
            if (touch_start != Vector2.zero)
            {
                if (getInput.GetTouch(0, 1))
                {
                    touch_start = Vector2.zero;
                    arrow.SetActive(false);
                    move = true;
                }
                if (getInput.GetTouch(0, 2))
                {
                    touch_end = getInput.Position(0);
                    Force_set();
                    arrow.transform.localRotation = Quaternion.FromToRotation(Vector2.up, force);
                    arrow.transform.localScale = new Vector2(distance * 0.2f, distance * 1.0f);
                }
            }
        }
    }

    // forceに代入
    void Force_set()
    {
        rad = Mathf.Atan2(touch_start.y - touch_end.y, touch_start.x - touch_end.x);

        distance = Vector2.Distance(touch_start, touch_end);

        distance = Mathf.InverseLerp(0, 200, distance);

        force.x = distance * speed.x * Mathf.Cos(rad);
        force.y = distance * speed.y * Mathf.Sin(rad);
    }

    void FixedUpdate()
    {
        // ふっとばし
        if (move)
        {
            rb.AddForce(force, ForceMode2D.Impulse);
            force = Vector2.zero;
            move = false;
            on_stay = false;
            on_move = true;
        }
    }

    // 当たり判定
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 地面かどうか
        if (collision.gameObject.tag == "Ground")
        {
            // 移動ストップ
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            // 当たった相手のトランスフォームを修得
            var Ground_tf = collision.gameObject.GetComponent<Transform>();

            // 天井or地面
            if (transform.position.y > Ground_tf.position.y)
            {
                isCeiling = false;
                rb.velocity = Vector2.zero;
                rb.constraints = RigidbodyConstraints2D.None;
                on_stay = true;
                on_move = false;
            }
            else
            {
                isCeiling = true;
                StartCoroutine(DelayMethod(0.5f));
                isfall = true;
                on_move = false;
            }
        }
    }

    //状態遷移用
    public int State()
    {
        int num;
        if (on_move && rb.velocity != Vector2.zero)
        {
            animator.SetBool("idle", false);
            animator.SetBool("jump", true);
            animator.SetBool("collision", false);
            animator.SetBool("fall", false);
            animator.SetBool("stand up", false);
            return num = 0;
        }

        if (isCeiling)
        {
            animator.SetBool("idle", false);
            animator.SetBool("jump", false);
            animator.SetBool("collision", true);
            animator.SetBool("fall", false);
            animator.SetBool("stand up", false);
        }

        if (isCeiling == true && rb.velocity.y < 0 || on_stay && rb.velocity.y < 0)
        {
            animator.SetBool("idle", false);
            animator.SetBool("jump", false);
            animator.SetBool("collision", false);
            animator.SetBool("fall", true);
            animator.SetBool("stand up", false);
            return num = 1;
        }

        if (on_stay && rb.velocity == Vector2.zero)
        {
            animator.SetBool("idle", false);
            animator.SetBool("jump", false);
            animator.SetBool("collision", false);
            animator.SetBool("fall", false);
            animator.SetBool("stand up", true);
            DelayMethod(0.5f);
        }
        return num = 100;
    }

    // 硬直用
    private IEnumerator DelayMethod(float num)
    {
        yield return new WaitForSeconds(num);
        rb.constraints = RigidbodyConstraints2D.None;
    }
}

