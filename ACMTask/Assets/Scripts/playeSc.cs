using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class playeSc : MonoBehaviour
{
    InputAction moveAction;
    InputAction mouseAction;
    Rigidbody rb;
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

    bool gotTrophy=false;
    public GameObject trophyInHand;
    public chatInterface chat;
    // Start is called before the first frame update
    void Start()
    {
        moveAction=InputSystem.actions.FindAction("Move");
        rb=GetComponent<Rigidbody>();
        mouseAction=InputSystem.actions.FindAction("Axis");
        gotTrophy=PlayerPrefs.GetInt("Trophy",0)==1?true:false;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        getTrohpy();
        if(Input.GetKeyDown(KeyCode.Escape)){
            restartLevel();
        }
    }
    void getTrohpy(){
        Collider[] colliders=Physics.OverlapSphere(transform.position,2f);
        trophyInHand.SetActive(gotTrophy);
        foreach(Collider col in colliders){
            if(col.gameObject.name=="Trophy"){
                Debug.Log("Trophy Found");
                if(Input.GetKeyDown(KeyCode.E)){
                    Destroy(col.gameObject);
                    PlayerPrefs.SetInt("Trophy",1);
                    PlayerPrefs.Save();
                    chat.activateChat();
                    Debug.Log("Trophy Collected");
                    chat.setText("Trophy Collected");
                    
                    gotTrophy=true;
                }
                
            }
        }
    }
    void Movement(){
        Vector2 moveValue=moveAction.ReadValue<Vector2>();
        
        rb.velocity=(transform.forward*moveValue.y +transform.right*moveValue.x)*sp;

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

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Ground"){
            Debug.Log("Grounded");
            Grounded=true;
        }
    }
    void restartLevel(){
        SceneManager.LoadScene(0);
    }
    void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag=="Ground"){
            Debug.Log("Grounded");
            Grounded=false;
        }

    }
    void OnTriggerEnter(Collider other)
    {
        if(other.name=="Lava"){
            restartLevel();
        }
        if(other.name=="Obs"){
            restartLevel();
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
