using CSharpFunctionalExtensions;
using Errs;

namespace DeliveryApp.Core.Domain.Models.CourierAggregate;

public class Status : Entity<int>
{
    public static readonly Status Assigned = new Status(1, nameof(Assigned).ToLowerInvariant());
    public static readonly Status Completed = new Status(2, nameof(Completed).ToLowerInvariant());

    private Status()
    {
    }

    private Status(int id, string name) : base(id)
    {
        Name = name;
    }

    public string Name { get; }

    public static IEnumerable<Status> List()
    {
        yield return Assigned;
        yield return Completed;
    }

    public static Result<Status, Error> FromName(string name)
    {
        var status = List().FirstOrDefault(s => s.Name == name);
        if (status is null)
        {
            return Errors.StatusIsWrong;
        }

        return status;
    }

    public static Result<Status, Error> FromId(int id)
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
        public static Error StatusIsWrong => new Error($"{nameof(Status).ToLowerInvariant()}.is.wrong",
            $"Не верное значение. Допустимые значения: {string.Join(", ", List().Select(s => s.Name))}");
    }
}