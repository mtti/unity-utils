namespace mtti.Funcs
{
    public interface IPipelineStage<T>
    {
        T Execute(T input);
    }
}
