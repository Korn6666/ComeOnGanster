using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Oui : MonoBehaviour
{   
    [SerializeField] private float maxSpeed;


    [SerializeField] Rigidbody rb;

    private Gamepad gamepad;
    private float acceleration;
    private float deceleration;
    private float teta;
    private float deltaTeta;
    private Vector3 rotation;
    [SerializeField] private float speedAcceleration;



    private Vector3 direction;
    private Vector3 directionInput;
    private Vector3 inputMovement;

    void Start (){
        gamepad = Gamepad.current;
    }



    void Update()
    {
        // V

        //rb.velocity += (acceleration - deceleration) * speedAcceleration * (transform.forward + directionInput);
        //if adhérence
        if (rb.velocity.magnitude > maxSpeed && (acceleration>0 || deceleration >0))
        {
            //rb.velocity = rb.velocity;
        }
        else 
            rb.velocity = (Vector3.Dot(rb.velocity, transform.forward) + (acceleration - deceleration) * speedAcceleration) * transform.forward + Vector3.Dot(rb.velocity, transform.up) * transform.up;
        //Debug.Log("velocité = " +rb.velocity);

        //rb.velocity += (acceleration - deceleration) * speedAcceleration * transform.forward;

        // W
        deltaTeta = rb.velocity.magnitude * Mathf.Pow(Mathf.Sin(teta * Mathf.Deg2Rad), 1) * inputMovement.magnitude * Time.deltaTime / (2 * Mathf.PI * transform.localScale.z); // 3 = longueur voiture
        rotation = new Vector3(0, deltaTeta * Mathf.Rad2Deg, 0);
        //Debug.Log("rotation =  " + rotation);
        transform.parent.Rotate(rotation);
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
        Debug.Log( "joystick = " +inputMovement);

        teta = Vector3.SignedAngle(Vector3.forward, directionInput, Vector3.up);
        //Debug.Log( "teta = " +teta);
    }


    public void Acceleration(InputAction.CallbackContext context)
    {
        acceleration = context.ReadValue<float>();


        //Debug.Log("a = "  + acceleration);
    }

    public void Deceleration(InputAction.CallbackContext context)
    {
        deceleration = context.ReadValue<float>();
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
