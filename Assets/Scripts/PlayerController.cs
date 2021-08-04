using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Parameter Configurations
    [SerializeField] InputAction movement;
    [SerializeField] InputAction fire;
    [SerializeField] float velocityTuner = 10f;
    [SerializeField] float xRange = 5f;
    [SerializeField] float yRange = 3.5f;
    [SerializeField] float positionPitchFactor = -2f;
    [SerializeField] float controlPitchFactor = -10f;
    [SerializeField] float positionYawFactor = 2f;

    [SerializeField] float controlRollFactor = -25f;
    [SerializeField] float pitchSpeed = .1f;

    float xThrow, yThrow;

    float pitch, yaw, roll;


    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnEnable()
    {
        movement.Enable();
        fire.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
        fire.Disable();
    }


    // Update is called once per frame
    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
    }

    private void ProcessRotation()
    {
        Quaternion oldRotation = Quaternion.Euler(pitch, yaw, roll);

        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControl = yThrow * controlPitchFactor;
        float newPitch = pitchDueToPosition + pitchDueToControl;

        float newYaw = transform.localPosition.x * positionYawFactor;
        float newRoll = xThrow * controlRollFactor;

        transform.localRotation = Quaternion.Lerp(oldRotation, Quaternion.Euler(newPitch, newYaw, newRoll), Time.time * pitchSpeed);

        pitch = newPitch;
        yaw = newYaw;
        roll = newRoll;
    }

    private void ProcessTranslation()
    {
        xThrow = movement.ReadValue<Vector2>().x;
        yThrow = movement.ReadValue<Vector2>().y;


        float rawXPos = transform.localPosition.x +
                        CalculateOffset(xThrow);
        float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);

        float rawYPos = transform.localPosition.y +
                        CalculateOffset(yThrow);
        float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3(
            clampedXPos,
            clampedYPos,
            transform.localPosition.z
        );
    }

    private float CalculateOffset(float temp)
    {
        float offset = temp * Time.deltaTime * velocityTuner;
        return offset;
    }

}