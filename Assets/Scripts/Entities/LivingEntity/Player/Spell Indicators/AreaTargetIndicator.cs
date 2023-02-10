using UnityEngine;

public class AreaTargetIndicator : IndicatorBase {
    static readonly float SCALE_MODIFIER_BASE = 0.3f;
    static readonly float INNER_SCALE_MODIFIER_BASE = 0.15f;

    readonly GameObject areaObject;
    readonly Material areaMaterial;
    float rangeRadius;

    public AreaTargetIndicator(Player owner)
        : base(owner, owner.spellIndicatorAreaTargetPrefab) {

        areaObject = Object.Instantiate(owner.spellIndicatorAreaRangePrefab, owner.transform);
        areaMaterial = areaObject.GetComponent<Renderer>().sharedMaterial;
    }

    public void Setup(Color color, Color rangeColor, float radius, float rangeRadius) {
        this.rangeRadius = rangeRadius;

        SetActive(true);
        SetColor(color);
        SetRadius(radius);

        SetAreaColor(rangeColor);
        SetAreaRadius(rangeRadius);
        SetInnerCircleSize(radius, rangeRadius);
        SetScaleModifier(radius, rangeRadius);
    }

    void SetAreaColor(Color color) {
        areaMaterial.SetColor("_Color", color);
    }

    protected override void SetActive(bool active) {
        base.SetActive(active);
        areaObject.SetActive(active);
    }
    
    public void SetAreaRadius(float radius) {
        float newScale = radius * 2;
        areaObject.transform.localScale = new(newScale, newScale, 1);
    }

    public override void Deactivate() {
        base.Deactivate();
        areaObject.SetActive(false);
    }

    void SetInnerCircleSize(float radius, float rangeRadius) {
        float scaleModifier = INNER_SCALE_MODIFIER_BASE / radius;
        material.SetFloat("_InnerCircleSize", scaleModifier);

        areaMaterial.SetFloat("_InnerCircleSize", 0);
    }

    void SetScaleModifier(float radius, float rangeRadius) {
        float scaleModifier = SCALE_MODIFIER_BASE / radius;
        material.SetFloat("_ScaleModifier", scaleModifier);

        scaleModifier = SCALE_MODIFIER_BASE / rangeRadius;
        areaMaterial.SetFloat("_ScaleModifier", scaleModifier);
    }

    public override void Update() {
        if (active) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, LAYER_MASK_GROUND)) {
                float distance = Mathf.Min(rangeRadius, Vector3.Distance(owner.transform.position, hit.point));
                Vector3 dir = (hit.point - owner.transform.position).normalized;
                Vector3 pos = owner.transform.position + (dir * distance);
                pos.y = 0.01f;
                indicatorObject.transform.position = pos;
            }

            Vector3 ownerPos = owner.transform.position;
            ownerPos.y = 0.01f;
            areaObject.transform.position = ownerPos;
        }
    }
}
