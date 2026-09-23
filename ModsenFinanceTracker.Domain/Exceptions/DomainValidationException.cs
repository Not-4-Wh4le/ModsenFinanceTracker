using System;
using System.Collections.Generic;
using System.Text;

namespace ModsenFinanceTracker.Domain.Exceptions;

public class DomainValidationException : DomainException
{
    public string FieldName { get; }
    public string Reason { get; }

    public DomainValidationException(string fieldName, string reason)
        : base($"Validation failed for '{fieldName}': {reason}")
    {
        FieldName = fieldName;
        Reason = reason;
    }
}
