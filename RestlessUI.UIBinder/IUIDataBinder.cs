namespace RestlessUI
{
    public interface IUIDataBinder<T>
    {
        public void RegisterEvents();
        public void RefreshUI(T data);
        public T BuildObject();
    }
}
