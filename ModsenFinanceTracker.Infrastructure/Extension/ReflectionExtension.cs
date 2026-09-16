using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ModsenFinanceTracker.Infrastructure.Extension;

public static class ReflectionExtension
{
    public static void SetPrivateField<TEntity, TValue>(
        this TEntity entity, 
        string fieldName,
        TValue value)
    {
        var entityType = entity.GetType();

        var field = entityType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance)
            ?? throw new InvalidOperationException($"Field {fieldName} not found in '{entityType.Name}'");

        var valueType = value?.GetType() ?? typeof(TValue);

        if (!field.FieldType.IsAssignableFrom(typeof(TValue)))
        {
            throw new ArgumentException(
                $"Cannot assign value of type '{valueType.Name}' " +
                $"to field '{fieldName}' of type '{field.FieldType.Name}'" +
                $" on '{entityType.Name}'", nameof(value));
        }

        field.SetValue(entity, value);
    }
}
