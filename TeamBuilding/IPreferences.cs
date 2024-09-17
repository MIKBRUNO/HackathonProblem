namespace TeamBuilding;

public interface IPreferences<T>
{
    T Owner { get; }

    /// <summary>
    /// Returns enumeration of objects in preference order
    /// </summary>
    /// <returns>IEnumerable of objects in preference order</returns>
    IEnumerable<T> Enumerate();

    /// <summary>
    /// Calculates rating of preference
    /// </summary>
    /// <param name="preference">object to get rating of</param>
    /// <returns>rating</returns>
    int GetRating(T preference);
    
    /// <summary>
    /// Compares provided objects and returns most preffered one
    /// </summary>
    /// <param name="left">left operand</param>
    /// <param name="right">right operand</param>
    /// <returns>Most preffered object</returns>
    T Compare(T left, T right);
}
