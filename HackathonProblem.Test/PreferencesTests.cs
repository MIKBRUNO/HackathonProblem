using HackathonProblem.TeamBuilding;

namespace HackathonProblem.Test;

public abstract class APreferencesTests
{
    protected abstract IPreferencesFactory<object> Factory { get; }

    [Fact]
    public void Enumerate_ResultMustBeSameAsSequenceAtConstructor()
    {
        IEnumerable<object> sequence = [1, 2, 3, 4];
        var preferences = Factory.CreatePreferences(0, sequence);
        Assert.True(sequence.SequenceEqual(preferences.Enumerate()));
    }

    [Fact]
    public void Compare_InputIsPairsOfPreferences_ResultMustBeMorePreferable()
    {
        IList<object> sequence = [1, 2, 3, 4];
        var preferences = Factory.CreatePreferences(0, sequence);
        for (int i = 0; i < sequence.Count; ++i)
        {
            for (int j = i; j < sequence.Count; ++j)
            {
                Assert.Equal(sequence[i], preferences.Compare(sequence[i], sequence[j]));
                Assert.Equal(sequence[i], preferences.Compare(sequence[j], sequence[i]));
            }
        }
    }

    [Fact]
    public void GetRating_InputIsElementOfSequence_ResultMustBeEqualToCountForFirstTo1ForLast()
    {
        IEnumerable<object> sequence = [1, 2, 3, 4];
        var preferences = Factory.CreatePreferences(0, sequence);
        int rating = sequence.Count();
        foreach (var o in sequence)
        {
            Assert.Equal(rating, preferences.GetRating(o));
            --rating;
        }
    }
}

public class PreferencesTests : APreferencesTests
{
    protected override IPreferencesFactory<object> Factory => new PreferencesFactory<object>();
}
