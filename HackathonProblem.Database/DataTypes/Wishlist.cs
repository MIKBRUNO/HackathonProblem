
using Microsoft.VisualBasic;

namespace HackathonProblem.Database.DataTypes;

public class Rate<T>
    where T : Employee
{
    public int WishlistId { get; set; }
    
    public int Rating { get; set; }

    public T Mate { get; set; }
}

public class Wishlist<O, E> : IWishlist
    where O : Employee
    where E : Employee
{
    public int Id { get; set; }

    public O Owner { get; set; }

    public List<Rate<E>> Ratings { get; set; }

    IEmployee IWishlist.Owner => Owner;

    IEnumerable<IEmployee> IWishlist.DesiredEmployees => Ratings
        .Aggregate(new List<IEmployee>(), (acc, r) => {
            acc.Insert(r.Rating, r.Mate);
            return acc;
        });
}

public class JuniorWishlist : Wishlist<Junior, Teamlead>;

public class TeamleadWishlist : Wishlist<Teamlead, Junior>;

public class WishlistFactory(HackathonContext context) : IWishlistFactory
{
    public IWishlist CreateWishlist(IEmployee Owner, IEnumerable<IEmployee> DesiredEmployees)
    {
        if (Owner is Junior)
        {
            return new JuniorWishlist() {
                Owner = Owner as Junior,
                Ratings = DesiredEmployees
                    .Select((e, i) => new Rate<Teamlead>() { Rating = i, Mate = e as Teamlead })
                    .ToList()
            };
        }
        else
        {
            return new TeamleadWishlist() {
                Owner = Owner as Teamlead,
                Ratings = DesiredEmployees
                    .Select((e, i) => new Rate<Junior>() { Rating = i, Mate = e as Junior })
                    .ToList()
            };
        }
    } 
}
