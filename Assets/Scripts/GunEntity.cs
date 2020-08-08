using UnityEngine;
using System.Collections.Generic;

public class GunEntity : MonoBehaviour {
    public static List<GunEntity> guns = new List<GunEntity>();

    public SpriteRenderer sprite;
    public Collider2D collision;
    public Gun gun;
    public bool active = true;

    bool inList = false;

    void Start() {
        if (!inList) {
            guns.Add(this);
            inList = true;
        }
    }

    void Awake() {
        if (!inList) {
            guns.Add(this);
            inList = true;
        }
    }

    void OnTriggerStay2D(Collider2D collider) {
        if (Input.GetMouseButtonDown(1) && !gun.full && collider.tag == "Player") {
            PlayerController player = collider.GetComponent<PlayerController>();
            player.PickupGun(this);
        } else if (collider.tag == "Enemy") {
            EnemyController enemy = collider.GetComponent<EnemyController>();
            if (enemy.initiatePickup) {
                enemy.initiatePickup = false;
                if (!gun.full)
                    enemy.PickupGun(this);
            }
        }
    }

    public void Disable() {
        sprite.color = Color.red;
        active = false;
        GameManager.TrySucceed(this);
    }
}