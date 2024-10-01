using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrictionManager : MonoBehaviour
{
    public float adherence;
    private Vector3 composantTang;
    [SerializeField] private Rigidbody rb;
    private Vector3 Frictions;
    [SerializeField] private bool groundTestRaycast;
    private float auSol;
    [SerializeField] private float frottement;


    void Update(){
        GroundCheck();


        composantTang = Vector3.Dot(rb.velocity, transform.right)* transform.right;
        Frictions = Vector3.Dot(rb.velocity, transform.forward)*(auSol* frottement *Time.deltaTime)* rb.velocity.normalized;
    }

    void GroundCheck()
    {
        int layerMask = 1 << 6;
        groundTestRaycast = Physics.Raycast(transform.position, -Vector2.up, 2, ~layerMask);
        if (groundTestRaycast)
        {
            auSol = 1;
            // if (!derape){
            //     adherence = OriginalAdherence;
            // }
        }
        else
        {
            adherence = 0;
            auSol = 0;
        }
    }
}
