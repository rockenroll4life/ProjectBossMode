public abstract class Goal {
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
}
