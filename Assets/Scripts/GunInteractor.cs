using UnityEngine;
using System.Collections;
using System.Linq;

public class GunInteractor : MonoBehaviour {

    public float speed;
    public int refCount;
    public Gun gun;

    protected Rigidbody2D rb;
    Transform arms;
    protected SpriteRenderer gun_sprite;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        arms = transform.GetChild(0);
        gun_sprite = arms.GetChild(0).GetComponent<SpriteRenderer>();

        StartVirtual();
    }

    protected virtual void StartVirtual() {

    }

    public IEnumerator Recoil() {
        SoundManager.Play("reload");
        Vector3 pos = new Vector3(0, 0.4f, 0);
        arms.localPosition = pos;
        float t = 0;
        while (pos.y != 0.6f) {
            pos = Vector3.Lerp(0.4f * Vector3.up, 0.6f * Vector3.up, t);
            arms.localPosition = pos + 0.1f * Vector3.forward;
            yield return new WaitForSecondsRealtime(1f/60f);
            t += 1f/10;
        }
    }

    public void PickupGun(GunEntity gunEntity) {
        SoundManager.intensity++;
        gunEntity.sprite.enabled = false;
        gunEntity.collision.enabled = false;
        gunEntity.transform.SetParent(transform);
        gunEntity.transform.localEulerAngles = Vector3.zero;
        gunEntity.transform.localPosition = Vector3.zero;

        if (gun != null) {
            ReleaseGun();
        }

        gun = gunEntity.gun;
        gun.parent = rb;
        gun_sprite.sprite = gun.gun_sprite;
        PickupGunVirtual(gunEntity);
    }

    protected virtual void PickupGunVirtual(GunEntity gunEntity) {

    }

    public void ReleaseGun() {
        SoundManager.intensity--;
        GunEntity gunEntity = gun.GetComponent<GunEntity>();
        gunEntity.sprite.enabled = true;
        gunEntity.collision.enabled = true;
        gunEntity.transform.SetParent(null);
        if (gun.full)
            gunEntity.Disable();

        gun = null;
        gun_sprite.sprite = null;
        ReleaseGunVirtual();
    }

    protected virtual void ReleaseGunVirtual() {

    }
}
