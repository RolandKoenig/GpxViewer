using System;
using System.ComponentModel;

namespace GpxViewer.Core.Utils
{
    [AttributeUsage(AttributeTargets.Property)]
    public class LocalizableCategoryAttribute : CategoryAttribute
    {
        private readonly Type _resourceType;
        private readonly string _propertyName;

        public LocalizableCategoryAttribute(Type resourceType, string propertyName)
            : base(LocalizationUtils.GetLocalizedString(resourceType, propertyName))
        {
            _resourceType = resourceType;
            _propertyName = propertyName;
        }

        /// <inheritdoc />
        protected override string? GetLocalizedString(string value)
        {
            return LocalizationUtils.GetLocalizedString(_resourceType, _propertyName);
        }
    }
}
