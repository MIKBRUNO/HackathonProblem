namespace HackathonProblem;

public interface IHRDirector
{
    /// <summary>
    /// Вычисляет показатель удовлетворенности по всем командам
    /// </summary>
    /// <param name="teams">Распределение на команды</param>
    /// <param name="teamleadsWishlists">Вишлисты тимлидов</param>
    /// <param name="juniorWishlists">Вишлисты джунов</param>
    /// <returns>Показатель удовлетворенности</returns>
    double CalculateSatisfaction(IEnumerable<ITeam> teams,
        IEnumerable<IWishlist> teamleadsWishlists, IEnumerable<IWishlist> juniorsWishlists);
}
