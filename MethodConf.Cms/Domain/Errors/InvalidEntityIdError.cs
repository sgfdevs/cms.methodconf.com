using FluentResults;

namespace MethodConf.Cms.Domain.Errors;

public class InvalidEntityIdError : Error
{
    public InvalidEntityIdError(string id) : base($"Entity id {id} is invalid")
    {
        Metadata.Add("Id", id);
    }
}
