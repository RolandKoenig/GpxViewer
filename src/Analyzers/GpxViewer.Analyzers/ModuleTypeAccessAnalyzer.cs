using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace GpxViewer.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ModuleTypeAccessAnalyzer : DiagnosticAnalyzer
    {
        public const string DIAGNOSTIC_ID = "GpxViewerAnalyzers";

        private const string TITLE = "Module Type accessibility";
        private const string MESSAGE_FORMAT = "Type '{0}' has invalid accessibility. Current: {1}, expected: {2}";
        private const string DESCRIPTION = "Only interface types and main type of modules are public, all others musst be internal";
        private const string CATEGORY = "GpxViewer Modules";

        private static readonly DiagnosticDescriptor s_rule = new DiagnosticDescriptor(DIAGNOSTIC_ID, TITLE, MESSAGE_FORMAT, CATEGORY, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: DESCRIPTION);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(s_rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;
            var fullNamespace = namedTypeSymbol.ContainingNamespace.ToString();
            if (!fullNamespace.StartsWith("GpxViewer.Modules")) { return; }

            var splittedNamespace = fullNamespace.Split('.');
            if (splittedNamespace.Length < 3) { return; }
            var moduleName = splittedNamespace[2];

            var namespaceInterface = $"GpxViewer.Modules.{moduleName}.Interface";
            var moduleMainTypeNamespace = $"GpxViewer.Modules.{moduleName}";
            var moduleMainTypeName = $"{moduleName}Module";

            var actTypeNamespace = namedTypeSymbol.ContainingNamespace.ToString();
            var actTypeName = namedTypeSymbol.Name;

            var expectedAccessibility = Accessibility.Internal;
            if (actTypeNamespace.StartsWith(namespaceInterface))
            {
                expectedAccessibility = Accessibility.Public;
            }
            else if (actTypeNamespace.Equals(moduleMainTypeNamespace, StringComparison.Ordinal) &&
                     actTypeName.Equals(moduleMainTypeName, StringComparison.Ordinal))
            {
                expectedAccessibility = Accessibility.Public;
            }

            if (namedTypeSymbol.DeclaredAccessibility != expectedAccessibility)
            {
                var diagnostic = Diagnostic.Create(s_rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name, namedTypeSymbol.DeclaredAccessibility, expectedAccessibility);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
