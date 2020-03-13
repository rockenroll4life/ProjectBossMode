public class StatModifier {
    public enum ModifierType {
        Additive,
        Multiplicative
    }

    public ModifierType modifierType { get; private set; }
    public float value { get; private set; }

    public StatModifier(ModifierType modifierType, float value) {
        this.modifierType = modifierType;
        this.value = value;
    }
}
