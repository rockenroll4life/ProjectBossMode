public class PlayerLocomotion : Locomotion {
    public PlayerLocomotion(LivingEntity owner)
        : base(owner) {
        agent.updateRotation = false;
    }
}
