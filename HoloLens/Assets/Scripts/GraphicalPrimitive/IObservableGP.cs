using System;

public interface IObservableGP<I,T>
{
    IDisposable Subscribe(IGPObserver<T> observer);
    void Unsubscribe(IGPObserver<T> observer);
}
