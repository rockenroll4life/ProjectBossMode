using System.Collections.Generic;

public class StatusEffectManager {
    readonly LivingEntity owner;
    
    //  TODO: [Rock]: Consider having this stored as a Dictionary of Dictionaries, where the keys are the effects alignment (Neutral, Positive, Negative) and the values are
    //  dictionaries holding a Guid and status effect...could simplify this some

    //  We are separating these out into 3 categories so we can more easily render these out in the UI in a logical order
    //  Neutral => Positive => Negative
    Dictionary<System.Guid, StatusEffect> neutralStatusEffects = new Dictionary<System.Guid, StatusEffect>();
    Dictionary<System.Guid, StatusEffect> positiveStatusEffects = new Dictionary<System.Guid, StatusEffect>();
    Dictionary<System.Guid, StatusEffect> negativeStatusEffects = new Dictionary<System.Guid, StatusEffect>();
    
    readonly List<System.Guid> toRemove = new List<System.Guid>();

    public StatusEffectManager(LivingEntity owner) {
        this.owner = owner;
    }

    public void AddStatusEffect(StatusEffect effect) {
        effect.OnApply(owner);

        if (effect.alignment == StatusEffect.Alignment.Neutral) {
            neutralStatusEffects[effect.effectID] = effect;
        } else if (effect.alignment == StatusEffect.Alignment.Positive) {
            positiveStatusEffects[effect.effectID] = effect;
        } else {
            negativeStatusEffects[effect.effectID] = effect;
        }
    }

    public void RemoveStatusEffect(StatusEffect effect) {
        if (effect.alignment == StatusEffect.Alignment.Neutral) {
            neutralStatusEffects.Remove(effect.effectID);
        } else if (effect.alignment == StatusEffect.Alignment.Positive) {
            positiveStatusEffects.Remove(effect.effectID);
        } else {
            negativeStatusEffects.Remove(effect.effectID);
        }
    }

    public void ClearStatusEffectsOfType(StatusEffect.Alignment alignment) {
        if (alignment == StatusEffect.Alignment.Neutral) {
            ClearStatusEffectDictionary(ref neutralStatusEffects);
        } else if (alignment == StatusEffect.Alignment.Positive) {
            ClearStatusEffectDictionary(ref positiveStatusEffects);
        } else {
            ClearStatusEffectDictionary(ref negativeStatusEffects);
        }
    }

    void ClearStatusEffectDictionary(ref Dictionary<System.Guid, StatusEffect> effects) {
        foreach (StatusEffect effect in effects.Values) {
            effect.OnRemove();
        }

        effects.Clear();
    }

    public void Update() {
        UpdateDictionary(ref neutralStatusEffects);
        UpdateDictionary(ref positiveStatusEffects);
        UpdateDictionary(ref negativeStatusEffects);
    }

    void UpdateDictionary(ref Dictionary<System.Guid, StatusEffect> effects) { 
        //  Loop through and process our status effects
        foreach (StatusEffect effect in effects.Values) {
            effect.OnUpdate();

            if (effect.markedForRemoval) {
                effect.OnRemove();
                toRemove.Add(effect.effectID);
            }
        }

        //  Clear out any status effects that are marked for removal
        foreach (System.Guid key in toRemove) {
            effects.Remove(key);
        }

        //  Clear out our cleanup list
        toRemove.Clear();
    }
}
