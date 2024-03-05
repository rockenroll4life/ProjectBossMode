using UnityEngine;
using UnityEngine.UI;
using RockUtils.GameEvents;

public class BossHealthBar : MonoBehaviour {
    public TMPro.TextMeshProUGUI name;
    public Image fillBar;
    
    LivingEntity target = null;

    float currentHealth, maxHealth;

    public void Setup(LivingEntity target) {
        EventManager.StartListening(target.GetEntityID(), GameEvents.Health_Changed, HealthChanged);

        this.target = target;

        name.text = target.name;

        currentHealth = target.GetResource(ResourceType.Health);
        maxHealth = target.GetAttribute(AttributeTypes.HealthMax).GetValue();
    }

    void HealthChanged(int param) {
        currentHealth = (param / 1000f);

        fillBar.fillAmount = (currentHealth / maxHealth);
    }
}
