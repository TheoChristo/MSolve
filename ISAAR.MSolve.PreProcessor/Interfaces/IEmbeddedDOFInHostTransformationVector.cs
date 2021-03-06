﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISAAR.MSolve.PreProcessor.Embedding;

namespace ISAAR.MSolve.PreProcessor.Interfaces
{
    public interface IEmbeddedDOFInHostTransformationVector
    {
        IList<DOFType> GetDependentDOFTypes { get; }
        IList<IList<DOFType>> GetDOFTypesOfHost(EmbeddedNode node);
        double[][] GetTransformationVector(EmbeddedNode node);
    }
}
