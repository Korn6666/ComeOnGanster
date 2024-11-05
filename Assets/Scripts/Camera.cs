using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{   
    [SerializeField] private Transform cameraPosRTS;
    [SerializeField] private Transform cameraPosTPS;

    private bool RTS;
    public Transform target;    // La voiture (ou l'objet que tu veux suivre)
    public Vector3 offset;      // D�calage de la cam�ra par rapport � la voiture
    public float smoothSpeed = 0.125f;  // Vitesse de transition liss�e
    private void Start()
    {
        offset = transform.position  - target.position;
    }

    void FixedUpdate()
    {   
        CamBehv();
        
    }

    public void ChangeCameraMode(){
        if (RTS){
            RTS = false;
            transform.position = cameraPosRTS.position;
            offset = cameraPosRTS.position - target.position;
            
        }
        else {
            RTS = true;
            transform.position = cameraPosTPS.position;
            offset = cameraPosTPS.position - target.position;
            
        }

    }


    private void CamBehv(){

        // Position souhait�e de la cam�ra
        Vector3 desiredPosition = target.TransformPoint(offset);

        // Interpolation de la position pour rendre le mouvement fluide
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Mise � jour de la position de la cam�ra
        transform.position = new Vector3(smoothedPosition.x, offset.y + target.position.y, smoothedPosition.z) ;

        // Orientation de la cam�ra vers l'objet suivi
        transform.LookAt(target);


    }
}
