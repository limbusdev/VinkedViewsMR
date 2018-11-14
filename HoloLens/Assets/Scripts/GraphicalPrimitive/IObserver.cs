public interface IObserver<T>
{
    /// <summary>
    /// Observe the given observable
    /// </summary>
    /// <param name="observable"></param>
    void Observe(T observable);

    /// <summary>
    /// Unsubscribe from the given observable
    /// </summary>
    /// <param name="observable"></param>
    void Ignore(T observable);

    /// <summary>
    /// What to do, when the given observable will be destroyed
    /// </summary>
    /// <param name="observable"></param>
    void OnDispose(T observable);

    /// <summary>
    /// What to do, when the given observable has changed
    /// </summary>
    /// <param name="observable"></param>
    void OnChange(T observable);
}
