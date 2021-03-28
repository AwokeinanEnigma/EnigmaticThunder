using System;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable 
public class EnigmaticList<T> : List<T>
{
    public delegate void OnRemoval(T objectRemoved);
    public delegate void OnAddition(T objectRemoved);

    public event OnRemoval removal;
    public event OnAddition addition;

    new public void Add(T addition)
    {
        base.Add(addition);
        OnAddition _event = this.addition;
        if (_event != null)
        {
            _event.Invoke(addition);
        }
    }

    new public void Remove(T removal)
    {
        base.Remove(removal);
        OnRemoval _event = this.removal;
        if (_event != null)
        {
            _event.Invoke(removal);
        }
    }
}

