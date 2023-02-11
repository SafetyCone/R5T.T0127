using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using R5T.D0101;
using R5T.T0127;

using Instances = R5T.T0127.X001.Instances;


namespace System
{
    public static class IProjectFileContextExtensions
    {
        public static async Task AddDependencyProjectReferencesByIdentityStrings_Idempotent(this IProjectFileContext projectFileContext,
            IProjectRepository projectRepository,
            IEnumerable<string> projectIdentityStrings)
        {
            var projectFilePaths = await projectRepository.GetProjectFilePaths(projectIdentityStrings);

            await projectFileContext.AddDependencyProjectReferencesByFilePath_Idempotent(
                projectFilePaths.Values);
        }

        public static async Task AddDependencyProjectReferencesByFilePath_Idempotent(this IProjectFileContext projectFileContext,
            IEnumerable<string> projectFilePaths)
        {
            // Add the project reference to the project.
            await projectFileContext.VisualStudioProjectFileOperator.AddProjectReferences(
                projectFileContext.ProjectFilePath,
                projectFilePaths);

            // Add the project reference and all dependency project references to the solution.
            var recursiveProjectReferences = await Instances.ProjectOperator.GetAllRecursiveProjectReferencesInclusive(
                projectFilePaths,
                projectFileContext.VisualStudioProjectFileReferencesProvider);

            await projectFileContext.VisualStudioSolutionFileOperator.AddDependencyProjectReferences(
                projectFileContext.SolutionFilePaths,
                recursiveProjectReferences);
        }

        /// <summary>
        /// Chooses <see cref="AddDependencyProjectReferencesByIdentityStrings_Idempotent(IProjectFileContext, IProjectRepository, IEnumerable{string})"/> as the default.
        /// </summary>
        public static async Task AddDependencyProjectReferences(this IProjectFileContext projectFileContext,
            IProjectRepository projectRepository,
            IEnumerable<string> projectIdentityStrings)
        {
            await projectFileContext.AddDependencyProjectReferencesByIdentityStrings_Idempotent(
                projectRepository,
                projectIdentityStrings);
        }

        public static async Task EnsureHasProjectReferences(this IProjectFileContext projectFileContext,
            IProjectRepository projectRepository,
            IEnumerable<string> projectIdentityStrings)
        {
            await projectFileContext.AddDependencyProjectReferencesByIdentityStrings_Idempotent(
                projectRepository,
                projectIdentityStrings);
        }

        public static async Task EnsureHasProjectReference(this IProjectFileContext projectFileContext,
            IProjectRepository projectRepository,
            string projectIdentityString)
        {
            await projectFileContext.EnsureHasProjectReferences(
                projectRepository,
                EnumerableHelper.From(projectIdentityString));
        }
    }
}
