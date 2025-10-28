using UnityEngine;

public class collectAble : MonoBehaviour
{
    public string whichCollectAble;
    private GameObject collectableModel;
    private ParticleSystem particleSystemToPlay;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip;
    void Start()
    {
        whichCollectAble = this.name.Substring(0, 4);
        collectableModel = this.transform.GetChild(0).gameObject;
        if (this.transform.GetChild(1) != null)
        {
            particleSystemToPlay =  this.transform.GetChild(1).GetComponent<ParticleSystem>();
        }
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

    public void pickUpCollectAble(Collider collider)
    {
        particleSystemToPlay.Play();
        Debug.Log("picked up " + whichCollectAble + " by " + collider.name);
        PlayerController player = collider.GetComponent<PlayerController>();
        player.collectAbleCount++;
        player.updateCountInCanvas();
        Destroy(collectableModel);
        source.PlayOneShot(clip);
        Destroy(this.gameObject, 5f);
    }

}
