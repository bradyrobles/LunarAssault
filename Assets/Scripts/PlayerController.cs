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
    [SerializeField] float yRange = 5f;


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
        float xThrow = movement.ReadValue<Vector2>().x;
        float yThrow = movement.ReadValue<Vector2>().y;


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