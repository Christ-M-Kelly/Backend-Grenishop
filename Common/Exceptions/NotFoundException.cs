namespace BackendGrenishop.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string name, object key) 
        : base($"{name} avec l'identifiant '{key}' n'a pas été trouvé.")
    {
    }
}
