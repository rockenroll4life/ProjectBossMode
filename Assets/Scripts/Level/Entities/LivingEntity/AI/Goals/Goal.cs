using System.Collections.Generic;

public abstract class Goal {
    readonly HashSet<Flag> flags = new HashSet<Flag>();
    public abstract bool CanUse();
    
    public virtual bool CanContinueToUse() {
        return CanUse();
    }

    public virtual bool IsInteruptable() {
        return true;
    }

    public virtual void Start() { }
    public virtual void Stop() { }

    public virtual void Update() { }

    public void SetFlags(HashSet<Flag> requiredControlFlags) {
        flags.Clear();
        flags.UnionWith(requiredControlFlags);
    }

    public virtual HashSet<Flag> GetFlags() {
        return flags;
    }

    public enum Flag {
        MOVE,
        LOOK,
        JUMP,
        TARGET,
    }
}
