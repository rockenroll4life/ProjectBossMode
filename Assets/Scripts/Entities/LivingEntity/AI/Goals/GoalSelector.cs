using System.Collections.Generic;
using System.Linq;

public class GoalSelector {
    readonly Dictionary<Goal.Flag, WrappedGoal> lockedFlags = new Dictionary<Goal.Flag, WrappedGoal>();
    readonly HashSet<WrappedGoal> availableGoals = new HashSet<WrappedGoal>();
    readonly HashSet<Goal.Flag> disabledFlags = new HashSet<Goal.Flag>();

    readonly WrappedNoGoal NO_GOAL = new WrappedNoGoal();

    public void AddGoal(int priority, Goal goal) {
        availableGoals.Add(new WrappedGoal(priority, goal));
    }

    public void RemoveGoal(Goal toRemove) {
        availableGoals.Where(goal => goal.GetGoal() == toRemove).ToList().ForEach(goal => goal.Stop());
        availableGoals.RemoveWhere(goal => goal.GetGoal() == toRemove);
    }
    
    public void Update() {
        //  Goal Cleanup
        GetRunningGoals()
            .Where(goal => goal.GetFlags().Any(flag => disabledFlags.Contains(flag)) || !goal.CanContinueToUse()).ToList()
            .ForEach(goal => goal.Stop());

        lockedFlags.ToList().ForEach(item => {
            if (!item.Value.IsRunning()) {
                lockedFlags.Remove(item.Key);
            }
        });

        //  Changing Goals
        availableGoals
            .Where(goal => !goal.IsRunning())
            .Where(goal => !goal.GetFlags().ToList().Any(flag => disabledFlags.Contains(flag)))
            .Where(goal => goal.GetFlags().ToList().All(flag => lockedFlags.GetValueOrDefault(flag, NO_GOAL).CanBeReplacedBy(goal)))
            .Where(goal => goal.CanUse())
            .ToList().ForEach(goal => {
                goal.GetFlags().ToList().ForEach(flag => {
                    WrappedGoal currentGoal = lockedFlags.GetValueOrDefault(flag, NO_GOAL);
                    currentGoal.Stop();
                    lockedFlags[flag] = goal;
                });
                goal.Start();
            });

        //  Goal Update
        GetRunningGoals().ToList().ForEach(goal => goal.Update());
    }

    public HashSet<WrappedGoal> GetAvailableGoals() {
        return availableGoals;
    }

    public IEnumerable<WrappedGoal> GetRunningGoals() {
        return availableGoals.Where(goal => goal.IsRunning());
    }
}
