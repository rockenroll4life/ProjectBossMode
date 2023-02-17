public class NoGoal : Goal {
    public override bool CanUse() {
        return false;
    }
}

public class WrappedNoGoal : WrappedGoal {
    public WrappedNoGoal()
        : base(int.MaxValue, new NoGoal()) {
    }

    public override bool IsRunning() {
        return false;
    }
}
