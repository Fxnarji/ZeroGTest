using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grappling : MonoBehaviour
{
    [Header("wdwadsdwadsadwadwdsadwad")]
    private Camera mainCamera;
    public GameObject objectToSpawn;
    public GameObject Player;
    public float maxDistance = 10f;
    private bool isPulling = false;

    private ZeroGMovement playerMovement;

    private void Awake()
    { 
        mainCamera = Camera.main;
        playerMovement = Player.GetComponent<ZeroGMovement>();
    }

    public void OnMouseClick(InputAction.CallbackContext context)
    {
        if (context.performed && !isPulling)
        {
            Debug.Log("MouseClicked");
            Vector3 rayOrigin = mainCamera.transform.position;
            Vector3 rayDirection = mainCamera.transform.forward;

            // Perform raycast in the forward direction
            if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, maxDistance))
            {
                Debug.Log("Ray HIT");
                // Spawn object at hit point
                Instantiate(objectToSpawn, hit.point, Quaternion.identity);
                playerMovement.Pull(hit.point, 2000);
            }
        }
    }
}
