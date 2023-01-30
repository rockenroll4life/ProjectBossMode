using RockUtils.GameEvents;

public class TestAbility : AbilityBase {
    public override void Setup(Entity owner, AbilityNum abilityNum) {
        base.Setup(owner, abilityNum);

        interruptsMovement = true;
    }

    protected override string GetName() { return "TestAbility"; }

    protected override float GetCooldownTime() { return 5; }

    protected override void RegisterEvents() {
        base.RegisterEvents();

        EventManager.StartListening((int) GameEvents.Ability_Press + (int) abilityID, AttemptUseAbility);
    }

    protected override void UnregisterEvents() {
        base.UnregisterEvents();

        EventManager.StopListening((int) GameEvents.Ability_Press + (int) abilityID, AttemptUseAbility);
    }
}
