using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    private float floatForce = 70.0f, bounceForce = 40.0f;
    private float gravityModifier = 1.0f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound, bounceSound;
    private float effectVolume = 1.0f;

    private float topBound = 15.79f;
    private float downwardForce = 3.0f;


    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();

        // Apply a small upward force at the start of the game
        MoveUp();

       

    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and player is low enough, float up
        if (Input.GetKeyDown(KeyCode.Space) && !gameOver)
        {
            MoveUp();
        }
        //At or greater than top boundary, descend
        if(transform.position.y >= topBound)
        {
            
            transform.position = new Vector3(transform.position.x, topBound, transform.position.z);
            
            playerRb.AddForce(Vector3.down * downwardForce, ForceMode.Impulse);
        }
        
    }

    private void OnCollisionEnter(Collision other)
    {
        //Upon colliding with ground, bounce up with sound effect
        if (other.gameObject.CompareTag("Ground") && !gameOver)
        {
            playerRb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
            playerAudio.PlayOneShot(bounceSound, effectVolume);
        }

        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, effectVolume);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, effectVolume);
            Destroy(other.gameObject);

        }

        

    }

    private void MoveUp()
    {
        
            playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);
        
    }
}
