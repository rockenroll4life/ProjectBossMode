using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : Entity {
   public override EntityType GetEntityType() {
        return EntityType.LivingEntity;
    }

    public override TargetingManager.TargetType GetTargetType() {
        return TargetingManager.TargetType.LivingEntity;
    }
}
