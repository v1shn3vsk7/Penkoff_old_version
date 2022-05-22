using Penkoff.Storage.Entities;

namespace Penkoff_ASP.NET_Core_.Models;

public class AccountViewModel
{
    public RubleAccount rubleAccount { get; set; }

    public DollarAccount dollarAccount { get; set; }

    public EuroAccount euroAccount { get; set; }

    public string? currentBalance;
}

