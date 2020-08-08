using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed;
    public float deadZone;
    public float PPU;
    public Transform realCamera;

    Transform player;
    Rigidbody2D rb;
    Vector3 new_loc = Vector3.zero;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        new_loc.z = realCamera.position.z;
    }

    void LateUpdate() {
        Vector2 delta = player.position - transform.position;
        if (delta.magnitude > deadZone) {
            rb.velocity = delta * speed;
        } else {
            rb.velocity /= 1.05f;
        }
        new_loc.x = Mathf.Round(transform.position.x * PPU) / PPU;
        new_loc.y = Mathf.Round(transform.position.y * PPU) / PPU;
        realCamera.position = new_loc;
    }
}
