using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public float moveSpeed = 25f;
    public float scrollSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    //todo change so that diagonal movement is smoother/not sped up.
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * Time.deltaTime * moveSpeed;
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.up * Time.deltaTime * moveSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.down * Time.deltaTime * moveSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * Time.deltaTime * moveSpeed;
        }
        if (transform.position.x > 60)
        {
            transform.position = new Vector3(60, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -60)
        {
            transform.position = new Vector3(-60, transform.position.y, transform.position.z);
        }
        if (transform.position.y > 50)
        {
            transform.position = new Vector3(transform.position.x, 50, transform.position.z);
        }
        if (transform.position.y < -50)
        {

            transform.position = new Vector3(transform.position.x, -50, transform.position.z);
        }
        float scrollAmount = scrollSpeed * Input.mouseScrollDelta.y;
        transform.GetComponent<Camera>().orthographicSize -= scrollAmount;
        if (transform.GetComponent<Camera>().orthographicSize < 3.5)
        {
            transform.GetComponent<Camera>().orthographicSize = 3.5f;
        }
        if (transform.GetComponent<Camera>().orthographicSize > 50)
        {
            transform.GetComponent<Camera>().orthographicSize = 50f;
        }
    }
}
