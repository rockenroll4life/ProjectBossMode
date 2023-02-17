using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect {
    public enum Alignment {
        Positive,
        Negative,
        Neutral
    }

    public System.Guid effectID { get; protected set; }
    
    //  A duration of -1 means we don't update any duration timer as it last until it gets removed
    public float duration { get; protected set; } = -1;

    public int level { get; protected set; } = 1;

    public Alignment alignment { get; protected set; } = Alignment.Neutral;

    public bool displayedInUI { get; protected set; } = true;

    public bool markedForRemoval { get; protected set; } = false;

    protected LivingEntity owner = null;

    protected bool needsToUpdate = false;

    //  When this Status Effect is given to an Entity, do stuff
    public virtual void OnApply(LivingEntity owner) {
        this.owner = owner;
        owner.OnStatusEffectApplied(this);
    }

    //  When this Status Effect is removed from an Entity, do stuff
    public virtual void OnRemove() {
        owner.OnStatusEffectRemoved(this);
    }

    //  If this status effect needs to do anything actively
    public virtual bool OnUpdate() {
        if (duration > 0) {
            duration -= Time.deltaTime;

            if (duration <= 0) {
                markedForRemoval = true;
            }
        }

        return needsToUpdate;
    }
}
