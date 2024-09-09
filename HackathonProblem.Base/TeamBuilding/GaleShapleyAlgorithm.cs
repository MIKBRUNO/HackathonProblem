using HackathonProblem.Base.Concepts;

namespace HackathonProblem.Base.TeamBuilding.GaleShapley;

public static class GaleShapleyAlgorithm
{
    /// <summary>
    /// Взят из: https://cpsc.yale.edu/sites/default/files/files/tr407.pdf
    /// Находит оптимальный для мужчин марьяж в решетке всех стабиьных марьяжей
    /// </summary>
    /// <param name="menWishlists">Вишлист мужчин</param>
    /// <param name="womenWishlists">Вичшлист женщин</param>
    /// <returns>Перечисление пар вида (m,w)</returns>
    public static IEnumerable<(int, int)> FindManOptimalMarriage(
        IEnumerable<Wishlist> menWishlists, IEnumerable<Wishlist> womenWishlists)
    {
        int count = menWishlists.Count();
        IList<(int, int)> pairs = [];
        Dictionary<int, Wishlist> dictMenWishlists = menWishlists.ToDictionary(w => w.EmployeeId);
        WomenPreferences womenPreferences = new WomenPreferences(womenWishlists);
        // least indecies of possible passions in men's wishlists
        // firstly they all are set to 0
        Dictionary<int, int> possibleMenPassions = new(count);
        foreach (Wishlist wishlist in menWishlists) {
            possibleMenPassions[wishlist.EmployeeId] = 0;
        }
        Dictionary<int, int> womenFiances = new(count);
        
        IEnumerator<Wishlist> menWishlistsEnumerator = menWishlists.GetEnumerator();
        bool continueCondition = menWishlistsEnumerator.MoveNext();
        if (!continueCondition) return pairs;
        Wishlist manWishlist = menWishlistsEnumerator.Current;
        while (continueCondition)
        {
            int manId = manWishlist.EmployeeId;
            int womanId = manWishlist.DesiredEmployees[possibleMenPassions[manId]];
            // if woman is already engaged compare men and get more preffered one
            if (womenFiances.ContainsKey(womanId))
            {
                int otherManId = womenFiances[womanId];
                int prefferedMan = womenPreferences.WPrefersM1ToM2(womanId, manId, otherManId) ? manId : otherManId;
                int lonelyMan = prefferedMan == manId ? otherManId : manId;
                womenFiances[womanId] = prefferedMan;
                manWishlist = dictMenWishlists[lonelyMan];
                ++possibleMenPassions[lonelyMan];
            }
            else
            {
                pairs.Add((manId, womanId));
                womenFiances[womanId] = manId;
                continueCondition = menWishlistsEnumerator.MoveNext();
                manWishlist = menWishlistsEnumerator.Current;
            }
        }
        return pairs;
    }

    private class WomenPreferences
    {
        /// <summary>
        /// [woman_i, man_j] := man_j's rating for woman_i (order number)
        /// </summary>
        private int[,] preferenceTable;
        private Dictionary<int, int> menIndecies = new();
        private Dictionary<int, int> womenIndecies = new();
        
        public WomenPreferences(IEnumerable<Wishlist> womenWishlists)
        {
            int count = womenWishlists.Count();
            preferenceTable = new int[count, count];
            int man_j = 0;
            foreach (int man in womenWishlists.First().DesiredEmployees)
            {
                menIndecies.Add(man, man_j);
                ++man_j;
            }
            int woman_i = 0;
            foreach (Wishlist wishlist in womenWishlists)
            {
                womenIndecies.Add(wishlist.EmployeeId, woman_i);
                int rate = 0;
                foreach (int manId in wishlist.DesiredEmployees) {
                    preferenceTable[woman_i, menIndecies[manId]] = rate;
                    rate++;
                }

                ++woman_i;
            }
        }

        public bool WPrefersM1ToM2(int wId, int m1Id, int m2Id) {
            return preferenceTable[womenIndecies[wId], menIndecies[m1Id]]
                <
                preferenceTable[womenIndecies[wId], menIndecies[m2Id]];
        }
    }
}
