using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static float timeWarp = 1;
    public static GameManager gameManager;
    public static EffectController effectController;

    public float warp = 1;
    public Transform player;
    public Animator animator;

    void Start() {
        timeWarp = warp;
        gameManager = this;  
        effectController = GameObject.Find("EffectController").GetComponent<EffectController>();   
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            LoadScene(0);
        }
    }

    public static void Spawn(GameObject toSpawn, Vector3 at, Rigidbody2D towards) {
        GameObject new_bullet = Instantiate(toSpawn, at, Quaternion.identity);
        float eta = timeTo(toSpawn.GetComponent<Bullet>().speed, at, towards);
        new_bullet.transform.up = towards.position + eta * towards.velocity - (Vector2) at;
        new_bullet.GetComponent<Bullet>().target = towards.transform;
    }

    public static bool ViableSpawn(float speed, Vector2 at, Rigidbody2D towards, float delay = 0f) {
        if (delay > 0) {
            towards.position += delay * towards.velocity;
        }
        float eta = timeTo(speed, at, towards);
        Vector2 dir = towards.position + eta * towards.velocity - at;
        if (delay > 0) {
            towards.position -= delay * towards.velocity;
        }
        return Physics2D.Raycast(at + 0.1f * dir, dir, eta * speed, LayerMask.GetMask("Wall", "Spawner")).collider == null;
    }

    public static float timeTo(float obj_speed, Vector3 at, Rigidbody2D towards) {
        float vp = towards.velocity.magnitude;
        float local_angle = Mathf.Atan2(towards.velocity.y, towards.velocity.x);
        Vector2 local = Quaternion.Euler(0, 0, -local_angle * Mathf.Rad2Deg) * (at - (Vector3) towards.position);
        return (vp * local.x - Mathf.Sqrt((obj_speed * obj_speed) * (local.x * local.x + local.y * local.y) - vp * vp * local.y * local.y)) / (vp * vp - obj_speed * obj_speed);
    }

    public static void Fail() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPaused = true;
        #endif
    }

    public static bool OnScreen(Vector3 pos) {
        Vector2 viewport_pos = Camera.main.WorldToViewportPoint(pos);
        return viewport_pos.x >= 0 && viewport_pos.x <= 1 && viewport_pos.y >= 0 && viewport_pos.y <= 1;
    }

    IEnumerator loadLevel(int index, bool relative = true) {
        SoundManager.intensity = 0;
        GunEntity.guns.Clear();
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1.2f);
        if (relative) {
            SceneManager.LoadScene(index + SceneManager.GetActiveScene().buildIndex);
        } else {
            SceneManager.LoadScene(index);
        }
    }

    public static void LoadScene(int index, bool relative = true) {
        gameManager.StartCoroutine(gameManager.loadLevel(index, relative));
    }

    public static void TrySucceed(GunEntity g) {
        foreach (var gunEntity in GunEntity.guns) {
            if (gunEntity.active && gunEntity != g) return;
        }
        if (GameObject.FindGameObjectWithTag("Spawner") != null) return;
        SoundManager.Play("success");
        LoadScene(1);
    }
}