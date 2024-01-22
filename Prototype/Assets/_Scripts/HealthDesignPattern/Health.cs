using System.Collections.Generic;

public sealed class Health
{
    private HashSet<IHealthObserver> observers = new HashSet<IHealthObserver>();

    public void Add(IHealthObserver observer) => observers.Add(observer);


    public void RemoveAll() => observers.Clear();

    public void NotifyObserver(int healthTaken)
    {
        foreach (var observer in observers)
            observer.Execute(healthTaken);
    }
}