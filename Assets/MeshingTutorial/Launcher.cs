using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{

    public GameObject Projectile;
    public float LaunchForce;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalVariables.CreateCubes == true)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 LaunchDirection = Camera.main.transform.forward + (Vector3.up * .25f);
            var ball = Instantiate(Projectile, Camera.main.transform.position, Quaternion.identity);
            var rb = ball.GetComponent<Rigidbody>();
            rb.AddForce(LaunchDirection * LaunchForce);
        }
    }
}
