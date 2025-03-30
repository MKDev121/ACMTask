using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class playeSc : MonoBehaviour
{
    InputAction moveAction;
    InputAction mouseAction;
    CharacterController plController;
    float sp;
    public float speed;
    public float sprintSpeed;
    public float mouseSpeed;
    float xrotation;
    bool Grounded;
    bool crouched;
    public float jumpForce;
    float verticalVelo;
    PlayerState playerState;
    // Start is called before the first frame update
    void Start()
    {
        moveAction=InputSystem.actions.FindAction("Move");
        plController=GetComponent<CharacterController>();
        mouseAction=InputSystem.actions.FindAction("Axis");
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

    }
    void Movement(){
        Vector2 moveValue=moveAction.ReadValue<Vector2>();
        
        transform.position+=(transform.forward*moveValue.y +transform.right*moveValue.x)*sp*Time.deltaTime;

        float mouse_y=Input.GetAxis("Mouse Y");
        float mouse_x=Input.GetAxis("Mouse X");
        xrotation-=mouse_y*mouseSpeed;
        xrotation=Mathf.Clamp(xrotation,-85,85);
        //float mouse_x=mouseAction.ReadValue<Vector2>().x;
        transform.Rotate(0,mouse_x*mouseSpeed,0);
        transform.GetChild(0).localRotation=Quaternion.Euler(xrotation,0,0);
        //transform.GetChild(0).Rotate(mouse_y,0,0);
        if(!Grounded){
            transform.position+=transform.up*verticalVelo*Time.deltaTime;
            verticalVelo-=9.8f*Time.deltaTime;
        }
        if(Grounded && Input.GetKeyDown(KeyCode.Space)&&playerState!=PlayerState.Crouching){
            verticalVelo=jumpForce;
            Grounded=false;
        }
        if(Input.GetKey(KeyCode.LeftShift))
            playerState=PlayerState.Running;
        else if(Input.GetKey(KeyCode.LeftControl))
            playerState=PlayerState.Crouching;
        else
            playerState=PlayerState.Walking;
        
        switch(playerState){
            case PlayerState.Walking:
                sp=speed;
                transform.localScale=new Vector3(1f,1f,1f);
            break;
            case PlayerState.Running:
                sp=sprintSpeed;
                transform.localScale=new Vector3(1f,1f,1f);
            break;
            case PlayerState.Crouching:
                sp=speed/2;
                transform.localScale=new Vector3(1f,0.5f,1f);
                break;
        }
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Hit"+hit.gameObject.name);
    }
    void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag=="Ground"){
            Debug.Log("Grounded");
            Grounded=true;
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag=="Ground"){
            Debug.Log("Grounded");
            Grounded=false;
        }
    }
    enum PlayerState
    {
        Idle,
        Walking,
        Running,
        Crouching,
        Jumping
    }
}
