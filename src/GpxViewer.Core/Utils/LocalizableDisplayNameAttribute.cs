using System;
using System.ComponentModel;

namespace GpxViewer.Core.Utils
{
    [AttributeUsage(AttributeTargets.Property)]
    public class LocalizableDisplayNameAttribute : DisplayNameAttribute
    {
        private readonly Type _resourceType;
        private readonly string _propertyName;

        public LocalizableDisplayNameAttribute(Type resourceType, string propertyName)
        {
            _resourceType = resourceType;
            _propertyName = propertyName;
        }

        /// <inheritdoc />
        public override string DisplayName => LocalizationUtils.GetLocalizedString(_resourceType, _propertyName);
    }
}
