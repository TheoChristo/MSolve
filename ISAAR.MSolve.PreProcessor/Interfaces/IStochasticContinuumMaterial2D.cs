using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISAAR.MSolve.Matrices;

namespace ISAAR.MSolve.PreProcessor.Interfaces
{
    public interface IStochasticContinuumMaterial2D : IContinuumMaterial2D
    {
        IStochasticMaterialCoefficientsProvider CoefficientsProvider { get; set; }
        ElasticityTensorContinuum2D GetConstitutiveMatrix(double[] coordinates);
    }
}
