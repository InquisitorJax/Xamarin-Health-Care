using System;

namespace Core
{
    public interface IView
    {
        IViewModel ViewModel { get; set; }
    }
}