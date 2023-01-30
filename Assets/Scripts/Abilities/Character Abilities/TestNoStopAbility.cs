using RockUtils.GameEvents;

public class TestNoStopAbility : AbilityBase {
    public override void Setup(Entity owner, AbilityNum abilityNum) {
        base.Setup(owner, abilityNum);

        triggerType = TriggerType.Toggle;
    }

    protected override string GetName() { return "TestNoStopAbility"; }

    protected override void RegisterEvents() {
        base.RegisterEvents();

        EventManager.StartListening((int) GameEvents.Ability_Press + (int) abilityID, AttemptUseAbility);
    }

    protected override void UnregisterEvents() {
        base.UnregisterEvents();

        EventManager.StopListening((int) GameEvents.Ability_Press + (int) abilityID, AttemptUseAbility);
    }
}
