﻿using System;

using R5T.T0126;


namespace R5T.T0127
{
    public interface IClassSyntaxContext
    {
        ClassAnnotation ClassAnnotation { get; }
        INamespaceSyntaxContext NamespaceContext { get; }
    }
}
