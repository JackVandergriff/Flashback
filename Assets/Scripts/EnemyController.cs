using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : GunInteractor {
    public float targetTolerance;
    public float prediction;
    public float shotTolerance;
    public bool initiatePickup;

    public Vector2 targetPos;
    public Vector2 targetVelocity;
    float targetAngle;
    Transform player;
    Transform tracking;
    Rigidbody2D player_rb;
    int shotCooldown;
    public int AI_state; 
    // 0: tracking player, 1: tracking gun, 2: approaching target, 3: tracking shot

    override protected void StartVirtual() {
        player = GameManager.gameManager.player;
        UpdateTargetGun();
        AI_state = UpdateTargetRandom(tracking, true) ? 2 : 1;
        initiatePickup = true;
        player_rb = player.GetComponent<Rigidbody2D>();
    }

    void Update() {
        transform.up = deltaTo(player);
    }

    void LateUpdate() {
        Vector2 delta = targetPos - (Vector2)transform.position;

        if (Random.value < 0.001f && AI_state != 3) { // Failsafe for if enemy gets stuck to re-target
            AI_state = 0;
            initiatePickup = false;
            UpdateTargetRandom(player);
        }

        switch (AI_state) {
        case 0:
            if (delta.magnitude > targetTolerance) {
                rb.velocity = delta.normalized * speed;
                break;
            }
            if (gun == null) {
                if (UpdateTargetGun()) {
                    AI_state = UpdateTargetRandom(tracking, true) ? 2 : 1;
                    initiatePickup = true;
                    break;
                }
            }
            UpdateTargetRandom(player);
            break;
        case 1:
            if (delta.magnitude > targetTolerance && tracking.root == tracking) {
                rb.velocity = delta.normalized * speed;
                break;
            }
            if (tracking.root == tracking) {
                AI_state = UpdateTargetRandom(tracking, true) ? 2 : 1;
                break;
            }
            UpdateTargetRandom(player);
            AI_state = 0;
            break;
        case 2:
            if (delta.magnitude > targetTolerance && tracking.root == tracking) {
                rb.velocity = delta.normalized * speed;
                break;
            }
            UpdateTargetRandom(player);
            AI_state = 0;
            break;
        case 3:
            if (refCount > 0) {
                rb.velocity = targetVelocity;
            } else {
                AI_state = 0;
            }
            break;
        default:
            break;
        }

        if (shotCooldown > 0) shotCooldown--;

        if (shotCooldown == 0 && gun != null && Random.value < 0.003f && AI_state == 0) { // Start lining up shot
            float bullet_speed = gun.bullet.GetComponent<Bullet>().speed;
            Vector2 pos_at_future = deltaToPlayerAt(prediction);
            float dist = availableDistance(pos_at_future);
            if (dist < 15f) {
                float eta_at_future = (dist - pos_at_future.magnitude) / bullet_speed;
                if (Mathf.Abs(eta_at_future - prediction) < shotTolerance) {
                    for (int i = 1; i <= 4; i++) {
                        targetVelocity = (Quaternion.Euler(0, 0, 90 * i) * pos_at_future).normalized * speed;
                        if (availableDistance(targetVelocity) / speed > prediction + eta_at_future) {
                            rb.velocity = targetVelocity;
                            if (gun != null && gun.Shoot()) {
                                AI_state = 3;
                                shotCooldown = 120;
                                return;
                            }
                        }
                    }
                }
            }
        }
    }

    float availableDistance(Vector2 dir) {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 15f, LayerMask.GetMask("Wall", "Spawner"));
        if (hit.collider != null) {
            return hit.distance;
        }
        return 15f;
    }

    bool UpdateTargetRandom(Transform target, bool exactApproach = false) {
        if (exactApproach && availableDistance(deltaTo(target)) > deltaTo(target).magnitude) {
            targetPos = target.position;
            return true;
        }
        targetAngle = (Random.value - 0.5f) * 2;
        targetAngle *= targetAngle * targetAngle;
        targetAngle *= 180f;
        Vector2 targetDir = (Quaternion.Euler(0, 0, targetAngle) * deltaTo(target).normalized);
        targetPos = Random.value * availableDistance(targetDir) * targetDir + (Vector2) transform.position;
        return false;
    }

    bool UpdateTargetGun() {
        GameObject[] guns = GameObject.FindGameObjectsWithTag("GunEntity");
        if (guns.Length == 0) {
            return false;
        }
        float min_distance = Mathf.Infinity;
        GameObject chosen = null;
        foreach (var gunObject in guns) {
            if ((gunObject.transform.position - transform.position).magnitude < min_distance && !gunObject.GetComponent<Gun>().full) {
                chosen = gunObject;
                min_distance = (gunObject.transform.position - transform.position).magnitude;
            }
        }
        if (chosen == null) return false;
        tracking = chosen.transform;
        return true;
    }

    Vector2 deltaTo(Transform target) {
        return target.position - transform.position;
    }

    Vector2 deltaToPlayerAt(float time) {
        return player.position + (Vector3)player_rb.velocity * time - transform.position;
    }
}