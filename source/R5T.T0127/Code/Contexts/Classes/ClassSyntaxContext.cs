using System;

using R5T.T0126;


namespace R5T.T0127
{
    public class ClassSyntaxContext : IClassSyntaxContext
    {
        #region From

        public static ClassSyntaxContext From(
            ClassAnnotation classAnnotation,
            INamespaceSyntaxContext namespaceContext)
        {
            var output = new ClassSyntaxContext
            {
                ClassAnnotation = classAnnotation,
                NamespaceContext = namespaceContext
            };

            return output;
        }

        #endregion


        public ClassAnnotation ClassAnnotation { get; set; }
        public INamespaceSyntaxContext NamespaceContext { get; set; }
    }
}
