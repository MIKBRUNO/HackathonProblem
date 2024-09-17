namespace HackathonProblem.Base.Concepts
{
    public interface IHackathon
{
    /// <summary>
    /// Формирует списки предпочтений для джунов и тимлидов
    /// </summary>
    /// <param name="teamleads">Тимлиды</param>
    /// <param name="juniors">Джуны</param>
    /// <param name="teamleadsWishlists">Списки предпочтений тимлидов</param>
    /// <param name="juniorsWishlists">Списки предпочтений джунов</param>
        public void PerformHackathon(in IEnumerable<Employee> teamleads, in IEnumerable<Employee> juniors,
            out IEnumerable<Wishlist> teamleadsWishlists, out IEnumerable<Wishlist> juniorsWishlists);
    }
}

