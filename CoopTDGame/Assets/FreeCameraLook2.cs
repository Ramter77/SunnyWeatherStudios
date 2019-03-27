using UnityEngine;

public class FreeCameraLook2 : Pivot
{
    public string mouseX2, mouseY2;
    public float m_Speed = 1;


	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float turnSpeed = 1.5f;
	[SerializeField] private float turnsmoothing = .1f;
	[SerializeField] private float tiltMax = 75f;
	[SerializeField] private float tiltMin = 45f;
	[SerializeField] private bool lockCursor = false;

	private float lookAngle;
	private float tiltAngle;

	private const float LookDistance = 100f;

	private float smoothX = 0;
	private float smoothY = 0;
	private float smoothXvelocity = 0;
	private float smoothYvelocity = 0;

    public float crosshairOffsetWiggle = 0.2f;
    CrosshairManager crosshairManager;

    //add the singleton
    public static FreeCameraLook2 instance;
    
    public static FreeCameraLook2 GetInstance()
    {
        return instance;
    }

	protected override void Awake()
	{
        instance = this;

		base.Awake();

		cam = GetComponentInChildren<Camera>().transform;
		pivot = cam.parent.parent; //take the correct pivot
	}

    protected override void Start()
    {
        base.Start();

        if (lockCursor)
            Cursor.lockState = CursorLockMode.Locked;

        crosshairManager = CrosshairManager.GetInstance();
    }
	
	// Update is called once per frame
    protected override void Update ()
	{
		base.Update();

		HandleRotationMovement();

	}

	protected override void Follow (float deltaTime)
	{
		transform.position = Vector3.Lerp(transform.position, target.position, deltaTime * moveSpeed);

	}

    void SmoothLook(Vector3 newDirection){
        transform.root.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newDirection), m_Speed * Time.deltaTime);
    }

	void HandleRotationMovement()
	{
        HandleOffsets();
		//float x = InputManager.Instance.MouseInput.x + offsetX;
        //Input.GetAxis("Mouse X") + offsetX;
        float x = Input.GetAxis(mouseX2) + offsetX;

		//float y = InputManager.Instance.MouseInput.y + offsetY;
        //Input.GetAxis("Mouse Y") + offsetY;
        float y = Input.GetAxis(mouseY2) + offsetY;

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

        //Rotate player along camera
        target.root.transform.Rotate(Vector3.up * smoothX * turnSpeed);
        /*
        Vector3 ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)).GetPoint(100);
        Vector3 rayXZ = new Vector3(ray.x, transform.position.y, ray.z);
        target.transform.LookAt(rayXZ);
*/



        Vector3 smoLok = Camera.main.transform.forward;
        smoLok.y = 0;
        //SmoothLook(smoLok);




		tiltAngle -= smoothY * turnSpeed;
		tiltAngle = Mathf.Clamp (tiltAngle, -tiltMin, tiltMax);

		pivot.localRotation = Quaternion.Euler(tiltAngle,0,0);

        if (x > crosshairOffsetWiggle || x < -crosshairOffsetWiggle || y > crosshairOffsetWiggle || y < -crosshairOffsetWiggle)
        {
            WiggleCrosshairAndCamera(0);
        }
	}


    float offsetX;
    float offsetY;
    

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
        crosshairManager.activeCrosshair.WiggleCrosshair();

        offsetY = kickback;
    }


}
