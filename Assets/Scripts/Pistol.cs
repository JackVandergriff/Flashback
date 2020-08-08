using UnityEngine;

public class Pistol : Gun {
    protected override bool spawnBullets() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 20f, LayerMask.GetMask("Wall", "Spawner"));
        if (hit.collider != null && GameManager.ViableSpawn(bullet.GetComponent<Bullet>().speed, hit.point, parent)) {
            if (hit.collider.tag == "Spawner") hit.collider.GetComponent<Spawner>().Spawn();
            GameManager.Spawn(bullet, hit.point, parent);
            return true;
        }
        return false;
    }
}