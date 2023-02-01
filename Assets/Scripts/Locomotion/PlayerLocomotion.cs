public class PlayerLocomotion : Locomotion {
    protected override void Start() {
        base.Start();

        //  TODO: [Rock]: Figure out why this was added here again...It doesn't really seem to do anything?
        agent.updateRotation = false;
    }
}
