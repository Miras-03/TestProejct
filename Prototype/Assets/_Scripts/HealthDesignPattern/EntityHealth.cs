using System.Collections.Generic;

public sealed class EntityHealth
{
    private HashSet<IHealthObserver> observers = new HashSet<IHealthObserver>();

    public void CheckOrAdd(IHealthObserver observer)
    {
        if (!observers.Contains(observer))
            observers.Add(observer);
    }

    public void CheckOrRemove(IHealthObserver observer)
    {
        if (observers.Contains(observer))
            observers.Remove(observer);
    }

    public void NotifyObserversAboutDamage()
    {
        foreach (var observer in observers)
            observer.ExecuteDamageOrDie();
    }
}