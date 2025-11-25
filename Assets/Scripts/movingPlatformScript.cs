using UnityEngine;
using UnityEngine.UIElements;

public class movingPlatformScript : MonoBehaviour
{
    [SerializeField] float waittime;
    [SerializeField] float speed = 1f;
    [SerializeField] private bool needsButton;
    private GameObject endPoint;
    private Vector3 startPoint;
    private bool goingBack = false;
    private bool justPressed = false;
    void Start()
    {
        startPoint = this.transform.position;
        endPoint = this.transform.parent.gameObject;
    }

    void Update()
    {
        float step = speed * Time.deltaTime;
        if (!needsButton)
        {
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
        else if (justPressed)
        {
            // Debug.Log("pressedbutton");
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
                justPressed = false;
                goingBack = false;
            }
            else if (this.transform.position == endPoint.transform.position)
            {
                justPressed = false;
                goingBack = true;
            }
        }
    }

    public void goOneWay()
    {
        if (needsButton == false)
        {
            needsButton = true;
        }
        justPressed = true;
    }
}
