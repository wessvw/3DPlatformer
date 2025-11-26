using TMPro;
using UnityEngine;

public class RewardEndGame : MonoBehaviour
{
    [SerializeField] private GameObject textGameObject;
    void Start()
    {
        textGameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider collider)
    {
        textGameObject.SetActive(true);
    }
}
