using UnityEngine;

public class GameplayUI : MonoBehaviour {
    public ResourceBars resourceBars;
    public AbilityButtons abilityButtons;

    public void Setup(LivingEntity owner) {
        abilityButtons.Setup(owner);
        resourceBars.Setup(owner, abilityButtons.ResourceValueChanged);
    }
}
