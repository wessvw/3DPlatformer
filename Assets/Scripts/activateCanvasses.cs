using UnityEngine;
using UnityEngine.InputSystem;

public class activateCanvasses : MonoBehaviour
{
    [SerializeField] private Canvas fatManCanvas;
    [SerializeField] private Canvas skeletonCanvas;
    int count = 0;
    void Start()
    {
        fatManCanvas.enabled = false;
        skeletonCanvas.enabled = false;
        PlayerInputManager.instance.onPlayerJoined += enableCanvas;
    }

    void enableCanvas(PlayerInput i)
    {
        if (count == 1)
        {
            skeletonCanvas.enabled = true;
        }
        fatManCanvas.enabled = true;
        count++;
    }
}
