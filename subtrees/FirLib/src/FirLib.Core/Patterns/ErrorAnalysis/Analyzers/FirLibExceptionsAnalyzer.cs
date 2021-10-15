using System;
using System.Collections.Generic;
using System.Text;
using FirLib.Core.Patterns.Messaging;

namespace FirLib.Core.Patterns.ErrorAnalysis.Analyzers
{
    public class FirLibExceptionsAnalyzer : IExceptionAnalyzer
    {
        /// <inheritdoc />
        public IEnumerable<ExceptionProperty>? ReadExceptionInfo(Exception ex)
        {
            if (ex is MessagePublishException msgPublishEx)
            {
                yield return new ExceptionProperty(
                    nameof(MessagePublishException.MessageType),
                    msgPublishEx.MessageType?.FullName ?? string.Empty);
            }
        }

        /// <inheritdoc />
        public IEnumerable<Exception>? GetInnerExceptions(Exception ex)
        {
            if (ex is MessagePublishException msgPublishEx)
            {
                foreach (var actInnerException in msgPublishEx.PublishExceptions)
                {
                    yield return actInnerException;
                }
            }
        }
    }
}
