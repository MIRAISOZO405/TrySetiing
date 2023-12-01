using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    
    [Header("移動の速さ"), SerializeField] private float speed = 3;  
    [Header("氷床の摩擦抵抗"), Tooltip("1は減速無し、0は止まる。"),SerializeField] private float iceFrictional = 0.995f;
    [Header("氷床での方向変化のスムーズさ"), SerializeField] private float iceSmoothness = 0.1f;
    [Header("泥ブロックでの移動速度減少"), Tooltip("1.0が通常速度、0.5なら半分の速度に"), SerializeField] private float mudSlowdown = 0.5f;

    private CinemachineVirtualCamera virtualCamera;    // バーチャルカメラ
    private Camera mainCamera;
    private Animator animator;
    private CharacterController characterController;

    private Vector2 inputMove;
    private float verticalVelocity;
    private float turnVelocity;

    [Space]
    [Header("接地判定")]
    [SerializeField] private bool isGround;
    [SerializeField] private bool isIce;
    [SerializeField] private bool isMud;
    //[SerializeField] private bool isBelt;
    
    private Vector3 moveDirection = Vector3.zero; // 進行方向
    public BeltConveyor currentConveyor;    // ベルトコンベア

    [Space]
    [SerializeField] private bool isShop = false; // shopに触れているか
    private Image bButton; // isShopと連動させる
    private PostProcessManager postprocessManager; // postprocessの操作csを取得
    private CanvasManager canvasManager;
    private Canvas shopCanvas;

    private RenderChange renderChange;

    private void Awake()
    {
        mainCamera = GameObject.Find("Camera").GetComponent<Camera>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        this.GetComponent<InputManager>().SwitchToPlayer(); // ActionMap切り替え
        bButton = GameObject.Find("bButton").GetComponent<Image>();
        shopCanvas = GameObject.Find("ShopCanvas").GetComponent<Canvas>();

        if (!shopCanvas)
            Debug.LogError("shopCanvasが見つからない");

        // postprocessのモザイクを無効化
        postprocessManager = FindObjectOfType<PostProcessManager>();
        if (postprocessManager)
        {
            postprocessManager.ActiveDepthOfField(false);
        }
        else
        {
            Debug.LogError("PostProcessが見つからない");
        }

        canvasManager = FindObjectOfType<CanvasManager>();
        if (!canvasManager)
        {
            Debug.LogError("CanvasManagerが見つからない");
        }

        if (bButton)
        {
            bButton.enabled = isShop;
        }
        else
        {
            Debug.LogError("bButtonが見つからない");
        }

        renderChange = GameObject.Find("RenderCamera_Player").GetComponent<RenderChange>();
        if (!renderChange)
        {
            Debug.LogError("renderChangeコンポーネントが見つかりません");
        }

        if (virtualCamera)
        {
            virtualCamera.Follow = this.transform.Find("LookPos").gameObject.transform;
            virtualCamera.LookAt = this.transform.Find("LookPos").gameObject.transform;
        }
        else
        {
            Debug.LogError("virtualCameraコンポーネントが見つかりません");
        }
    }

    // ボタンUI表示
    public void EnterShop(bool front)
    {
        isShop = front;
        bButton.enabled = front;
    }

    public void OnAction(InputAction.CallbackContext context)
    {
        if(isShop)
        {
            this.GetComponent<InputManager>().SwitchToUI(); // ActionMap切り替え
            virtualCamera.enabled = false;  // カメラ止める

            if (inputMove != Vector2.zero)
                inputMove = Vector2.zero;

            postprocessManager.ActiveDepthOfField(true);
            canvasManager.EnableOnlyThisCanvas(shopCanvas); // shopCanvasのみを表示
            shopCanvas.GetComponent<ShopManager>().GoodsReset();    // goodsのanimationのため

            renderChange.CurrentModelChange();
        }
    }

    public void OnReturnMap()
    {
        this.GetComponent<InputManager>().SwitchToPlayer(); // ActionMap切り替え
        virtualCamera.enabled = true;  // カメラ動かす
        postprocessManager.ActiveDepthOfField(false);
        canvasManager.DisableOnlyThisCanvas(shopCanvas);    // shopCanvasのみを非表示
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // 入力値を保持しておく
        inputMove = context.ReadValue<Vector2>();
    }



    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // 接地判定
        if (hit.gameObject.tag == "Block")
        {
            animator.SetBool("isJump", false);
            isGround = true;
        }
        else
        {
            isGround = false;
        }

        if (hit.gameObject.tag == "IceBlock")
        {
            isIce = true;
        }
        else
        {
            isIce = false;
        }

        if (hit.gameObject.tag == "mudBlock")
        {
            isMud = true;
        }
        else
        {
            isMud = false;
        }

        //if (hit.gameObject.tag == "ConveyorBlock")
        //{
        //    isBelt = true;
        //}
        //else
        //{
        //    isBelt = false;
        //}

    }

    private void Update()
    {
        UpdateMove();
    }


    private void UpdateMove()
    {
        // カメラの向き（角度[deg]）取得
        var cameraAngleY = mainCamera.transform.eulerAngles.y;

        if (inputMove != Vector2.zero)
        {
            // 入力に基づいて新しい方向を計算
            Vector3 inputDirection = new Vector3(inputMove.x, 0, inputMove.y).normalized;
            inputDirection = Quaternion.Euler(0, cameraAngleY, 0) * inputDirection;

            // 現在のmoveDirection（慣性）と入力方向を合成
            //moveDirection += inputDirection * iceSmoothness;
            if (isIce)
            {
                // 慣性の処理を氷の上にいるときのみ実行
                moveDirection += inputDirection * iceSmoothness;
            }
            else
            {
                moveDirection = inputDirection;
            }

            // 合成後の方向を正規化（長さを1に固定）
            moveDirection.Normalize();

            if (!animator.GetBool("isRun"))
            {
                animator.SetBool("isRun", true);
                // 足音SEを追加
            }

            var targetAngleY = -Mathf.Atan2(inputMove.y, inputMove.x) * Mathf.Rad2Deg + 90;
            targetAngleY += cameraAngleY;
            var angleY = Mathf.SmoothDampAngle(this.transform.eulerAngles.y, targetAngleY, ref turnVelocity, 0.1f);
            this.transform.rotation = Quaternion.Euler(0, angleY, 0);
        }
        else
        {
            if (animator.GetBool("isRun"))
            {
                animator.SetBool("isRun", false);
                // 足音SEの停止
            }

            if (isIce)
                moveDirection *= iceFrictional;
            else
                moveDirection = Vector3.zero;
        }

        float currentSpeed = speed;
        if (isMud)
        {
            currentSpeed *= mudSlowdown;
        }

        var moveVelocity = moveDirection * currentSpeed + new Vector3(0, verticalVelocity, 0);
        var moveDelta = moveVelocity * Time.deltaTime;
        characterController.Move(moveDelta);

    }
}