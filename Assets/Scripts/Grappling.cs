using StarterAssets;
using UnityEngine;
public class Grappling : MonoBehaviour
{
    private LineRenderer lr;
    //grappleする場所
    private Vector3 grapplePoint;
    //grapple出来る場所を指定する
    public LayerMask whatIsGrappleable;
    private Camera _camera;
    public Transform gunTip, player;
    private float maxDistance = 100f;
    //2 つの Rigidbody をグループ化し、バネで連結されているかのように動かせる
    private SpringJoint joint;
    private CharacterController characterController;
    private Rigidbody rb;
    private PlayerMovement playerMovement;
    private ThirdPersonController thirdPersonController;
    
    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        _camera = Camera.main;
        Transform CameraTrans = _camera.transform;

        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        thirdPersonController = GetComponent<ThirdPersonController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }
    }

    //Updateメソッドが呼ばれた後に実行されるメソッド
    //ロープが少しずれるのを防ぐ
    void LateUpdate()
    {
        DrawRope();
    }

    void StartGrapple()
    {
        //print("start");
        //Raycastがヒットしたオブジェクトの情報を格納する
        //characterController.enabled = false;
        rb.useGravity = true;
        playerMovement.enabled = true;
        //thirdPersonController.enabled = false;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance, whatIsGrappleable))
        {
            Debug.Log("s");
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            //接続点（connectedAnchor）を手動で設定できる
            joint.autoConfigureConnectedAnchor = false;
            //SpringJoint の接続点を先ほどの grapplePoint に設定
            joint.connectedAnchor = grapplePoint;
            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);
            //maxdistaceになったらばねの挙動をして戻される
            joint.maxDistance = distanceFromPoint * 0.8f;
            //mindistaceになったらばねの挙動をして戻される
            joint.minDistance = distanceFromPoint * 0.25f;
            //設定調整
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;
            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;
        }
    }


    void StopGrapple()
    {
       //characterController.enabled = true;
       rb.useGravity = false;
       playerMovement.enabled = false;
       //thirdPersonController.enabled = true;
       lr.positionCount = 0;
       Destroy(joint);
    }


    private Vector3 currentGrapplePosition;
    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!joint) return;
        //等速直線運動
        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }
    public bool IsGrappling()
    {
        return joint != null;
    }
    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}