using System;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using R5T.Magyar;

using R5T.T0126;
using R5T.T0127;


namespace System
{
    public static class INamespaceSyntaxContextExtensions
    {
        public static async Task<CompilationUnitSyntax> InAcquiredClassContext(this INamespaceSyntaxContext namespaceContext,
            CompilationUnitSyntax compilationUnit,
            Func<NamespaceDeclarationSyntax, WasFound<ClassDeclarationSyntax>> classSelector,
            Func<CompilationUnitSyntax, IClassSyntaxContext, Task<CompilationUnitSyntax>> classModifier,
            Func<ClassDeclarationSyntax> classConstructor)
        {
            var outputCompilationUnit = compilationUnit;

            var @namespace = namespaceContext.NamespaceAnnotation.GetAnnotatedNode_Typed(compilationUnit);

            var classWasFound = classSelector(@namespace);

            var unannotatedClass = classWasFound
                ? classWasFound.Result
                : classConstructor()
                ;

            var annotatedClass = unannotatedClass.Annotate(out var annotation);

            // Make sure the annotated class exists in the compilation unit.
            if(classWasFound)
            {
                outputCompilationUnit = outputCompilationUnit.ReplaceNode_Better(unannotatedClass, annotatedClass);
            }
            else
            {
                var newNamespace = @namespace.AddClassWithSurroundingSpacingAdjustment(annotatedClass);

                outputCompilationUnit = outputCompilationUnit.ReplaceNode_Better(@namespace, newNamespace);
            }

            var classAnnotation = ClassDeclarationAnnotation.From(annotation);

            var classContext = ClassSyntaxContext.From(
                classAnnotation,
                namespaceContext);

            outputCompilationUnit = await classModifier(
                outputCompilationUnit,
                classContext);

            return outputCompilationUnit;
        }

        public static async Task<CompilationUnitSyntax> InAcquiredClassContext(this INamespaceSyntaxContext namespaceContext,
            CompilationUnitSyntax compilationUnit,
            string className,
            Func<CompilationUnitSyntax, IClassSyntaxContext, Task<CompilationUnitSyntax>> classModifier,
            Func<ClassDeclarationSyntax> classConstructor)
        {
            var output = await namespaceContext.InAcquiredClassContext(
                compilationUnit,
                xNamespace =>
                {
                    var classWasFound = xNamespace.HasClass(className);
                    return classWasFound;
                },
                classModifier,
                classConstructor);

            return output;
        }
    }
}
