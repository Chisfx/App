namespace App.Application.Interfaces.Shared
{
    public interface ICompareObject
    {
        bool Compare<T>(T o1, T o2);
    }
}
