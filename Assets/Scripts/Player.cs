using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{

    [SyncVar(hook = "OnHolaCountChange")]
    int holaCount = 0;
    void HandleMovement()
    {
        if (isLocalPlayer)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0f);

            transform.position = transform.position + movement * 6f * Time.deltaTime;

        }
    }

    private void Update()
    {
        HandleMovement();

        if (isLocalPlayer && Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Sending Hola to Server.");
            Hola();
        }

        if (isServer && transform.position.y > 5)
        {
            TooHigh();
        }
    }

    [ClientRpc]
    void TooHigh()
    {
        Debug.Log("Too High!");
    }


    [Command]
    void Hola()
    {
        Debug.Log("Recieved Hola from Client");
        ReplyHola();
        holaCount++;
    }

    [TargetRpc]
    void ReplyHola()
    {
        Debug.Log("Recieved Hola from Server");
    }

    void OnHolaCountChange(int oldCount, int newCount)
    {

        Debug.Log($"We had {oldCount} holas, but now we have {newCount} holas");

    }

}


