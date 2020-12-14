namespace mtti.Funcs.Gameplay
{
    /// <summary>
    /// Special level load listener that will only work in MonoBehaviours
    /// on the same GameObject as the Level component.
    /// </summary>
    public interface ILevelLoadListener
    {
        void OnLevelLoad();

        void OnLevelWillUnload();
    }
}
