using UnityEngine;

public class movingPlatformScript : MonoBehaviour
{
    [SerializeField] float waittime;
    [SerializeField] float speed = 1f;
    private GameObject endPoint;
    private Vector3 startPoint;
    private bool goingBack = false;
    void Start()
    {
        startPoint = this.transform.position;
        endPoint = this.transform.parent.gameObject;
    }

    void Update()
    {
        float step = speed * Time.deltaTime;

        if (this.transform.position != endPoint.transform.position && goingBack == false)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, endPoint.transform.position, step);
        }
        else if (this.transform.position != startPoint && goingBack == true)
        {

            this.transform.position = Vector3.MoveTowards(this.transform.position, startPoint, step);
        }
        else if (this.transform.position == startPoint)
        {
            goingBack = false;
        }
        else if (this.transform.position == endPoint.transform.position)
        {
            goingBack = true;
        }
    }
}
