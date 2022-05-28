using Penkoff.Storage.Entities;

namespace Penkoff_ASP.NET_Core_.Models;

public class SendMoneyViewModel
{
    public string? CurrencyPick { get; set; }

    public uint Balance { get; set; }

    public string Result { get; set; }

    public string ReceiverPhone { get; set; }

    public uint Amount { get; set; }
}

