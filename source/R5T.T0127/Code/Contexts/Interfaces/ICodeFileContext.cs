using System;


namespace R5T.T0127
{
    public interface ICodeFileContext : IHasProjectFileContext
    {
        public string CodeFilePath { get; }
    }
}
