using UnityEngine;

public class SpellIndicators {
    public static readonly Color DEFAULT_COLOR = new(0, 0.8196079f, 0.7143784f, 0.25f);
    readonly ConeIndicator cone;
    readonly AOEIndicator aoe;
    readonly AreaTargetIndicator areaTarget;

    public SpellIndicators(Player owner) {
        cone = new ConeIndicator(owner);
        aoe = new AOEIndicator(owner);
        areaTarget = new AreaTargetIndicator(owner);

        ResetIndicators();
    }

    public void Cone(Color color, float radius, float angle) {
        ResetIndicators();
        cone.Setup(color, radius, angle);
    }

    public void AreaTarget(Color color, Color rangeColor, float spellRadius, float rangeRadius) {
        ResetIndicators();
        areaTarget.Setup(color, rangeColor, spellRadius, rangeRadius);
    }

    public void AOE(Color color, float radius) {
        ResetIndicators();
        aoe.Setup(color, radius);
    }

    public void Update() {
        cone.Update();
        aoe.Update();
        areaTarget.Update();
    }

    public void ResetIndicators() {
        cone.Deactivate();
        aoe.Deactivate();
        areaTarget.Deactivate();
    }
}
