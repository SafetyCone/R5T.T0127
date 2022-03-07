using System;


namespace R5T.T0127
{
    public class CompilationUnitContext : ICompilationUnitContext
    {
        public ICodeFileContext CodeFileContext { get; set; }

        public IProjectFileContext ProjectFileContext => this.CodeFileContext.ProjectFileContext;
    }
}
