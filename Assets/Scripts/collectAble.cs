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
            particleSystemToPlay = this.transform.GetChild(1).GetComponent<ParticleSystem>();
        }
    }

    public void pickUpCollectAble(Collider collider)
    {
        particleSystemToPlay.Play();
        // Debug.Log("picked up " + whichCollectAble + " by " + collider.name);

        FatManController fatMan = collider.GetComponent<FatManController>();
        SkeletonController skeleton = collider.GetComponent<SkeletonController>();

        if (fatMan != null)
        {
            fatMan.collectAbleCount++;
            fatMan.updateCountInCanvas();
        }
        else if (skeleton != null)
        {
            skeleton.collectAbleCount++;
            skeleton.updateCountInCanvas();
        }
        
        Destroy(collectableModel);
        source.PlayOneShot(clip);
        Destroy(this.gameObject, 5f);
    }

}
