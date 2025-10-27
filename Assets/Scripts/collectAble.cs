using UnityEngine;

public class collectAble : MonoBehaviour
{
    private string whichCollectAble;
    void Start()
    {
        whichCollectAble = this.name.Substring(0, 4);
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.name);
        if (collider.name == "fatMan(Clone)" && whichCollectAble == "chee")
        {
            pickUpCollectAble(collider);
        }
        else if (collider.name == "skeleton(Clone)" && whichCollectAble == "milk")
        {
            pickUpCollectAble(collider);
        }
    }

    private void pickUpCollectAble(Collider collider)
    {
        Debug.Log("picked up " + whichCollectAble + " by " + collider.name);
        PlayerController player = collider.GetComponent<PlayerController>();
        player.collectAbleCount++;
        Destroy(this.gameObject);
    }

}
