using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimReceiver : MonoBehaviour
{
    PlayerController playerController;

    public void Start()
    {
        playerController = Utilities.GetRootComponent<PlayerController>();
    }

    public void DashComplete()
    {
        if (playerController)
        {
            playerController.DashComplete();
        }
    }
}
