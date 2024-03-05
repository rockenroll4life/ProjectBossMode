using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : MonoBehaviour {
    static InGameUI instance;
    public static InGameUI Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<InGameUI>();
            }
            return instance;
        }
    }

    public GameObject bossHealthBarPrefab;

    BossHealthBar bossHealthBar;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Debug.LogWarning("Duplicate instance of InGameUI found. Destroying this instance.");
            Destroy(gameObject);
        }
    }

    public static void EnableBossHealthBar(LivingEntity target) {
        GameObject obj = Instantiate(Instance.bossHealthBarPrefab, Instance.transform);
        Instance.bossHealthBar = obj.GetComponent<BossHealthBar>();
        Instance.bossHealthBar.Setup(target);
    }
}
