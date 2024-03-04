using UnityEngine;

public class GameplayUI : MonoBehaviour {
    private Player owner;

    public ResourceBars resourceBars;
    public AbilityButtons abilityButtons;
    
    //  TODO: [Rock]: Support rebinding keys
    readonly KeyCode[] defaultKeybindings = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5 };

    public void Setup(Player owner) {
        this.owner = owner;

        abilityButtons.Setup(owner, defaultKeybindings);
        resourceBars.Setup(owner, abilityButtons.ResourceValueChanged);
    }

    public void Breakdown() {
        abilityButtons.Breakdown();
        resourceBars.Breakdown();
    }
}
