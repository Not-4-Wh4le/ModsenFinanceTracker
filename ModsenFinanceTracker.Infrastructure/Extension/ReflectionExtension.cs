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
        var type = typeof(TEntity);

        var field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance)
            ?? throw new InvalidOperationException($"Field {fieldName} not found");

        if (!field.FieldType.IsAssignableFrom(typeof(TValue)))
        {
            throw new ArgumentException();
        }

        field.SetValue(entity, value);
    }
}
