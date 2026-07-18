using CSharpFunctionalExtensions;
using Errs;

namespace DeliveryApp.Core.Domain.Models.OrderAggregate;

public class OrderStatus : Entity<int>
{
    public static readonly OrderStatus Created = new OrderStatus(1, nameof(Created).ToLowerInvariant());
    public static readonly OrderStatus Assigned = new OrderStatus(2, nameof(Assigned).ToLowerInvariant());
    public static readonly OrderStatus Completed = new OrderStatus(3, nameof(Completed).ToLowerInvariant());

    private OrderStatus()
    {
    }

    private OrderStatus(int id, string name) : base(id)
    {
        Name = name;
    }

    public string Name { get; }

    public static IEnumerable<OrderStatus> List()
    {
        yield return Created;
        yield return Assigned;
        yield return Completed;
    }

    public static Result<OrderStatus, Error> FromName(string name)
    {
        var status = List().FirstOrDefault(s => s.Name == name);
        if (status is null)
        {
            return Errors.StatusIsWrong;
        }

        return status;
    }

    public static Result<OrderStatus, Error> FromId(int id)
    {
        var status = List().FirstOrDefault(s => s.Id == id);
        if (status is null)
        {
            return Errors.StatusIsWrong;
        }

        return status;
    }

    public static class Errors
    {
        public static Error StatusIsWrong => new Error($"{nameof(OrderStatus).ToLowerInvariant()}.is.wrong",
            $"Не верное значение. Допустимые значения: {string.Join(", ", List().Select(s => s.Name))}");
    }
}