using System;
using System.ComponentModel;
using System.Reflection;

namespace GpxViewer.Core.Utils
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    internal class LocalizableDescriptionAttribute : DescriptionAttribute
    {
        private readonly Type _resourceType;
        private readonly string _propertyName;

        public LocalizableDescriptionAttribute(Type resourceType, string propertyName)
        {
            _resourceType = resourceType;
            _propertyName = propertyName;
        }

        /// <inheritdoc />
        public override string Description => LocalizationUtils.GetLocalizedString(_resourceType, _propertyName);
    }
}
