using UnityEngine;
using System.Linq;

public class PlayerController : GunInteractor {

    public GunController gunController;
    public ShellController shellController;

    Vector2 targetVelocity;

    void Update() {
        Vector2 delta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        transform.up = delta;

        if(Input.GetMouseButtonDown(0)) {
            if (gun != null) {
                gun.Shoot();
            }
        }
    }

    void LateUpdate() {
        if (refCount == 0) {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            targetVelocity = new Vector2(h, v) * speed * GameManager.timeWarp;
        }
        rb.velocity = targetVelocity;
    }

    override protected void PickupGunVirtual(GunEntity gunEntity) {
        SoundManager.Play("pickup");
        shellController.Initialize(gun);
        StartCoroutine(gunController.SwapGun(gun.gun_sprite));
    }

    override protected void ReleaseGunVirtual() {
        SoundManager.Play("drop");
        StartCoroutine(gunController.SwapGun(null));
        shellController.Initialize(null);
    }
}
