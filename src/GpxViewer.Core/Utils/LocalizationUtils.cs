using System;
using System.Reflection;

namespace GpxViewer.Core.Utils
{
    public static class LocalizationUtils
    {
        public static string GetLocalizedString(Type resourceType, string propertyName)
        {
            var property = resourceType.GetProperty(
                propertyName,
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            if (property == null)
            {
                throw new InvalidOperationException(
                    $"Property {propertyName} not found on resource type {resourceType.FullName}!");
            }

            var result = property.GetValue(null) as string;
            if (string.IsNullOrEmpty(result))
            {
                throw new InvalidOperationException(
                    $"No value for Property {propertyName} on resource type {resourceType.FullName}!");
            }

            return result;
        }
    }
}
