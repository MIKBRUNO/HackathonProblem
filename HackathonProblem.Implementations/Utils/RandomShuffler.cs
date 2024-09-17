namespace HackathonProblem.Implementations.Utils;

public class RandomShuffler<T>(Random random)
{
    private readonly Random random = random;

    public IEnumerable<T> Shuffle(IEnumerable<T> values)
    {
        List<T> list = new(values);
        int length = list.Count;
        List<T> shuffled = [];
        while (length > 0)
        {
            int i = random.Next() % length;
            shuffled.Add(list.ElementAt(i));
            list.RemoveAt(i);
            --length;
        }
        return shuffled;
    }
}
