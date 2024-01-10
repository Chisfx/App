namespace App.Application.Interfaces.Shared
{
    /// <summary>
    /// Represents an interface for comparing objects.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.</typeparam>
    public interface ICompareObject
    {
        /// <summary>
        /// Compares two objects of type T.
        /// </summary>
        /// <param name="o1">The first object to compare.</param>
        /// <param name="o2">The second object to compare.</param>
        /// <returns>True if the objects are equal, false otherwise.</returns>
        bool Compare<T>(T o1, T o2);
    }
}
