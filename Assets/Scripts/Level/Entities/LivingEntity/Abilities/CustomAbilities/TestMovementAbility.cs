using System.Collections;
using UnityEngine;

public class TestMovementAbility : InstantCastAbility {
    private Rigidbody rb;
    private bool isDashing = false;
    private float dashStartTime;
    private Vector3 dashStartPosition;
    private Vector3 dashEndPosition;

    public float dashDistance = 5f;
    public float dashTime = 0.5f;
    public float dashCooldown = 2f;

    public TestMovementAbility(LivingEntity owner, Ability.ID abilityID, Ability.Binding abilityBinding)
        : base(owner, abilityID, abilityBinding) {
        rb = owner.GetComponent<Rigidbody>();
    }

    protected override void CastAbility() {
        base.CastAbility();

        isDashing = true;
        dashStartTime = Time.time;
        dashStartPosition = owner.transform.position;
        dashEndPosition = dashStartPosition + owner.transform.forward * dashDistance;
    }

    public override void Update() {
        base.Update();

        if (isDashing) {
            float elapsedTime = Time.time - dashStartTime;
            float t = Mathf.Clamp01(elapsedTime / dashTime);
            rb.MovePosition(Vector3.Lerp(dashStartPosition, dashEndPosition, t));

            if (t >= 1.0f) {
                isDashing = false;
                rb.velocity = Vector3.zero; // Reset velocity after dashing
                owner.transform.position = dashEndPosition; // Ensure the player reaches the end position
                dashStartTime = Time.time + dashCooldown; // Set cooldown start time
            }
        }
    }
}
