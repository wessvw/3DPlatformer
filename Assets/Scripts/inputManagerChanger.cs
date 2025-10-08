using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class inputManagerChanger : MonoBehaviour
{
    public PlayerInputManager inputManager;
    public GameObject newPlayerPrefab;
    private void Awake()
    {
        inputManager.onPlayerJoined += changePlayerPrefab;
    }

    void changePlayerPrefab(PlayerInput input)
    {
        inputManager.playerPrefab = newPlayerPrefab;
    }
}
