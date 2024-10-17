using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //Voiture
    [SerializeField] private Rigidbody rb;

    //Moteur 
    [SerializeField] private float maxSpeed;
    [SerializeField] private float derapageSpeed;
    [SerializeField] private float pertesMoteurCoef;
    [SerializeField] private float speedAcceleration;
    private float acceleration;
    private float deceleration;
    
    // Tourner
    private float omega;
    [SerializeField] private float omegafactor;
    private Vector3 rotation;
    private float teta;
    private Vector3 inputMovement;
    //Adherence
    private float adherence;

    void Update()
    {
        Rotation();
        Motor();
    }

    private void Motor()
    {
        float motorAccel = (acceleration - deceleration) * speedAcceleration*Time.deltaTime;

        Vector3 composantMoteurForward = (motorAccel)* transform.forward;
        Vector3 composantForward = Vector3.Dot(rb.velocity, transform.forward) * transform.forward;
        Vector3 composantTang = Vector3.Dot(rb.velocity, transform.right)* transform.right;
        Vector3 composantUp = Vector3.Dot(rb.velocity,transform.up) * transform.up;

        //Vector3 Frottements = Vector3.Dot(rb.velocity, transform.forward)*(auSol* frottement *Time.deltaTime)* rb.velocity.normalized;
        Vector3 pertesMoteur = Vector3.Dot(rb.velocity, -transform.forward)*pertesMoteurCoef * Time.deltaTime* transform.forward;

        //rb.velocity = composantForward + adherence*(composantMoteurForward-pertesMoteur) + (1-adherence)*(composantTang + Frottements) + composantUp;
        rb.velocity = composantForward + composantMoteurForward-pertesMoteur + composantUp;
        //Bridage la vitesse
        if (rb.velocity.magnitude > maxSpeed){
            rb.velocity = rb.velocity.normalized*maxSpeed;
        }
    }

    private void Rotation(){
        omega = omegafactor*rb.velocity.magnitude * Mathf.Sin(teta * Mathf.Deg2Rad) * inputMovement.magnitude  / (2 * Mathf.PI * transform.localScale.z);
        if (Vector3.Dot(rb.velocity, transform.forward)<0){
            omega = -omega;
        }
        rb.angularVelocity = new Vector3(rb.angularVelocity.x, omega, rb.angularVelocity.z);
        //rb.angularVelocity = new Vector3(rb.angularVelocity.x, omega*adherence + rb.angularVelocity.y*(1- adherence), rb.angularVelocity.z)

        //Bloque rotation en z
        Vector3 eulerRotation = transform.rotation.eulerAngles; 
        transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 0);
    }
}
