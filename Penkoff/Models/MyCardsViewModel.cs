using Penkoff.Storage.Entities;

namespace Penkoff_ASP.NET_Core_.Models;

public class MyCardsViewModel
{
    public User User { get; set; }

    public IEnumerable<Card> Cards { get; set; }
}

