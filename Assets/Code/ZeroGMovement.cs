using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ZeroGMovement : MonoBehaviour
{

    [Header("=== PLAYER MOVEMENT SETTINGS ===")]
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

    public float maxPitchAngle = 60f; // Maximum pitch angle before player tilts
    public float pitchTorque = 100f;
    private Camera MainCam;


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
        MainCam = Camera.main;
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

        // Calculate Pitch
        rb.AddTorque(MainCam.transform.right * -pitchYaw.y * pitchTorque * Time.fixedDeltaTime);

        // Calculate Yaw
        rb.AddTorque(MainCam.transform.up * pitchYaw.x * pitchTorque * Time.fixedDeltaTime);

        //calculate Roll
        rb.AddTorque(-MainCam.transform.forward * roll1D * rollTorque * Time.fixedDeltaTime);

        //calc FORWARD
        if (thrust1D > 0.1f || thrust1D < -0.1f)
        {
            float currenthrust = thrust;
            rb.AddForce(MainCam.transform.forward * thrust1D * currenthrust * Time.fixedDeltaTime);
            glide = thrust;
        }
        else
        {
            rb.AddForce(MainCam.transform.forward * glide * Time.fixedDeltaTime);
            glide *= thrustGlideReduction;
        }

        //calc STRAFE

        if (strafe1D > 0.1f || strafe1D < -0.1f)
        {
            float currenthrust = thrust;
            rb.AddForce(MainCam.transform.right * strafe1D * currenthrust * Time.fixedDeltaTime);
            horizontalGlide = strafe1D * strafeThrust;
        }
        else
        {
            rb.AddForce(MainCam.transform.right * horizontalGlide * Time.fixedDeltaTime);
            horizontalGlide *= leftRightGlideReduction;
        }

        //calc UPDOWN

        if (upDown1D > 0.1f || upDown1D < -0.1f)
        {
            float currenthrust = thrust;
            rb.AddForce(MainCam.transform.up * upDown1D * currenthrust * Time.fixedDeltaTime);
            verticalGlide = upDown1D * upThrust;
        }
        else
        {
            rb.AddForce(MainCam.transform.up * verticalGlide * Time.fixedDeltaTime);
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
        upDown1D = context.ReadValue<float>();
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
