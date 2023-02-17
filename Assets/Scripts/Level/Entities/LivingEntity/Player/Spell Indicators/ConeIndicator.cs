using UnityEngine;

public class ConeIndicator : IndicatorBase {
    public ConeIndicator(Player owner)
        : base(owner, owner.spellIndicatorPrefabs.Cone) {
        SetActive(false);
    }

    public void Setup(Color color, float radius, float angle) {
        SetActive(true);
        SetColor(color);
        SetRadius(radius);
        SetAngle(angle);
    }

    public void SetAngle(float angle) {
        material.SetFloat("_Angle", angle);
    }

    public override void Update() {
        if (active) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, LAYER_MASK_GROUND)) {
                Vector3 dir = (hit.point - owner.transform.position).normalized;
                Quaternion rot = Quaternion.LookRotation(dir);

                indicatorObject.transform.eulerAngles = new(90, rot.eulerAngles.y, 0);
            }
        }
    }
}
