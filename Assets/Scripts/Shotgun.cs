using UnityEngine;
using System.Linq;
using System.Collections;

public class Shotgun : Gun {
    protected override bool spawnBullets() {
        RaycastHit2D[] hits = new RaycastHit2D[3];
        hits[0] = Physics2D.Raycast(transform.position, transform.up, 20f, LayerMask.GetMask("Wall", "Spawner"));
        hits[1] = Physics2D.Raycast(transform.position, transform.up + transform.right * 0.1f, 20f, LayerMask.GetMask("Wall", "Spawner"));
        hits[2] = Physics2D.Raycast(transform.position, transform.up - transform.right * 0.1f, 20f, LayerMask.GetMask("Wall", "Spawner"));
        if (hits[0].collider != null && hits[1].collider != null && hits[2].collider != null) {
            float[] etas = new float[3];
            for (int i = 0; i < 3; i++) {
                etas[i] = GameManager.timeTo(bullet.GetComponent<Bullet>().speed, hits[i].point, parent);
            }
            float max_eta = etas.Max();
            if (
                !GameManager.ViableSpawn(bullet.GetComponent<Bullet>().speed, hits[0].point, parent, (max_eta - etas[0]))
                || !GameManager.ViableSpawn(bullet.GetComponent<Bullet>().speed, hits[0].point, parent, (max_eta - etas[0]))
                || !GameManager.ViableSpawn(bullet.GetComponent<Bullet>().speed, hits[0].point, parent, (max_eta - etas[0]))
            ) return false;
            for (int i = 0; i < 3; i++) {
                StartCoroutine(SpawnWithDelay((max_eta - etas[i]), hits[i]));
            }
            return true;
        }
        return false;
    }

    IEnumerator SpawnWithDelay(float delay, RaycastHit2D hit) {
        Rigidbody2D cached_parent = parent;
        yield return new WaitForSecondsRealtime(delay);
        if (hit.collider != null && hit.collider.tag == "Spawner") hit.collider.GetComponent<Spawner>().Spawn();
        GameManager.Spawn(bullet, hit.point, cached_parent);
    }
}