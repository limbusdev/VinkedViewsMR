public interface IGPObserver<T>
{
    void OnDispose(T observable);

    void Notify(T observable);
}
