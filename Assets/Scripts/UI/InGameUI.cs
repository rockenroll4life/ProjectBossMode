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

    public GameObject bossHealthBarCanvasPrefab;

    GameplayUI ui = null;
    BossHealthBar bossHealthBar = null;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Debug.LogWarning("Duplicate instance of InGameUI found. Destroying this instance.");
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        Destroy(gameObject);
    }

    public static void EnableBossHealthBar(LivingEntity target) {
        GameObject obj = Instantiate(Instance.bossHealthBarCanvasPrefab, Instance.transform);
        Instance.bossHealthBar = obj.GetComponent<BossHealthBar>();
        Instance.bossHealthBar.Setup(target);
    }

    public static void DisableBossHealthBar() {
        Destroy(Instance.bossHealthBar.gameObject);
        Instance.bossHealthBar = null;
    }

    public static void EnablePlayerUI(GameObject playerUIPrefab, Player uiOwner) {
        Instance.ui = Instantiate(playerUIPrefab, Instance.transform).GetComponent<GameplayUI>();
        Instance.ui.Setup(uiOwner);
    }

    public static void DisablePlayerUI() {
        Destroy(Instance.ui.gameObject);
    }
}
