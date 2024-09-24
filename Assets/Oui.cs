using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Oui : MonoBehaviour
{   
    [SerializeField] private float maxSpeed;
    [SerializeField] private float derapageSpeed;
    private bool derape;
    [SerializeField] Rigidbody rb;

    private Gamepad gamepad;
    private float acceleration;
    private float deceleration;
    private float teta;
    private float omega;
    [SerializeField] private float omegafactor;
    private Vector3 rotation;
    [SerializeField] private float speedAcceleration;
    [SerializeField] private float adherence;
    [SerializeField] private float OriginalAdherence;
    [SerializeField] private bool groundTestRaycast;
    [SerializeField] private float maxteta;

    private float auSol;


    private Vector3 direction;
    private Vector3 directionInput;
    private Vector3 inputMovement;

    [SerializeField] private float frottement = -10;


    void Start (){
        gamepad = Gamepad.current;
    }



    void Update()
    {
        // V
        //rb.velocity += (acceleration - deceleration) * speedAcceleration * (transform.forward + directionInput);
        //if adh�rence
        int layerMask = 1 << 6;
        groundTestRaycast = Physics.Raycast(transform.position, -Vector2.up, 2, ~layerMask);
        if (groundTestRaycast)
        {
            auSol = 1;
            if (!derape){
                adherence = OriginalAdherence;
            }
        }
        else {
            adherence =0;
            auSol =0;
        }


        Move();

    }

    private void Move()
    {
        omega = omegafactor*rb.velocity.magnitude * Mathf.Sin(teta * Mathf.Deg2Rad) * inputMovement.magnitude  / (2 * Mathf.PI * transform.localScale.z); // 3 = longueur voiture
        
        rb.angularVelocity = new Vector3(rb.angularVelocity.x,omega*adherence + rb.angularVelocity.y*(1- adherence),0   );
        Debug.Log(rb.angularVelocity.y);
        Vector3 eulerRotation = transform.rotation.eulerAngles; 
        transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 0);

        float motorAccel = (acceleration - deceleration) * speedAcceleration*Time.deltaTime;
        Vector3 composantMoteurForward = (motorAccel)* transform.forward;
        Vector3 composantForward = Vector3.Dot(rb.velocity, transform.forward) * transform.forward;
        Vector3 composantTang = Vector3.Dot(rb.velocity, transform.right)* transform.right;
        Vector3 composantUp = Vector3.Dot(rb.velocity,transform.up) * transform.up;
        Vector3 Frottements = (rb.velocity)*(auSol* frottement *Time.deltaTime);


        rb.velocity =  composantForward + adherence*composantMoteurForward + (1-adherence)*(composantTang + Frottements) + composantUp;

        //rb.velocity = (Vector3.Dot(rb.velocity, transform.forward) + (acceleration - deceleration) * speedAcceleration) * transform.forward + composantUp;

        //Debug.Log("velocit� = " +rb.velocity);

        //rb.velocity += (acceleration - deceleration) * speedAcceleration * transform.forward;

        // W
        
        // rotation = new Vector3(0, omega * Mathf.Rad2Deg, 0);
        // //Debug.Log("rotation =  " + rotation);
        // transform.Rotate(rotation);
        //}
        

    }

    //SImon est beau
    public void GetTeta(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();
        direction = inputMovement;

        float angle = Mathf.Deg2Rad*Vector3.Angle(transform.forward, inputMovement);
        Debug.Log(Vector3.Angle(transform.forward, inputMovement));
    }

    public void Rotation(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();
        directionInput = new Vector3(inputMovement.x, 0, inputMovement.y);
        //Debug.Log( "joystick = " +inputMovement);

        teta = Vector3.SignedAngle(Vector3.forward, directionInput, Vector3.up);
        
        if (teta > maxteta)
            teta = maxteta;
        else if (teta < -maxteta)
            teta = -maxteta;
        //Debug.Log( "teta = " +teta);
    }


    public void Acceleration(InputAction.CallbackContext context)
    {
        acceleration = context.ReadValue<float>();

        if (derape && acceleration > 0) {
            derape = false;
            adherence = OriginalAdherence;
        }
        //Debug.Log("a = "  + acceleration);
    }

    public void Deceleration(InputAction.CallbackContext context)
    {
        deceleration = context.ReadValue<float>();
        if (context.started && rb.velocity.magnitude > derapageSpeed){
            Debug.Log("derape");
            adherence = OriginalAdherence/2;
            derape = true;
        }

        if (context.canceled) {
            derape = false;
            adherence = OriginalAdherence;
        }
        //Debug.Log( "d = " +deceleration);
    }



    //context.duration marche po

    // public void Quit(InputAction.CallbackContext context){
    //     Debug.Log("Quitting");
    //     if (context.duration > 1){
    //         Application.Quit();
    //         Debug.Log("quitOMG");
    //     }
    // }
}
