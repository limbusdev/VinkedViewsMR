public interface IGPObserver<T>
{
    void OnDispose(T observable);
    void Observe(T observable);
    void Notify(T observable);
}
