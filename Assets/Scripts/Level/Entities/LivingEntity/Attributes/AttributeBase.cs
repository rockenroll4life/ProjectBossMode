public abstract class AttributeBase : IAttribute {
    readonly string name;
    readonly float defaultValue;

    public AttributeBase(string name, float defaultValue) {
        this.name = name;
        this.defaultValue = defaultValue;
    }

    public float GetDefaultValue() {
        return defaultValue;
    }

    public string GetName() {
        return name;
    }

    public override int GetHashCode() {
        return name.GetHashCode();
    }

    public override bool Equals(object obj) {
        return obj is IAttribute attribute && name.Equals(attribute.GetName());
    }

    public abstract float CleanupValue(float value);
}
