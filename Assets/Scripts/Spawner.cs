using UnityEngine;

public class Spawner : MonoBehaviour {
    public void Spawn() {
        if (transform.childCount == 0) return;
        Transform child = transform.GetChild(0);
        child.parent = null;
        child.gameObject.SetActive(true);
        SoundManager.Play("spawn");
        SoundManager.intensity++;
        Destroy(gameObject);
    }
}
