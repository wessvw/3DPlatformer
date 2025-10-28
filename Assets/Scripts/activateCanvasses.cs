using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class activateCanvasses : MonoBehaviour
{
    [SerializeField] private Canvas fatManCanvas;
    [SerializeField] private Canvas skeletonCanvas;
    private Text fatManText;
    private Text skeletonText;
    int count = 0;
    void Start()
    {
        fatManCanvas.enabled = false;
        skeletonCanvas.enabled = false;
        PlayerInputManager.instance.onPlayerJoined += enableCanvas;
        fatManText = fatManCanvas.GetComponentInChildren<Text>();
        skeletonText = skeletonCanvas.GetComponentInChildren<Text>();
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

    public void updateText(int count, string whichPlayer)
    {
        if (whichPlayer == "fatMan(Clone)")
        {
            fatManText.text = count.ToString();
        }
        else if (whichPlayer == "skeleton(Clone)")
        {
            skeletonText.text = count.ToString();
        }
    }
}
