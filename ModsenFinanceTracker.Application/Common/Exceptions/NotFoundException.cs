namespace ModsenFinanceTracker.Application.Common.Exceptions;

public class NotFoundException : ApplicationException
{
    public NotFoundException(string name, object key)
        : base($"Entity {name} with {key} id not found")
    {
    }

    public NotFoundException(string message) 
        : base(message)
    {
    }
}
