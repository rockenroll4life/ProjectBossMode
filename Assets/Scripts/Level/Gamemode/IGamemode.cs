public interface IGamemode {
    public WorldEventSystem GetWorldEvents();

    public void Setup();
    public void Breakdown();

    public void Update();
}
