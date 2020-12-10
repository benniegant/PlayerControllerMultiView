using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform target;
	public float smoothSpeed        = 0.08f;
    public Vector3 offset;
    public float rotationSpeed      = 1;
    public Transform Target_, Player;
    public float mouseX, mouseY, mouseY_Ground;
    public Transform Obstruction;
    public float zoomSpeed          = 2f;
    public Transform CameraMount;
    public float mousex;
    public float mousey;
    public float BreatheRate        =1.5f;

    bool View_leftShoulder          = false;
    bool View_RighShoulder          = false;
    bool View_FPS = false;
    bool View_ThirdPerson = false;

    public GameObject targetRotation_a;

    private void Start()
    {
         targetRotation_a = new GameObject();
    }
    void Update()
    {
        BreatheCamera();

    }
    private void LateUpdate()
    {
        CamControl(); 
    }
    private void FixedUpdate()
    {
        CamControl();
        LookAtTargetOnPlayer();
    }
    void LookAtTargetOnPlayer()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        transform.LookAt(target);
    }
    void Cam_Switch_Views()
    {
        // Left and Right Shoulder Views
        if (Input.GetKeyDown(KeyCode.Q))
        {

        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {

        }

    }
    void CamControl()
    {
        if (mouseX <= 0)
        {
            mouseX = 360;
        }
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, 0,90);

        if (mouseY <= 3)
        {
            //mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
            //mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
           // mouseY = Mathf.Clamp(mouseY, 0, 90);
            ///CameraMount.transform.position = Vector3.Lerp(CameraMount.transform.position, Target_.transform.position + new Vector3(0, 0,  -12), smoothSpeed * Time.deltaTime,Spc
               // );
            // Target_.position = new Vector3(0,Input.GetAxis("Mouse Y") * rotationSpeed,0);
            //  mouseY_Ground -= Input.GetAxis("Mouse Y") * rotationSpeed;

            //  targetRotation_a.transform.rotation = Quaternion.Euler(mouseY_Ground, 0, 0);

            // gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, targetRotation_a.transform.rotation, (rotationSpeed / 1.2f) * Time.deltaTime);


            // mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;x
            // transform.Rotate(new Vector3(mouseY, 0,0));
        }
        else {
            //mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
            //mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
            //mouseY = Mathf.Clamp(mouseY, 0, 90);

            //CameraMount.transform.position = Vector3.Lerp(CameraMount.transform.position, new Vector3(0, 0, - 12), smoothSpeed * Time.deltaTime);
        }


        offset = new Vector3(offset.x, mouseY, offset.z);

            Vector3 desiredPosition = CameraMount.transform.position;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = new Vector3(CameraMount.transform.position.x,transform.position.y, CameraMount.transform.position.z);

        float rotateClampY = 0;
        rotateClampY += mouseX;
        mousex = rotateClampY;
        mousey = mouseY;
        transform.LookAt(Target_.transform.position);//maybe set and offset which brings the cameras focuse position up a bit when camera is on ground.
        GameObject targetRotation = new GameObject();
        targetRotation.transform.rotation = Quaternion.Euler(0, mouseX, 0);
        Player.GetComponent<Rigidbody>().rotation = Quaternion.Slerp(Player.rotation, targetRotation.transform.rotation, (rotationSpeed / 1.2f) * Time.deltaTime);

        Destroy(targetRotation);
    }
    void BreatheCamera()
    {
     //   transform.Translate(2f * (Vector3.up * Mathf.Cos(Time.time * BreatheRate) * Time.deltaTime));
    }
}