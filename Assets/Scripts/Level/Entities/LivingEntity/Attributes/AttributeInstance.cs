public interface AttributeInstance {
    public Attribute GetAttribute();
    
    public float GetBaseValue();

    public void SetBaseValue(float baseValue);

    public float GetValue();

    public void AddModifier(AttributeModifier modifier);

    public void RemoveModifier(AttributeModifier modifier);
}
