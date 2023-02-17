using UnityEngine;

public class AOEIndicator : IndicatorBase {
    static readonly float SCALE_MODIFIER_BASE = 0.3f;

    public AOEIndicator(Player owner)
        : base(owner, owner.spellIndicatorPrefabs.AOE) {
    }

    public void Setup(Color color, float radius) {
        SetActive(true);
        SetColor(color);
        SetRadius(radius);

        SetAreaAngle(360);
        SetInnerCircleSize(0);
        SetScaleModifier(radius);
    }

    public void SetAreaAngle(float angle) {
        material.SetFloat("_Angle", angle);
    }

    void SetInnerCircleSize(float size) {
        material.SetFloat("_InnerCircleSize", size);
    }

    void SetScaleModifier(float radius) {
        float scaleModifier = SCALE_MODIFIER_BASE / radius;
        material.SetFloat("_ScaleModifier", scaleModifier);
    }

    public override void Update() { }
}
