using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [Header("Rotation")] 
    public Vector2 inputMovement;
    [SerializeField] private float maxteta;
    public Vector3 directionInput;
    public float teta;

    [Header("Acceleration")]
    public float acceleration;
    public float deceleration;
    [Header("Boost")]
    public float boost;
    
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

        // if (derape && acceleration > 0) {
        //     derape = false;
        //     adherence = OriginalAdherence;
        // }
    }

    public void Deceleration(InputAction.CallbackContext context)
    {
        deceleration = context.ReadValue<float>();
        // if (context.started && rb.velocity.magnitude > derapageSpeed){
        //     //Debug.Log("derape");
        //     adherence = OriginalAdherence/2;
        //     derape = true;
        // }

        // if (context.canceled) {
        //     derape = false;
        //     adherence = OriginalAdherence;
        // }
        //Debug.Log( "d = " +deceleration);
    }

    public void Boost(InputAction.CallbackContext context){
        boost = context.ReadValue<float>();
        
        Debug.Log(boost);
    }
}
