using UnityEngine;

public class RangedAttribute : AttributeBase {
    readonly float minValue;
    readonly float maxValue;

    public RangedAttribute(string name, float defaultValue, float minValue, float maxValue)
        : base(name, defaultValue) {
        this.minValue = minValue;
        this.maxValue = maxValue;
    }

    public float GetMinValue() {
        return minValue;
    }

    public float GetMaxValue() {
        return maxValue;
    }

    public override float CleanupValue(float value) {
        return Mathf.Clamp(value, minValue, maxValue);
    }
}
