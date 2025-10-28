using UnityEngine;

public class collectableCollisionDetector : MonoBehaviour
{
    private collectAble parent;
    void Start()
    {
        parent = GetComponentInParent<collectAble>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.name);
        if (collider.name == "fatMan(Clone)" && parent.whichCollectAble == "chee")
        {
            parent.pickUpCollectAble(collider);
        }
        else if (collider.name == "skeleton(Clone)" && parent.whichCollectAble == "milk")
        {
            parent.pickUpCollectAble(collider);
        }
    }
}
