using System;

using R5T.D0078;
using R5T.D0079;
using R5T.D0083;


namespace R5T.T0127
{
    public interface IProjectFileContext
    {
        string[] SolutionFilePaths { get; }
        string ProjectFilePath { get; }


        IVisualStudioProjectFileOperator VisualStudioProjectFileOperator { get; }
        IVisualStudioProjectFileReferencesProvider VisualStudioProjectFileReferencesProvider { get; }
        IVisualStudioSolutionFileOperator VisualStudioSolutionFileOperator { get; }
    }
}
