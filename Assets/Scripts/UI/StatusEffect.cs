using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : MonoBehaviour {
    public enum ApplicationType {
        Unique_DurationPriority,
        Unique_LevelPriority,
        Stacking,
    }

    public System.Guid effectID { get; protected set; }
    
    //  A duration of -1 means we don't update any duration timer as it last until it gets removed
    public float duration { get; protected set; } = -1;

    public int level { get; protected set; } = 1;

    public bool displayedInUI { get; protected set; } = true;

    //  Whether or not this effect should be the only one of it's effect ID (I.E. should not stack)
    public ApplicationType applicationType { get; protected set; } = ApplicationType.Unique_LevelPriority;

    protected Entity owner = null;

    protected bool needsToUpdate = false;

    //  When this Status Effect is given to an Entity, do stuff
    public virtual void OnApply(Entity owner) {
        this.owner = owner;
        owner.OnStatusEffectApplied(this);
    }

    //  When this Status Effect is removed from an Entity, do stuff
    public virtual void OnRemove() {
        owner.OnStatusEffectRemoved(this);
    }

    //  If this status effect needs to do anything actively
    public virtual bool OnUpdate() {
        return needsToUpdate;
    }
}
