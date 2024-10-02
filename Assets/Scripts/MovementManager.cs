using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class MovementManager : MonoBehaviour
{   
    [Header("VOITURE")]
    // voiture
    [SerializeField] Rigidbody rb;

    [Header("MOTEUR")]
    //Moteur 
    [SerializeField] private float maxSpeed;
    [SerializeField] private float pertesMoteurCoef;
    [SerializeField] private float speedAcceleration;

    // Inputs
    private float acceleration;
    private float deceleration;
    private float teta;
    private Vector3 direction;
    private Vector3 directionInput;
    private Vector3 inputMovement;

    [Header("DERAPAGE")]
    // Derapage
    static public bool derape;
    [SerializeField] private float derapageSpeedBoost;
    [SerializeField] private float derapageMinSpeed;
    [SerializeField] private float adhDerapage;
    [SerializeField] private float longDerapageTimer;
    [SerializeField] private GameObject derapageParticles;
    [SerializeField] private List<Transform> listRoues;

    private List<GameObject> listParticles;
    private bool longDerapage;

    [Header("Frottements et adherence")]
    // Frottements
    [SerializeField] private float adherence;
    [SerializeField] private float adhModifier;
    [SerializeField] private float OriginalAdherence;
    [SerializeField] private float frottement;

    [Header("Rotation")]
    // Tourner
    private float omega;
    [SerializeField] private float omegafactor;
    private Vector3 rotation;
    [SerializeField] private float maxteta;

    // Grounded?
    private bool groundTestRaycast;
    
    private float auSol;

    public bool boost;

    void Update()
    {
        int layerMask = 1 << 6;
        groundTestRaycast = Physics.Raycast(transform.position, -Vector2.up, 2, ~layerMask);
        if (groundTestRaycast)
        {
            auSol = 1;
            if (!derape){
                adherence = adhModifier/(adhModifier+rb.angularVelocity.magnitude);
            }
        }
        else {
            adherence =0;
            auSol =0;
        }
        Move();
    }

    // Movement
    private void Move()
    {
        if (derape) {
            //si on veut pimp le dérapage

        }


        // Angle
        omega = omegafactor*rb.velocity.magnitude * Mathf.Sin(teta * Mathf.Deg2Rad) * inputMovement.magnitude  / (2 * Mathf.PI * transform.localScale.z);
        rb.angularVelocity = new Vector3(rb.angularVelocity.x, omega*adherence + rb.angularVelocity.y*(1- adherence), rb.angularVelocity.z);

        // Pas d'histoire de rotation en z et ça évite d'etre bloqué à l'envers 
        Vector3 eulerRotation = transform.rotation.eulerAngles; 
        transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 0);

        // Vitesse
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


        //Debug.Log("frottements:" + Frottements);
        //Debug.Log("adh * pertesmoteur:" + adherence*pertesMoteur);
    }

    // Inputs
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

    }

    public void Deceleration(InputAction.CallbackContext context)
    {
        deceleration = context.ReadValue<float>();
        if (context.started && rb.velocity.magnitude > derapageMinSpeed){
            Debug.Log("derape");
            adherence = adhDerapage;
            derape = true;

            if (auSol == 1){
                foreach (Transform roue in listRoues){

                    roue.GetChild(0).gameObject.SetActive(true);
                    
            }
            }

            StartCoroutine("IsItALongDerapage");
        }

        if (context.canceled) {

            if (longDerapage){
                rb.velocity+= transform.forward*derapageSpeedBoost*auSol;
                longDerapage = false;
            }
            if (derape){
                derape = false;
                foreach (Transform roue in listRoues){
                    roue.GetChild(0).gameObject.SetActive(false);
                  
            }       
            }
            

            adherence = OriginalAdherence;
        }
    }

    public void Boost(InputAction.CallbackContext context)
    {
        boost = context.ReadValue<float>() == 1;
        if (context.started){
            transform.GetChild(0).gameObject.SetActive(true);
        }
        if (context.canceled){
            transform.GetChild(0).gameObject.SetActive(false);
        }

        //Debug.Log(boost);
    }

    private IEnumerator IsItALongDerapage(){
        yield return new WaitForSeconds(longDerapageTimer);
        if (derape){
            longDerapage = true;
        }
        else {
            longDerapage = false;
        }
    }

}
