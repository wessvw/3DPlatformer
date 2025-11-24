using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private string keyID;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<FatManController>(out FatManController fController))
        {
            fController.inventory.Add(keyID);
        }
        else if (collider.TryGetComponent<SkeletonController>(out SkeletonController sController))
        {
            sController.inventory.Add(keyID);
        }
        Destroy(this.gameObject);
    }
}
