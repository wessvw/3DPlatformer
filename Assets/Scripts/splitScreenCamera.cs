using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class SplitScreenCamera : MonoBehaviour
{
    Camera cam;
    int index;
    int totalPlayers;

    private void Awake()
    {
        PlayerInputManager.instance.onPlayerJoined += HandlePlayerJoined;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        index = GetComponentInParent<PlayerInput>().playerIndex;
        totalPlayers = PlayerInput.all.Count;
        cam = GetComponent<Camera>();
        cam.depth = index;
        SetupCamera();
    }

    private void HandlePlayerJoined(PlayerInput obj)
    {
        Debug.Log($"Updating Cam for Player {index}");

        totalPlayers = PlayerInput.all.Count;
        SetupCamera();
    }

    private void SetupCamera()
    {
        if (totalPlayers == 1)
        {
            cam.rect = new Rect(0, 0, 1, 1);
        }
        else if (totalPlayers == 2)
        {
            float x, y, width, height;
            width = 0.5f;
            height = 1f;
            y = 0f;

            if (index == 0)
            {
                x = 0f;
            }
            else
            {
                x = 0.5f;
            }

            cam.rect = new Rect(x, y, width, height);
        }
        else
        {
            float x, y, width, height;
            width = 0.5f;
            height = 0.5f;

            if (index < 2)
            {
                y = 0.5f;
            }
            else
            {
                y = 0f;
            }

            if (index % 2 == 0)
            {
                x = 0f;
            }
            else
            {
                x = 0.5f;
            }

            cam.rect = new Rect(x, y, width, height);
        }
    }
}
