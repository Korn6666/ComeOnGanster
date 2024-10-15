using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Boost : MonoBehaviour
{
    [SerializeField] MovementManager movementManager;
    [SerializeField] Rigidbody rb;
    public bool isBoosting;
    private bool isCooldown;
    [SerializeField] private float boostMultiplier;
    [SerializeField] private float maxBoost;
    [SerializeField] private float speedBoostReload;
    [SerializeField] private float speedBoostConsume;
    [SerializeField] private float boostCooldown;

    private float currentSpeed;
    private float boostValue;
    [SerializeField] private float boostSpeed;
    [SerializeField] private Slider boostSlider;

    [SerializeField] private GameObject boostParticles;
    private void Start()
    {
        boostSlider.maxValue = maxBoost;
        boostValue = maxBoost;
        isCooldown = false;
    }
    private void Update()
    {
        //UI
        boostSlider.value = boostValue;
        //Effets
        if (isBoosting) {
            boostParticles.SetActive(true);
        }

        else {
            boostParticles.SetActive(false);
        }
           
        
    
        //Calculs
        currentSpeed = Vector3.Dot(rb.velocity, transform.forward);
        if (movementManager.boost && boostValue > 0 && !isCooldown)
        {
            isBoosting = true;
            boostValue -= Time.deltaTime * speedBoostConsume;
            Debug.Log("boost");
        }else if (boostValue < 0)
        {
            StartCoroutine(BoostCoolDown());
            if (boostValue < maxBoost)
            {
                boostValue += Time.deltaTime * speedBoostReload;
            }
            isBoosting = false;
        }else
        {
            if (boostValue < maxBoost)
            {
                boostValue += Time.deltaTime * speedBoostReload;
            }
            isBoosting = false;
        }

        if (isBoosting)
        {
            rb.velocity += boostSpeed * transform.forward*Time.deltaTime;
        }
    }

    IEnumerator BoostCoolDown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(boostCooldown);
        isCooldown = false;
    }

    public void BoostReset()
    {
        Debug.Log("reset boost");
        boostSlider.maxValue = maxBoost;
        boostSlider.value = maxBoost;
        boostValue = maxBoost;
        isCooldown = false;
    }
}
