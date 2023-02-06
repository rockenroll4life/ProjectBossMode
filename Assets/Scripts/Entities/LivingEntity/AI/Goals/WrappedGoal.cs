public class WrappedGoal : Goal {
    Goal goal;
    int priority;
    bool isRunning;

    public WrappedGoal(int priority, Goal goal) {
        this.priority = priority;
        this.goal = goal;
    }

    public Goal GetGoal() { return goal; }
    public int GetPriority() { return priority; }
    public bool IsRunning() { return isRunning; }

    public override bool CanUse() {
        return goal.CanUse();
    }

    public override bool CanContinueToUse() {
        return goal.CanContinueToUse();
    }

    public override bool IsInteruptable() {
        return goal.IsInteruptable();
    }

    public override void Start() {
        if (isRunning) {
            return;
        }

        isRunning = true;
        goal.Start();
    }

    public override void Stop() {
        if (!isRunning) {
            return;
        }

        isRunning = false;
        goal.Stop();
    }

    public override void Update() {
        goal.Update();
    }

    public override bool Equals(object obj) {
        if (this == obj) {
            return true;
        }

        if (obj == null || GetType() != obj.GetType()) {
            return false;
        }

        return goal.Equals(((WrappedGoal) goal).goal);
    }

    public override int GetHashCode() {
        return goal.GetHashCode();
    }
}
