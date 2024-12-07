// using System.Diagnostics.CodeAnalysis;

// namespace HackathonProblem.Database.DataTypes_old;

// public class Teamlead : Employee
// {
//     public Teamlead() : base() {}

//     [SetsRequiredMembers]
//     public Teamlead(int Id, string Name) : base(Id, Name) {}

//     [SetsRequiredMembers]
//     public Teamlead(IEmployee employee) : base(employee) {}
// }

// public class Junior : Employee
// {
//     public Junior() : base() {}

//     [SetsRequiredMembers]
//     public Junior(int Id, string Name) : base(Id, Name) {}

//     [SetsRequiredMembers]
//     public Junior(IEmployee employee) : base(employee) {}
// }

// public class Hackathon
// {
//     public required int Id { get; set; }

//     public required List<TeamleadWishlist> TeamleadWishlists { get; set; }

//     public required List<JuniorWishlist> JuniorWishlists { get; set; }
// }

// public class Rate<T> where T : IEmployee
// {
//     public int? WishlistId { get; set; }
//     public required int Rating { get; set; }
//     public required T Mate { get; set; }
// }

// public class CommonWishlist<OwnerType, MateType> : IWishlist
//     where OwnerType : IEmployee
//     where MateType : IEmployee
// {
//     public int? Id { get; set; }

//     public int? HackathonId { get; set; }

//     public int? OwnerId { get; set; }

//     public required OwnerType Owner { get; set; }

//     public required List<Rate<MateType>> Mates { get; set; }

//     public IEnumerable<IEmployee> DesiredEmployees {
//         get {
//             Employees ??= Mates
//                     .Aggregate(new List<IEmployee>(), (e, p) => {
//                         e.Insert(p.Rating, p.Mate);
//                         return e;
//                     });
//             return Employees!;
//         }
//     }

//     IEmployee IWishlist.Owner => Owner;

//     private List<IEmployee>? Employees;
// }

// public class TeamleadWishlist : CommonWishlist<Teamlead, Junior>
// {
//     public TeamleadWishlist() {}
    
//     [SetsRequiredMembers]
//     public TeamleadWishlist(IWishlist wishlist)
//     {
//         var mates = wishlist.DesiredEmployees;
//         Owner = new Teamlead(wishlist.Owner);
//         Mates = Enumerable
//             .Range(0, mates.Count())
//             .Select<int, Rate<Junior>>(i => new() { Rating = i, Mate = new Junior(mates.ElementAt(i)) })
//             .ToList();
//     }
// }

// public class JuniorWishlist : CommonWishlist<Junior, Teamlead>
// {
//     public JuniorWishlist() {}
    
//     [SetsRequiredMembers]
//     public JuniorWishlist(IWishlist wishlist)
//     {
//         var mates = wishlist.DesiredEmployees;
//         Owner = new Junior(wishlist.Owner);
//         Mates = Enumerable
//             .Range(0, mates.Count())
//             .Select<int, Rate<Teamlead>>(i => new(){ Rating = i, Mate = new Teamlead(mates.ElementAt(i))})
//             .ToList();
//     }
// }
