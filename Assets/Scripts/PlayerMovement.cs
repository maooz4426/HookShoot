
using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    private Animator animator;
    [SerializeField]
    private Vector3 velocity;
    [SerializeField]
    private float jumpPower = 5f;
    //　入力値
    private Vector3 input;
    //　歩く速さ
    [SerializeField]
    private float walkSpeed = 4f;
    //　rigidbody
    private Rigidbody rigid;
    //　地面に接地しているかどうか
    [SerializeField]
    private bool isGrounded;
    //　前方の壁に衝突しているかどうか
    [SerializeField]
    private bool isCollision;
    //　ジャンプ中かどうか
    [SerializeField]
    private bool isJump;
    //　ジャンプ後の着地判定までの遅延時間
    [SerializeField]
    private float delayTimeToLanding = 0.5f;
    //　ジャンプ後の時間
    [SerializeField]
    private float jumpTime;
    //　接地確認のコライダの位置のオフセット
    [SerializeField]
    private Vector3 groundPositionOffset = new Vector3(0f, 0.02f, 0f);
    //　接地確認の球のコライダの半径
    [SerializeField]
    private float groundColliderRadius = 0.29f;
    //　衝突確認のコライダの位置のオフセット
    [SerializeField]
    private Vector3 collisionPositionOffset = new Vector3(0f, 0.5f, 0.1f);
    //　衝突確認の球のコライダの半径
    [SerializeField]
    private float collisionColliderRadius = 0.3f;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }


    void Update()
    {
        CheckGround();
        //　キャラクターが接地している場合
        if (isGrounded)
        {
            //　接地したので移動速度を0にする
            velocity = Vector3.zero;
            input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            //　方向キーが多少押されている
            if (input.magnitude > 0f)
            {
                animator.SetFloat("Speed", input.magnitude);
                transform.LookAt(rigid.position + input);

                velocity = rigid.transform.forward * walkSpeed;
                //　キーの押しが小さすぎる場合は移動しない
            }
            else
            {
                animator.SetFloat("Speed", 0f);
            }
            //　ジャンプ
            if (Input.GetButtonDown("Jump"))
            {
                isGrounded = false;
                isJump = true;
                jumpTime = 0f;
                velocity.y = jumpPower;
                animator.SetBool("Jump", true);
                // 2ax = v²-v₀²より
                //velocity.y = Mathf.Sqrt(-2 * Physics.gravity.y * jumpPower);
            }
        }
        //　接触していたら移動方向の値は0にする
        if (!isGrounded && isCollision)
        {
            velocity = new Vector3(0f, velocity.y, 0f);
        }
        //　ジャンプ時間の計算
        if (isJump && jumpTime < delayTimeToLanding)
        {
            jumpTime += Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        //　キャラクターを移動させる処理
        rigid.MovePosition(rigid.position + velocity * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //　指定したコライダと接触、かつ接触確認コライダと接触していたら衝突状態にする
        if (Physics.CheckSphere(rigid.position + transform.up * collisionPositionOffset.y + transform.forward * collisionPositionOffset.z, collisionColliderRadius, ~LayerMask.GetMask("Player"))
            )
        {
            isCollision = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //　指定したコライダと離れたら衝突していない状態にする
        isCollision = false;
    }

    //　地面のチェック
    private void CheckGround()
    {
        //　地面に接地しているか確認
        if (Physics.CheckSphere(rigid.position + groundPositionOffset, groundColliderRadius, ~LayerMask.GetMask("Player")))
        {
            //　ジャンプ中
            if (isJump)
            {
                if (jumpTime >= delayTimeToLanding)
                {
                    isGrounded = true;
                    isJump = false;
                }
                else
                {
                    isGrounded = false;
                }
            }
            else
            {
                isGrounded = true;
            }
        }
        else
        {
            isGrounded = false;
        }
        animator.SetBool("Jump", !isGrounded);
    }

    private void OnDrawGizmos()
    {
        //　接地確認のギズモ
        Gizmos.DrawWireSphere(transform.position + groundPositionOffset, groundColliderRadius);
        Gizmos.color = Color.blue;
        //　衝突確認のギズモ
        Gizmos.DrawWireSphere(transform.position + transform.up * collisionPositionOffset.y + transform.forward * collisionPositionOffset.z, collisionColliderRadius);
    }
}