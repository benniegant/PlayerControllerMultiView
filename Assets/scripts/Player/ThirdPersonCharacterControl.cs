using UnityEngine;
//Automaticly add rigidbody to the gameobject
[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonCharacterControl : MonoBehaviour
{
    public float _speed = 6;
    public float _jumpForce = 6;
    public float castDistanceToGround = 5f;
    public bool onGround = false;
    private Rigidbody _rig;
    private Vector2 _input;
    private Vector3 _movementVector;
    private void Start()
    {
        if (this.GetComponent<Rigidbody>())
        {
            _rig = this.GetComponent<Rigidbody>();
        }
        else
        {

            gameObject.AddComponent<Rigidbody>();
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX  | RigidbodyConstraints.FreezeRotationZ;
            this.GetComponent<Rigidbody>().useGravity = true;
            _rig = this.GetComponent<Rigidbody>();
        }


        if (_rig == null)
            Debug.LogError("RigidBody could not be found.");
        
        //Need to freez rotation so the player do not flip over
        _rig.freezeRotation = true;
    }
    private void Update()
    {
        onGround = IsGrounded();
        //Cleanerway to get input
        _input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            _rig.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }
    private void FixedUpdate()
    {
     
        //Keep the movement vector aligned with the player rotation
        _movementVector = _input.x * transform.right * _speed + _input.y * transform.forward * _speed;
        //Apply the movement vector to the rigidbody without effecting gravity
        _rig.velocity = new Vector3(_movementVector.x, _rig.velocity.y, _movementVector.z);
    }
    private bool IsGrounded()
    {
        //Simple way to check for ground
        Debug.DrawLine(transform.position,new Vector3(transform.position.x,  castDistanceToGround - transform.position.y, transform.position.y),Color.red);
        if (Physics.Raycast(transform.position, -Vector3.up, castDistanceToGround + 0.1f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}