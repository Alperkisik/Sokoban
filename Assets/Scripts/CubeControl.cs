using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CubeControl : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textMeshpro;

    [SerializeField]
    ParticleSystem particleSystem;

    public bool Collided = false;
    public int CubePositionX, CubePositionY;
    public bool movementRestricted = false;
    public bool victoryTile;

    public int movementCount = 4;
    // Start is called before the first frame update
    void Start()
    {
        particleSystem.Stop();
        textMeshpro.text = movementCount.ToString();
        if (movementRestricted == false) textMeshpro.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Moved()
    {
        bool ableToMove = true;
        movementCount -= 1;
        textMeshpro.text = movementCount.ToString();
        if (movementCount < 0)
        {
            movementCount = 0;
            ableToMove = false;
        }
        else
        {
            ableToMove = true;
        }

        if (movementRestricted) return ableToMove; else return true;
    }

    public void playParticles()
    {
        particleSystem.Play();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == gameObject.tag)
        {
            Collided = true;
        }
    }
}
