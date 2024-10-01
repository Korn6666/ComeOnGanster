using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class OuiKarel : MonoBehaviour
{   
    [SerializeField] private float maxSpeed;
    [SerializeField] private float derapageSpeed;

    [SerializeField] private float pertesMoteurCoef;
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

    [SerializeField] private float frottement;


    void Start (){
        gamepad = Gamepad.current;
    }



    void Update()
    {

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
        
        rb.angularVelocity = new Vector3(rb.angularVelocity.x, omega*adherence + rb.angularVelocity.y*(1- adherence), 0);
        Vector3 eulerRotation = transform.rotation.eulerAngles; 
        transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 0);

        float motorAccel = (acceleration - deceleration) * speedAcceleration*Time.deltaTime;

        Vector3 composantMoteurForward = (motorAccel)* transform.forward;
        Vector3 composantForward = Vector3.Dot(rb.velocity, transform.forward) * transform.forward;
        Vector3 composantTang = Vector3.Dot(rb.velocity, transform.right)* transform.right;
        Vector3 composantUp = Vector3.Dot(rb.velocity,transform.up) * transform.up;

        Vector3 Frottements = Vector3.Dot(rb.velocity, transform.forward)*(auSol* frottement *Time.deltaTime)* rb.velocity.normalized;
        Vector3 pertesMoteur = Vector3.Dot(rb.velocity, -transform.forward)*pertesMoteurCoef * Time.deltaTime* transform.forward;

        rb.velocity =  composantForward + adherence*(composantMoteurForward-pertesMoteur) + (1-adherence)*(composantTang + Frottements) + composantUp;
        
        if (rb.velocity.magnitude > maxSpeed){
            rb.velocity = rb.velocity.normalized*maxSpeed;
        }
    }

    public void Rotation(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();
        directionInput = new Vector3(inputMovement.x, 0, inputMovement.y); 

        teta = Vector3.SignedAngle(Vector3.forward, directionInput, Vector3.up);
        
        if (teta > maxteta)
            teta = maxteta;
        else if (teta < -maxteta)
            teta = -maxteta;
    }


    public void Acceleration(InputAction.CallbackContext context)
    {
        acceleration = context.ReadValue<float>();

        if (derape && acceleration > 0) {
            derape = false;
            adherence = OriginalAdherence;
        }
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
