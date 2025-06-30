using UnityEngine;

public abstract class GamemodeBase : IGamemode {
    private readonly Level level;
    private readonly WorldEventSystem worldEvents;

    protected Level GetLevel() => level;
    public WorldEventSystem GetWorldEvents() => worldEvents;

    public GamemodeBase(Level level) {
        this.level = level;
        worldEvents = new WorldEventSystem(level);
    }

    public abstract void Setup();
    public abstract void Breakdown();

    protected virtual void RegisterWorldEvents() { }

    //  TODO: [Rock]: Remove the offset once we fix up the Tower prefab
    protected void PopulateNodes(GameplayNode.Type type, GameObject prefab, Vector3 offset) {
        level.GetGameplayNodes().GetAllGameplayNodes(type)
            .ForEach(node => {
                level.SpawnEntity(prefab, node.transform.position + offset, Quaternion.identity);
        });
    }
}
