using UnityEngine;

public class FreeCameraLook : Pivot {

    [Tooltip ("Makes transform root rotate along camera X movement")]
    public bool RotateTransformRoot;

    /* [Header ("Axis setup")]
    public string mouseX;
    public string mouseY; */

    [Header ("Debug")]
    [Tooltip ("Debug X axis")]
    public float _inputX;
    [Tooltip ("Debug Y axis")]
    public float _inputY;

    [Header ("Camera Controls")]
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float turnSpeed = 1.5f;
    [SerializeField] float turnSpeedMultiplier = 1;
	[SerializeField] private float turnsmoothing = .1f;
	[SerializeField] private float tiltMax = 75f;
	[SerializeField] private float tiltMin = 45f;

	private float lookAngle;
	private float tiltAngle;

	private const float LookDistance = 100f;

	private float smoothX = 0;
	private float smoothY = 0;
	private float smoothXvelocity = 0;
	private float smoothYvelocity = 0;
    private float offsetX;
    private float offsetY;

    public float crosshairOffsetWiggle = 0.2f;
    private CrosshairManager crosshairManager;

    private PlayerController playC;

    //add the singleton
    public static FreeCameraLook instance;
    
    public static FreeCameraLook GetInstance()
    {
        return instance;
    }

	protected override void Awake()
	{
        instance = this;

		base.Awake();

		cam = GetComponentInChildren<Camera>().transform;
		pivot = cam.parent.parent; //take the correct pivot


        playC = transform.root.GetComponent<PlayerController>();
	}

    protected override void Start()
    {
        base.Start();

        crosshairManager = CrosshairManager.GetInstance();
    }
	
    protected override void Update ()
	{
		base.Update();

		HandleRotationMovement();

        if (playC.isDead) {
            RotateTransformRoot = false;
        }
        else
        {
            RotateTransformRoot = true;
        }
	}

	protected override void Follow (float deltaTime)
	{
		transform.position = Vector3.Lerp(transform.position, target.position, deltaTime * moveSpeed);
	}

	void HandleRotationMovement()
	{
        HandleOffsets();

        if (playC.Player_ == 0)
        {
            _inputX = InputManager.Instance.MouseInput0.x;
            _inputY = InputManager.Instance.MouseInput0.y;
        }
        else if (playC.Player_ == 1) {
            _inputX = InputManager.Instance.MouseInput1.x;
            _inputY = InputManager.Instance.MouseInput1.y;
        }
        else if (playC.Player_ == 2) {
            _inputX = InputManager.Instance.MouseInput2.x;
            _inputY = InputManager.Instance.MouseInput2.y;
        }

        //float x = InputManager.Instance.MouseInput.x + offsetX;
        //Input.GetAxis("Mouse X") + offsetX;
        float x = _inputX + offsetX;

		//float y = InputManager.Instance.MouseInput.y + offsetY;
        //Input.GetAxis("Mouse Y") + offsetY;
        float y = _inputY + offsetY;

        if (turnsmoothing > 0)
        {
            smoothX = Mathf.SmoothDamp(smoothX, x, ref smoothXvelocity, turnsmoothing);
            smoothY = Mathf.SmoothDamp(smoothY, y, ref smoothYvelocity, turnsmoothing);
        }
        else
        {
            smoothX = x;
            smoothY = y;
        }

		lookAngle += smoothX * turnSpeed;

		transform.rotation = Quaternion.Euler(0f, lookAngle, 0);

        if (RotateTransformRoot) {
            target.rotation = Quaternion.Euler(0f, lookAngle, 0);
        }

        //* Rotate player along camera */
        //target.transform.Rotate(Vector3.up * smoothX * turnSpeed * turnSpeedMultiplier);
        //*----------------------------*/

		tiltAngle -= smoothY * turnSpeed;
		tiltAngle = Mathf.Clamp (tiltAngle, -tiltMin, tiltMax);

		pivot.localRotation = Quaternion.Euler(tiltAngle,0,0);

        if (x > crosshairOffsetWiggle || x < -crosshairOffsetWiggle || y > crosshairOffsetWiggle || y < -crosshairOffsetWiggle)
        {
            WiggleCrosshairAndCamera(0);
        }
	}

    void HandleOffsets()
    {
        if (offsetX != 0)
        {
            offsetX = Mathf.MoveTowards(offsetX, 0, Time.deltaTime);
        }

        if (offsetY != 0)
        {
            offsetY = Mathf.MoveTowards(offsetY, 0, Time.deltaTime);
        }
    }

    public void WiggleCrosshairAndCamera(float kickback)
    { 
        crosshairManager.activeCrosshair1.WiggleCrosshair();

        offsetY = kickback;
    }
}
