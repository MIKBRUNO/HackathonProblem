namespace TeamBuilding.Algorithms;

/// <summary>
/// From: https://cpsc.yale.edu/sites/default/files/files/tr407.pdf
/// Finds man optimal marriage
/// </summary>
/// <typeparam name="T">Type to make pairs of</typeparam>
public class GaleShapleyAlgorithm<T> : IMarriageAlgorithm<T> where T: class
{
    public IEnumerable<Pair<T>> BuildMarriage(
        IEnumerable<IPreferences<T>> menPreferences,
        IEnumerable<IPreferences<T>> womenPreferences)
    {
        Dictionary<T, IPreferences<T>> menPreferencesDict =
            menPreferences.ToDictionary(obj => obj.Owner);
        Dictionary<T, IPreferences<T>> womenPreferencesDict =
            womenPreferences.ToDictionary(obj => obj.Owner);
        Dictionary<T, T> womenFiances = [];
        Dictionary<T, IEnumerator<T>> menBestPassions =
            menPreferences.ToDictionary(obj => obj.Owner, obj => obj.Enumerate().GetEnumerator());
        var enumerator = menPreferences.GetEnumerator();
        bool singleMenLeft = enumerator.MoveNext();
        var manPreferences = enumerator.Current;
        while (singleMenLeft)
        {
            T man = manPreferences.Owner;
            if (!menBestPassions[man].MoveNext())
            {
                throw new MarriageNotFoundException("There is a man without pair");
            }
            T woman = menBestPassions[man].Current;
            if (womenFiances.TryGetValue(woman, out T? otherMan))
            {
                T prefferedMan = womenPreferencesDict[woman].Compare(man, otherMan);
                T lonelyMan = man == prefferedMan ? otherMan : man;
                womenFiances[woman] = prefferedMan;
                manPreferences = menPreferencesDict[lonelyMan];
            }
            else
            {
                womenFiances[woman] = man;
                singleMenLeft = enumerator.MoveNext();
                manPreferences = enumerator.Current;
            }
        }
        return womenFiances.Select(pair => new Pair<T>(Woman: pair.Key, Man: pair.Value));
    }
}
