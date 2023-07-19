using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testPlayer : MonoBehaviour
{
    private new Rigidbody rigid;

    private float v;
    private float h;
    private float r;

    public float moveSpeed = 8f;
    public float turnSpeed = 0f;
    public float jumpPower = 5f;

    private float turnSpeedValue = 200f;

    RaycastHit hit;
    
    IEnumerator Start()
    {
        rigid = GetComponent<Rigidbody>();
        turnSpeed = 0;
        yield return new WaitForSeconds(0.5f);
        turnSpeed = turnSpeedValue;
    }

    private void Update()
    {
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");
        r = Input.GetAxis("Mouse X");

        if (Input.GetKeyDown("space"))
        {
            if (Physics.Raycast(transform.position, -transform.up, out hit, 0.6f))
            {
                rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 dir = (Vector3.forward * v) + (Vector3.right * h);
        transform.Translate(dir.normalized * Time.deltaTime * moveSpeed, Space.Self);
        transform.Rotate(Vector3.up * Time.smoothDeltaTime * turnSpeed * r);
    }
}
