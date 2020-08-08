using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public Transform target;

    float coneOfControl;

    void Start() {
        target.GetComponent<GunInteractor>().refCount++;
    }

    void Update() {
        transform.Translate(0, speed * GameManager.timeWarp * Time.deltaTime, 0);

        if (passedPlayer() && !GameManager.OnScreen(transform.position)) {
            GameManager.LoadScene(0);
        }

        if (target != GameManager.gameManager.player) return;
        Vector3 local = transform.InverseTransformPoint(target.position);
        coneOfControl = local.y - Mathf.Abs(local.x);
        if (coneOfControl < 0) {
            GameManager.effectController.setIntensity(-coneOfControl / 20f);
        } else {
            GameManager.effectController.setIntensity(0);
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.transform == target) {
            GunInteractor target_interactor = target.GetComponent<GunInteractor>();
            target_interactor.StartCoroutine(target_interactor.Recoil());
            target_interactor.refCount--;
            Destroy(gameObject);
        } else {
            SoundManager.Play("loss");
            GameManager.LoadScene(0);
            Destroy(gameObject);
        }
    }

    bool passedPlayer() {
        if (transform.InverseTransformPoint(target.position).y < -2) {
            return true;
        }
        return false;
    }

}