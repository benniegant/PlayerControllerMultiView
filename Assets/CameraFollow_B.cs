using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow_B : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Player_;
    public Camera PlayerCamera_;
    public GameObject CameraTarget;
    public GameObject CameraMount3rdPerson;
    public GameObject CameraMount1stPerson;
    public GameObject CameraMount_RightShoulder;
    public GameObject CameraMount_LeftShoulder;

    public bool ThirdPersonActive = true;
    public bool FirstPersonActive = false;
    public bool TopDownActive = false;
    public bool RightCamActive = false;
    public bool LeftCamActive = false;

    public Transform CameraFromPosition;
    public Transform CameraNewPosition;
    public Transform coMountPoint;

    public bool CamIsTransitioningKeyPress = false; //flag
    public bool LerpedUp = false;

    public float tTransitionTime = .4f;
    public float TransitionFract = .4f;
    public float TransitionSpeed = .4f;

    public float MousePosition_X;
    public float MousePosition_Y;

    public Vector3 CameraTarget_CurrentPosition = Vector3.zero;
    public Vector3 CameraTarget_DesiredPosition = Vector3.zero;
    public Vector3 CameraTarget_PreviousPosition = Vector3.zero;

    public float fract_target = .04f;

    public Transform target_second;


    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //Setting a Default if none of the views are set from the Unity's Editor inspector
        if (
                ThirdPersonActive == false &&
                FirstPersonActive == false &&
                TopDownActive == false &&
                RightCamActive == false &&
                LeftCamActive == false
            )

        {
            // Defualting to Third Person View as Active view. this can be changed by selecting a view from inside the 
            //Editor or by Changing this line of Code to any of the 5 available view(s)
            //ThirdPersonActive = true;
            //  coMountPoint = null;

            coMountPoint = CameraMount3rdPerson.transform;
            coMountPoint.position = CameraMount3rdPerson.transform.position;
            PlayerCamera_.transform.position = coMountPoint.transform.position;
        }


    }

    // Update is called once per frame

    public void moveMouseTarget()
    {

        transform.LookAt(target_second.transform.position);

        if (MousePosition_X <= 0)
        {
            MousePosition_X = 360;
        }
        MousePosition_X += Input.GetAxis("Mouse X") * fract_target *4;
        MousePosition_Y += Input.GetAxis("Mouse Y") * fract_target;
       // MousePosition_Y = Mathf.Clamp(MousePosition_Y, -345, 90);



        GameObject targetRotation = new GameObject();
        targetRotation.transform.rotation = Quaternion.Euler(0, MousePosition_X , 0);


        if (MousePosition_Y <= 0)
        {
            MousePosition_Y = 360;
        }
        //MousePosition_Y = Mathf.Clamp(MousePosition_Y, -90, 90);

        if (Mathf.Sign(MousePosition_Y) == -1)
        {
            MousePosition_Y = 360;
        }
        else { }
        PlayerCamera_.transform.Rotate(new Vector3(MousePosition_Y, 0, 0), -MousePosition_Y);// = Quaternion.Slerp(CameraTarget.transform.rotation, targetRotation.transform.rotation, (fract_target / 1.2f) * Time.deltaTime);

        Destroy(targetRotation);

        targetRotation = new GameObject();
        targetRotation.transform.rotation = Quaternion.Euler(0, MousePosition_X , 0);
        Player_.GetComponent<Rigidbody>().rotation = Quaternion.Slerp(Player_.transform.rotation, targetRotation.transform.rotation, fract_target  * Time.deltaTime);

        Destroy(targetRotation);




        // CameraTarget_DesiredPosition = new Vector3(MousePosition_X, MousePosition_Y, CameraTarget.transform.position.z);
        //CameraTarget.transform.position = Vector3.Lerp(CameraTarget.transform.position, CameraTarget_DesiredPosition,fract_target * Time.deltaTime);


    }

    void Update()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        CameraTarget.transform.LookAt(target_second.transform.position);
        ///Check Require Objects and The Camera
        ///
        if (Player_ != null &&
            PlayerCamera_ != null &&
            CameraTarget != null &&
            CameraMount3rdPerson != null &&
            CameraMount1stPerson != null &&
            CameraMount_RightShoulder != null &&
            CameraMount_LeftShoulder != null)

        {
            ///Attatch Camera to Camera Mount Point. The Mounting Point for the Camera should be a Child of the Player Object.
            ///If not attatch an empty object as a child object of the Player. This can be accomplished in the Unity Editor of (Optionally)
            ///Via Code.  This applies to the other Required Objects and Camera.

            //Cameras Position is set to the Cameras Mounting Objects Position. This places the Camera Behind the player.
            // PlayerCamera_.transform.position = CameraMount3rdPerson.transform.position;

            //Switch Camera View on KeyPress [TAB]
            switchCameraViews();

            if (coMountPoint)
            {
                Vector3 Temp_Pos = PlayerCamera_.transform.position;
                PlayerCamera_.transform.position = Vector3.Lerp(Temp_Pos, coMountPoint.transform.position, TransitionSpeed * Time.deltaTime);
            }

            moveMouseTarget();//get mouse position and move line of sight for camera targets



        }   
        else
        {
            return;
        }

    }
    public void ThirdPersonView()
    {
        StopCoroutine(COThirdPersonView());
        StartCoroutine(COThirdPersonView());
        FirstPersonActive = true;//Active!
    }
    public void FirstPersonView()
    {
        StopCoroutine(COFirstPersonView());
        StartCoroutine(COFirstPersonView());
        RightCamActive = true;
    }
    private void RightShoulderCamView()
    {
        StopCoroutine(CORightShoulderCamView());
        StartCoroutine(CORightShoulderCamView());
        LeftCamActive = true;
    }
    public void LeftShoulderCamView()
    {
        StopCoroutine(COLeftShoulderCamView());
        StartCoroutine(COLeftShoulderCamView());
        ThirdPersonActive = true; //Active!
    }
    public IEnumerator COThirdPersonView()
    {
        float currentProgress = 0;

        while (currentProgress <= 1)
        {
            CameraFromPosition = CameraMount_LeftShoulder.transform;
            CameraNewPosition = CameraMount3rdPerson.transform;
            currentProgress += TransitionFract * Time.deltaTime;

            coMountPoint.position = Vector3.Lerp(PlayerCamera_.transform.position,    //From - Position
                                                        CameraNewPosition.transform.position,    //Desired/to - Postion
                                                        Time.deltaTime * TransitionFract);          // Interpolate from and to over time(CPU Clock Tick

            PlayerCamera_.transform.position = coMountPoint.position;
           //tTransitionTime += Time.deltaTime * TransitionFract;
           ThirdPersonActive = false;
            FirstPersonActive = true;//Active!
                                     // TopDownActive       = false;
            RightCamActive = false;
            LeftCamActive = false;

            yield return null;
        }
    }
 
    public IEnumerator COFirstPersonView()
    {
        float currentProgress = 0;

        while (currentProgress <= 1)
        {
            ThirdPersonActive = false;
            FirstPersonActive = false;//Active!
                                      // TopDownActive       = false;
            RightCamActive = true;
            LeftCamActive = false;

            CameraFromPosition = CameraMount3rdPerson.transform;
            CameraNewPosition = CameraMount1stPerson.transform;

            currentProgress += TransitionFract * Time.deltaTime;
            coMountPoint.position = Vector3.Lerp(PlayerCamera_.transform.position,    //From - Position
                                                                CameraNewPosition.transform.position,    //Desired/to - Postion
                                                                Time.deltaTime * TransitionFract);          // Interpolate from and to over time(CPU Clock Ticks)

            PlayerCamera_.transform.position = coMountPoint.position;
            // tTransitionTime += Time.deltaTime * TransitionFract;
            yield return null;
        }
    }
 
    public IEnumerator CORightShoulderCamView()
    {
        float currentProgress = 0;

        while (currentProgress <= 1)
        {

            ThirdPersonActive = false;
            FirstPersonActive = false;//Active!
                                     // TopDownActive       = false;
            RightCamActive = false;
            LeftCamActive = true;

            CameraFromPosition = CameraMount1stPerson.transform;
            CameraNewPosition = CameraMount_RightShoulder.transform;


            currentProgress += TransitionFract * Time.deltaTime;
            coMountPoint.position  = Vector3.Lerp(PlayerCamera_.transform.position,
                                                            CameraNewPosition.transform.position,
                                                            Time.deltaTime * TransitionFract);
            PlayerCamera_.transform.position = coMountPoint.position;
            //tTransitionTime += Time.deltaTime * TransitionFract;
            yield return null;
        }
    }
    public IEnumerator COLeftShoulderCamView()
    {

      //  tTransitionTime += Time.deltaTime * TransitionFract;

        float currentProgress = 0;

        while (currentProgress <= 1)
        {

            ThirdPersonActive = true; //Active!
            FirstPersonActive = false;
            TopDownActive = false;
            RightCamActive = false;
            LeftCamActive = false;

            CameraFromPosition = CameraMount1stPerson.transform;
            CameraNewPosition = CameraMount_LeftShoulder.transform;


            currentProgress += TransitionFract * Time.deltaTime;
            coMountPoint.position = Vector3.Lerp(PlayerCamera_.transform.position,    //From - Position
                                                            CameraNewPosition.transform.position,    //Desired/to - Postion
                                                            Time.deltaTime * TransitionFract);          // Interpolate from and to over time(CPU Clock Ticks)
            PlayerCamera_.transform.position = coMountPoint.position;
            yield return null;
        }
    }
    private void switchCameraViews()
    {
        ///
        ///TODO: Add input Key Bindings with Specific Titles (ie. View_Switch) instead of KeyCode.W but, this works for now.
        ///   
        // Bind PC(IBM Compatible) KeyCode for switching Camera Position to that of the Parented mounting points.

        if (Input.GetKeyDown(KeyCode.Tab))
        {

      
            if (ThirdPersonActive == true)
            {
               
                ThirdPersonView(); //// TEST  /////////
                return;
            }
            if (FirstPersonActive == true)
            {

                FirstPersonView();
                return;
            }
            if (TopDownActive == true)
            {
                // ThirdPersonActive = false;
                // FirstPersonActive = true;
                // TopDownActive = false;
                // RightCamActive = false;
                // LeftCamActive = false;
                // CameraFromPosition = SOMEVECTOR_TRANSFORM_sOURCE
                // CameraNewPosition = SOMEVECTOR_TRANSFORM_TARGET
                // PlayerCamera_.transform.position = Vector3.Lerp(PlayerCamera_.transform.position,    //From - Position
                //                                                 CameraNewPosition.transform.position,    //Desired/to - Postion
                //                                                Time.deltaTime * TransitionFract);          // Interpolate from and to over time(CPU Clock Ticks)
                // return; //Stop from executing the next condition. which will be true! just needs to turn it on for the next Request
            }
            if (RightCamActive == true)
            {
             
                RightShoulderCamView();
                return;
            }
            if (LeftCamActive == true)
            {
      
                LeftShoulderCamView();
                return;
            }
        
        }
    }
}
