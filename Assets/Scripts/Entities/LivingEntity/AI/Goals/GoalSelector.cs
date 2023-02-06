using System.Collections.Generic;
using System.Linq;

public class GoalSelector {
    readonly HashSet<WrappedGoal> availableGoals = new HashSet<WrappedGoal>();
    
    public void AddGoal(int priority, Goal goal) {
        availableGoals.Add(new WrappedGoal(priority, goal));
    }

    public void RemoveGoal(Goal toRemove) {
        availableGoals.Where(goal => goal.GetGoal() == toRemove).ToList().ForEach(goal => goal.Stop());
        availableGoals.RemoveWhere(goal => goal.GetGoal() == toRemove);
    }
    
    public void Update() {

    }
}
