using UnityEngine;

public class Gun : MonoBehaviour {
    public static Transform shells;

    public int id;
    public int num_shells;
    public Rigidbody2D parent;
    public GameObject bullet;
    public ShellController shellController;
    public Sprite gun_sprite;
    public bool full = false;
    public int shells_used;

    void Start() {
        shellController = GameObject.Find("Shells").GetComponent<ShellController>();
    }

    public bool Shoot() {
        if (shells_used < num_shells) {
            if (spawnBullets()) {
                SoundManager.Play("shoot");
                shells_used++;
                if (parent.transform == GameManager.gameManager.player)
                    shellController.SetState(num_shells - shells_used, true);
                if (shells_used == num_shells)
                    full = true;
                return true;
            }
            if (parent.transform == GameManager.gameManager.player)
                SoundManager.Play("shot_failed");
        } else {
            SendMessageUpwards("ReleaseGun");
        }
        return false;
    }

    protected virtual bool spawnBullets() {
        return false;
    }
}
