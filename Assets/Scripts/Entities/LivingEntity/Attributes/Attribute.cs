public interface Attribute {
    string GetName();

    float GetDefaultValue();

    float CleanupValue(float value);
}
