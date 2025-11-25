using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] movingPlatformScript platform;
    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("pressedButton");
        platform.goOneWay();
    }
}
