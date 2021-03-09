﻿using System;
using System.Collections.Generic;
using System.Reflection;
using FirLib.Core.Checking;

namespace FirLib.Core.Patterns.ErrorAnalysis
{
    public class ExceptionInfoNode : IComparable<ExceptionInfoNode>
    {
        private Exception? m_exception;

        /// <summary>
        /// Gets a collection containing all child nodes.
        /// </summary>
        public List<ExceptionInfoNode> ChildNodes { get; } = new();

        public bool IsExceptionNode => m_exception != null;

        public string PropertyName { get; }

        public string PropertyValue { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionInfoNode"/> class.
        /// </summary>
        public ExceptionInfoNode(Exception ex)
        {
            ex.EnsureNotNull(nameof(ex));

            m_exception = ex;

            this.PropertyName = ex.GetType().GetTypeInfo().Name;
            this.PropertyValue = ex.Message;
        }

        public ExceptionInfoNode(ExceptionProperty property)
        {
            property.EnsureNotNull(nameof(property));

            this.PropertyName = property.Name;
            this.PropertyValue = property.Value;
        }

        public int CompareTo(ExceptionInfoNode? other)
        {
            if (other == null) { return -1; }
            if(this.IsExceptionNode != other.IsExceptionNode)
            {
                if (this.IsExceptionNode) { return -1; }
                else { return 1; }
            }

            return string.Compare(this.PropertyName, other.PropertyName, StringComparison.Ordinal);
        }

        public override string ToString()
        {
            return $"{this.PropertyName}: {this.PropertyValue}";
        }
    }
}