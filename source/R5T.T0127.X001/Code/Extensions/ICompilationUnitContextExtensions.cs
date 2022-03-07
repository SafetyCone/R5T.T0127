using System;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using R5T.Magyar;

using R5T.T0126;
using R5T.T0127;

using Instances = R5T.T0127.X001.Instances;


namespace System
{
    public static class ICompilationUnitContextExtensions
    {
        /// <summary>
        /// Performs common actions before modifying code, including:
        /// * Adding the System namespace.
        /// </summary>
        public static Task<CompilationUnitSyntax> PerformBeforeModificationOperationActions(this ICompilationUnitContext _,
            CompilationUnitSyntax compilationUnit)
        {
            compilationUnit = compilationUnit.EnsureHasUsings(
                Instances.NamespaceName.System());

            return Task.FromResult(compilationUnit);
        }

        /// <summary>
        /// Performs common actions after modifying code, including:
        /// * Formatting using directives, including blocking, block ordering, and within-block ordering.
        /// </summary>
        public static Task<CompilationUnitSyntax> PerformAfterModificationOperationActions(this ICompilationUnitContext _,
            CompilationUnitSyntax compilationUnit)
        {
            return Task.FromResult(compilationUnit);
        }

        public static async Task<CompilationUnitSyntax> Modify(this ICompilationUnitContext compilationUnitContext,
            CompilationUnitSyntax compilationUnit,
            Func<ICompilationUnitContext, CompilationUnitSyntax, Task<CompilationUnitSyntax>> compilationUnitContextAction)
        {
            compilationUnit = await compilationUnitContext.PerformBeforeModificationOperationActions(compilationUnit);

            compilationUnit = await compilationUnitContextAction(
                compilationUnitContext,
                compilationUnit);

            compilationUnit = await compilationUnitContext.PerformAfterModificationOperationActions(compilationUnit);

            return compilationUnit;
        }

        public static async Task<CompilationUnitSyntax> Modify(this ICompilationUnitContext compilationUnitContext,
            CompilationUnitSyntax compilationUnit,
            Func<CompilationUnitSyntax, Task<CompilationUnitSyntax>> compilationUnitAction)
        {
            compilationUnit = await compilationUnitContext.PerformBeforeModificationOperationActions(compilationUnit);

            compilationUnit = await compilationUnitAction(compilationUnit);

            compilationUnit = await compilationUnitContext.PerformAfterModificationOperationActions(compilationUnit);

            return compilationUnit;
        }

        public static async Task<CompilationUnitSyntax> InAcquiredNamespaceContext(this ICompilationUnitContext _,
            CompilationUnitSyntax compilationUnit,
            Func<CompilationUnitSyntax, WasFound<NamespaceDeclarationSyntax>> namespaceSelector,
            Func<CompilationUnitSyntax, INamespaceSyntaxContext, Task<CompilationUnitSyntax>> namespaceModifier,
            Func<NamespaceDeclarationSyntax> namespaceConstructor)
        {
            var outputCompilationUnit = compilationUnit;

            var namespaceWasFound = namespaceSelector(outputCompilationUnit);

            var unannotatedNamespace = namespaceWasFound
                ? namespaceWasFound.Result
                : namespaceConstructor()
                ;

            var annotatedNamespace = unannotatedNamespace.Annotate(out var annotation);

            // Make sure the annotated namespace exists in the compilation unit.
            if(namespaceWasFound)
            {
                outputCompilationUnit = outputCompilationUnit.ReplaceNode_Better(unannotatedNamespace, annotatedNamespace);
            }
            else
            {
                // Add the namspace.
                outputCompilationUnit = outputCompilationUnit.AddNamespace(annotatedNamespace);
            }

            var namespaceAnnotation = NamespaceDeclarationAnnotation.From(annotation);

            var namespaceContext = NamespaceSyntaxContext.From(namespaceAnnotation);

            outputCompilationUnit = await namespaceModifier(
                outputCompilationUnit,
                namespaceContext);

            return outputCompilationUnit;
        }

        public static async Task<CompilationUnitSyntax> InAcquiredNamespaceContext(this ICompilationUnitContext compilationUnitContext,
            CompilationUnitSyntax compilationUnit,
            string namespaceName,
            Func<CompilationUnitSyntax, INamespaceSyntaxContext, Task<CompilationUnitSyntax>> namespaceModifier)
        {
            var outputCompilationUnit = await compilationUnitContext.InAcquiredNamespaceContext(
                compilationUnit,
                xCompilationUnit =>
                {
                    var namespaceWasFound = xCompilationUnit.HasNamespace(namespaceName);
                    return namespaceWasFound;
                },
                namespaceModifier,
                () =>
                {
                    var @namespace = Instances.NamespaceGenerator.GetNewNamespace2(namespaceName);
                    return @namespace;
                });

            return outputCompilationUnit;
        }
    }
}
