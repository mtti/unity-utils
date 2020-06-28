using System.Collections.Generic;

namespace mtti.Funcs
{
    public class Pipeline<T>
    {
        private List<IPipelineStage<T>> _steps = new List<IPipelineStage<T>>();

        public T Execute(T input)
        {
            T current = input;
            for (int i = 0, count = _steps.Count; i < count; i++)
            {
                current = _steps[i].Execute(current);
            }
            return current;
        }

        public void AddStage(IPipelineStage<T> stage)
        {
            _steps.Add(stage);
        }
    }
}
