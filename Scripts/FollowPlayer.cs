using UnityEngine;



public class FollowPlayer : MonoBehaviour
{
    private float amountUp;

    private float amountForward;

    private float amountTimesPressedF1 = 0;

    public GameObject playerTwo;

    public GameObject playerToFollow;

    private void Start()
    {
        amountUp = 3f;
        amountForward = 6f;
    }

    public void LateUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Camera.main.fieldOfView /= 2;

            DartScript.zoomed = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            Camera.main.fieldOfView *= 2;

            DartScript.zoomed = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Camera.main.fieldOfView /= 1.3f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Camera.main.fieldOfView *= 1.3f;
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            amountTimesPressedF1++;
            if (amountTimesPressedF1 % 2 != 0)
            {
                amountForward = 0f;
                amountUp = 0f;
            } 
            if (amountTimesPressedF1 % 2 == 0)
            {
                amountForward = 1f;
                amountUp = 1.5f;
            } 
            else if (amountTimesPressedF1 % 3 == 0)
            {
                amountForward = 3f;
                amountUp = 1f;
                amountTimesPressedF1 = 0;
            }
        }

        transform.rotation = playerToFollow.transform.rotation;//Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(90, playerTwo.transform.forward) * playerTwo.transform.rotation, 3 * Time.deltaTime);
        //transform.rotation = Quaternion.AngleAxis(90, playerTwo.transform.forward) * playerTwo.transform.rotation;
        transform.position = playerTwo.transform.position + amountUp * transform.up - amountForward * transform.forward;
    }
}
