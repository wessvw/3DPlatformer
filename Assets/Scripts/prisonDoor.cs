using UnityEngine;

public class prisonDoor : MonoBehaviour
{
    // north == true | south == false
    [SerializeField] private bool direction;
    [SerializeField] private bool isLocked = false;
    [SerializeField] private GameObject southPoint;
    [SerializeField] private GameObject northPoint;
    [SerializeField] private LayerMask Playerlayer;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private Transform armature;
    [SerializeField] private string doorID;


    private void OnTriggerEnter(Collider collider)
    {
        if (isLocked)
        {
            if (collider.TryGetComponent<FatManController>(out FatManController fController))
            {
                foreach (string i in fController.inventory)
                {
                    if (i == doorID)
                    {
                        checkSide();
                    }
                }
            }
            else if (collider.TryGetComponent<SkeletonController>(out SkeletonController sController))
            {
                foreach (string i in sController.inventory)
                {
                    if (i == doorID)
                    {
                        checkSide();
                    }
                }
            }
        }
        else
        {
            checkSide();
        }
    }

    private void checkSide()
    {
        if (Physics.CheckSphere(southPoint.gameObject.transform.position, 1.5f, Playerlayer) && direction == false)
        {
            Vector3 rotation = new Vector3(0, 0, 90);
            openDoor(rotation);
        }
        else if (Physics.CheckSphere(northPoint.gameObject.transform.position, 1.5f, Playerlayer) && direction == true)
        {
            Vector3 rotation = new Vector3(0, 0, -90);
            openDoor(rotation);
        }
    }

    private void openDoor(Vector3 rotation)
    {
        armature.Rotate(rotation);
        boxCollider.enabled = false;
        this.GetComponent<BoxCollider>().enabled = false;
    }
}
