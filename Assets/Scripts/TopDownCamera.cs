using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    GameObject player;
    public float height , distance , angle;
    public bool playerFound= false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerFound) handInCamera(); else CheckForPlayer();
    }

    void CheckForPlayer()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null) 
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerFound = true;
        }
        else
        {
            playerFound = false;
        }
    }

    void handInCamera()
    {
        if (!player) CheckForPlayer();

        Vector3 worldPosition = (Vector3.forward * -distance) + (Vector3.up) * height;
        Debug.DrawLine(player.transform.position,worldPosition,Color.red);

        Vector3 rotateVector = Quaternion.AngleAxis(angle, Vector3.up) * worldPosition;
        Debug.DrawLine(player.transform.position, rotateVector, Color.green);

        Vector3 flatTargetPosition = player.transform.position;
        flatTargetPosition.y = 1f;

        Vector3 finalTargetPosition = flatTargetPosition + rotateVector;
        Debug.DrawLine(player.transform.position, finalTargetPosition, Color.blue);

        transform.position = finalTargetPosition;
        transform.LookAt(player.transform.position);
    }
}
