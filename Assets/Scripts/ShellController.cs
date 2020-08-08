using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShellController : MonoBehaviour {
    public GameObject shell_prefab;

    List<RectTransform> shells;

    void Start() {
        shells = new List<RectTransform>();
    }

    public void Initialize(Gun gun) {
        if (shells.Count > 0) {
            foreach (var shell in shells) {
                Destroy(shell.gameObject);
            }
            shells.Clear();
        }

        if (gun != null) {
            for (int i = 0; i < gun.num_shells; i++) {
                shells.Add(Instantiate(shell_prefab, Vector3.zero, Quaternion.identity, transform).GetComponent<RectTransform>());
                SetState(i, (gun.num_shells - i) <= gun.shells_used);
                shells[i].anchoredPosition = new Vector2(-110 - 40 * i, 150);
            }
        }
    }

    public void SetState(int shell_index, bool active) {
        Image shell_img = shells[shell_index].GetComponent<Image>();
        if (active) {
            shell_img.color = Color.white;
        } else {
            shell_img.color = new Color(1, 1, 1, 0.2f);
        }
    }
}
