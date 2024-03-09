using UnityEngine;
using UnityEngine.UI;
using RockUtils.GameEvents;

public class BossHealthBar : MonoBehaviour {
    public TMPro.TextMeshProUGUI entityName;
    public Image barFill;
    
    System.Guid entityID;

    float currentHealth, maxHealth;

    public void Setup(LivingEntity target) {
        EventManager.StartListening(target.GetEntityID(), GameEvents.Entity_Data_Changed + (int) EntityDataType.Health, HealthChanged);

        entityID = target.GetEntityID();
        entityName.text = target.name;

        currentHealth = target.GetEntityData(EntityDataType.Health);
        maxHealth = target.GetAttribute(AttributeTypes.HealthMax).GetValue();
        barFill.fillAmount = (currentHealth / maxHealth);
    }

    private void OnDestroy() {
        EventManager.StopListening(entityID, GameEvents.Entity_Data_Changed + (int) EntityDataType.Health, HealthChanged);
    }

    void HealthChanged(int param) {
        currentHealth = (param / 1000f);
        barFill.fillAmount = (currentHealth / maxHealth);
    }
}
