using System;

using R5T.T0126;


namespace R5T.T0127
{
    public class NamespaceSyntaxContext : INamespaceSyntaxContext
    {
        #region Static

        public static NamespaceSyntaxContext From(NamespaceAnnotation namespaceDeclarationAnnotation)
        {
            var output = new NamespaceSyntaxContext
            {
                NamespaceAnnotation = namespaceDeclarationAnnotation
            };

            return output;
        }

        #endregion


        public NamespaceAnnotation NamespaceAnnotation { get; set; }
    }
}
