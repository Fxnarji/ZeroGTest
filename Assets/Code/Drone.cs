using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Drone : MonoBehaviour
{
    [Header("=== DRONE MOVEMENT SETTINGS ===")] [SerializeField]
    private float yawTorque = 500f;

    [SerializeField] private float pitchTorque = 1000f;
    [SerializeField] private float rollTorque = 1000f;
    [SerializeField] private float thrust = 100f;
    [SerializeField] private float upThrust = 50;
    [SerializeField] private float strafeThrust = 50f;

    [SerializeField, Range(0.001f, 0.999f)]
    private float thrustGlideReduction = 0.999f;

    [SerializeField, Range(0.001f, 0.999f)]
    private float upDownGlideReduction = 0.111f;

    [SerializeField, Range(0.001f, 0.999f)]
    private float leftRightGlideReduction = 0.111f;


    //input values
    private float thrust1D;
    private float upDown1D;
    private float strafe1D;
    private float roll1D;
    private Vector2 pitchYaw;
    private float glide, verticalGlide, horizontalGlide = 0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        handleMovement();
    }

    void handleMovement()
    {
        //calculate Roll
        rb.AddRelativeTorque(Vector3.back * roll1D * rollTorque * Time.deltaTime);

        //calculate Pitch
        rb.AddRelativeTorque(Vector3.right * Mathf.Clamp(-pitchYaw.y, -1f, 1f) * pitchTorque * Time.deltaTime);

        //calc Yaw
        rb.AddRelativeTorque(Vector3.up * Mathf.Clamp(pitchYaw.x, -1f, 1f) * yawTorque * Time.deltaTime);



        //calc FORWARD

        if (thrust1D > 0.1f || thrust1D < -0.1f)
        {
            float currenthrust = thrust;
            rb.AddRelativeForce(Vector3.forward * thrust1D * currenthrust * Time.deltaTime);
            glide = thrust;
        }
        else
        {
            rb.AddRelativeForce(Vector3.forward * glide * Time.deltaTime);
            glide *= thrustGlideReduction;
        }

        //calc STRAFE

        if (strafe1D > 0.1f || strafe1D < -0.1f)
        {
            float currenthrust = thrust;
            rb.AddRelativeForce(Vector3.right * strafe1D * currenthrust * Time.deltaTime);
            horizontalGlide = strafe1D * strafeThrust;
        }
        else
        {
            rb.AddRelativeForce(Vector3.right * horizontalGlide * Time.deltaTime);
            horizontalGlide *= leftRightGlideReduction;
        }

        //calc UPDOWN

        if (upDown1D > 0.1f || upDown1D < -0.1f)
        {
            float currenthrust = thrust;
            rb.AddRelativeForce(Vector3.up * upDown1D * currenthrust * Time.deltaTime);
            verticalGlide = upDown1D * upThrust;
        }
        else
        {
            rb.AddRelativeForce(Vector3.up * verticalGlide * Time.deltaTime);
            verticalGlide *= upDownGlideReduction;
        }



    }

    #region input values

    public void onThrust(InputAction.CallbackContext context)
    {
        thrust1D = context.ReadValue<float>();
    }

    public void onStrafe(InputAction.CallbackContext context)
    {
        strafe1D = context.ReadValue<float>();
    }

    public void onUpDown(InputAction.CallbackContext context)
    {
        upDown1D= context.ReadValue<float>();
    }

    public void onRoll(InputAction.CallbackContext context)
    {
        roll1D = context.ReadValue<float>();
    }

    public void onPitchYaw(InputAction.CallbackContext context)
    {
        pitchYaw = context.ReadValue<Vector2>();
    }

    #endregion

}
