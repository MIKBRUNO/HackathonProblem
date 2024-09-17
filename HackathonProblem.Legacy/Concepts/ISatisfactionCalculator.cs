namespace HackathonProblem.Base.Concepts;

public interface ISatisfactionCalculator
{
    /// <summary>
    /// Вычисляет показатель удовлетворенности по всем командам
    /// </summary>
    /// <param name="teams">Распределение на команды</param>
    /// <param name="teamleadsWishlists">Вишлисты тимлидов</param>
    /// <param name="juniorWishlists">Вишлисты джунов</param>
    /// <returns>Показатель удовлетворенности</returns>
    double CalculateSatisfaction(IEnumerable<Team> teams,
        IEnumerable<Wishlist> teamleadsWishlists, IEnumerable<Wishlist> juniorWishlists);
}
