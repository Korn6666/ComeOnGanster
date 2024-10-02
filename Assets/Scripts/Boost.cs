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
    [SerializeField] private Slider boostSlider;
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

        //Calculs
        currentSpeed = Vector3.Dot(rb.velocity, transform.forward);
        if (movementManager.boost && boostValue > 0 && !isCooldown)
        {
            isBoosting = true;
            boostValue -= Time.deltaTime * speedBoostConsume;
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
            rb.velocity += currentSpeed * boostMultiplier * transform.forward*Time.deltaTime;
        }
    }

    IEnumerator BoostCoolDown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(boostCooldown);
        isCooldown = false;
    }

}
