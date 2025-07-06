using UnityEngine;

public class InGameUI : Singleton<InGameUI> {
    public GameObject bossHealthBarCanvasPrefab;

    GameplayUI ui = null;
    BossHealthBar bossHealthBar = null;

    private void OnDestroy() {
        Destroy(gameObject);
    }

    public static void EnableBossHealthBar(Entity target) {
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
