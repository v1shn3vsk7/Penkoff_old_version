using Penkoff.Storage.Entities;

namespace Penkoff_ASP.NET_Core_.Models;

public class OperationsViewModel
{
    public User User { get; set; }

    public IEnumerable <Operation> Operations { get; set; }
}
