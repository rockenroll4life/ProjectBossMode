public interface IAttribute {
    string GetName();

    float GetDefaultValue();

    float CleanupValue(float value);
}
