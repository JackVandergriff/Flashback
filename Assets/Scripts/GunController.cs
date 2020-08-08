using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GunController : MonoBehaviour {
    public RectTransform gun_transform;
    public Sprite placeholder;

    Image image;

    void Start() {
        image = GetComponent<Image>();
    }

    public IEnumerator SwapGun(Sprite sprite) {
        if (image.sprite != null) {
            for (int i = 0; i <= 5; i++) {
                gun_transform.eulerAngles = Vector3.forward * (90 - 5 * i);
                yield return new WaitForSecondsRealtime(1f/60f);
            }
        }

        if (sprite == null) {
            image.sprite = placeholder;
        } else {
            image.sprite = sprite;
        }
        yield return new WaitForSecondsRealtime(1f/60f);

        for (int i = 0; i <= 5; i++) {
            gun_transform.eulerAngles = Vector3.forward * (65 + 5 * i);
            yield return new WaitForSecondsRealtime(1f/60f);
        }
    }
}
