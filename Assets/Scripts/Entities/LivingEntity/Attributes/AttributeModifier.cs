using System;

public class AttributeModifier
{
    public enum Operation {
        Addition,
        Multiply_Base,
        Multiply_Total,

        _TOTAL
    }

    readonly float amount;
    readonly Operation operation;
    readonly string name;
    Guid id;

    public AttributeModifier(Guid id, string name, float amount, Operation operation) {
        this.id = id;
        this.name = name;
        this.amount = amount;
        this.operation = operation;
    }

    public float GetAmount() { return amount; }
    public Operation GetOperation() { return operation; }
    public string GetName() { return name; }
    public Guid GetID() { return id; }

    public override bool Equals(object obj) {
        if (this == obj) {
            return true;
        }

        if (obj == null || GetType() != obj.GetType()) {
            return false;
        }

        AttributeModifier modifier = (AttributeModifier) obj;
        return Equals(id, modifier.id);
    }

    public override int GetHashCode() {
        return id.GetHashCode();
    }
}
