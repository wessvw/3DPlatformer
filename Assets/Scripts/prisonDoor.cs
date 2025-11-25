using UnityEngine;

public class prisonDoor : MonoBehaviour
{
    [Header("Do not touch zone")]
    [SerializeField] private GameObject southPoint;
    [SerializeField] private GameObject northPoint;
    [SerializeField] private LayerMask Playerlayer;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private Transform armature;
    enum direction
    {
        North,
        South,
        Both,
    }
    [Header("Customizeable")]
    [SerializeField] private direction openDirection;
    [SerializeField] private string doorID;
    [SerializeField] private bool isLocked = false;


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
        // it checks at which side the player is at and which direction it can be openned at
        // it also checks which side its at and opens it from that side if the door can be opened from both directions
        if (Physics.CheckSphere(southPoint.gameObject.transform.position, 1.5f, Playerlayer) && openDirection == direction.North || Physics.CheckSphere(southPoint.gameObject.transform.position, 1.5f, Playerlayer) && openDirection == direction.Both)
        {
            Vector3 rotation = new Vector3(0, 0, 90);
            openDoor(rotation);
        }
        else if (Physics.CheckSphere(northPoint.gameObject.transform.position, 1.5f, Playerlayer) && openDirection == direction.South || Physics.CheckSphere(northPoint.gameObject.transform.position, 1.5f, Playerlayer) && openDirection == direction.Both)
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
