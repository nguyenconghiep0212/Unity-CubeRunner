using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody rb;
     
    public float jumpForce;
    public bool canJump;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (canJump)
            {
                SoundManager.Instance.mainAudioChannel.PlayOneShot(SoundManager.Instance.jumpSound);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = false;
        }
    }

    public void BreakApart()
    {
        gameObject.SetActive(false);
        foreach (GameObject cell in GameManager.Instance.cells)
        {
            Instantiate(cell, transform.position, Quaternion.identity);
        }
    }
}
