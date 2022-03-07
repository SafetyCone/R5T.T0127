using System;

using R5T.B0002;
using R5T.T0045;
using R5T.T0113;


namespace R5T.T0127.X001
{
    public static class Instances
    {
        public static INamespaceName NamespaceName { get; } = B0002.NamespaceName.Instance;
        public static INamespaceGenerator NamespaceGenerator { get; } = T0045.NamespaceGenerator.Instance;
        public static IProjectOperator ProjectOperator { get; } = T0113.ProjectOperator.Instance;
    }
}
