using UnityEngine;

public abstract class IndicatorBase {
    protected static readonly int LAYER_MASK_GROUND = LayerMask.GetMask("Ground");

    protected readonly LivingEntity owner;
    protected readonly GameObject indicatorObject;
    protected readonly Material material;
    protected bool active = false;

    public IndicatorBase(Player owner, GameObject indicatorObject) {
        this.owner = owner;

        this.indicatorObject = Object.Instantiate(indicatorObject, owner.transform);
        material = indicatorObject.GetComponent<Renderer>().sharedMaterial;
    }

    protected virtual void SetActive(bool active) {
        this.active = active;
        indicatorObject.SetActive(active);
    }

    public virtual void Deactivate() {
        SetActive(false);
    }

    public void SetColor(Color color) {
        material.SetColor("_Color", color);
    }

    public void SetRadius(float radius) {
        if (radius < 1) {
            Debug.LogException(new System.Exception("Spell Indicator radius for " + indicatorObject.name + " cannot be < 0"));
        }

        float newScale = radius * 2;
        indicatorObject.transform.localScale = new(newScale, newScale, 1);
    }

    public abstract void Update();
}
