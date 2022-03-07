using System;


namespace R5T.T0127
{
    public class CodeFileContext : ICodeFileContext
    {
        public string CodeFilePath { get; set; }

        public IProjectFileContext ProjectFileContext { get; set; }
    }
}
