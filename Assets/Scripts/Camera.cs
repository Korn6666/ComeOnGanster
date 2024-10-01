using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    public Transform target;    // La voiture (ou l'objet que tu veux suivre)
    public Vector3 offset;      // D�calage de la cam�ra par rapport � la voiture
    public float smoothSpeed = 0.125f;  // Vitesse de transition liss�e
    private void Start()
    {
        offset = transform.position  - target.position;
    }

    void FixedUpdate()
    {
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
