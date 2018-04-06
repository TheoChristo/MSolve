﻿
using ISAAR.MSolve.Numerical.LinearAlgebra;
using ISAAR.MSolve.Numerical.LinearAlgebra.Interfaces;

namespace ISAAR.MSolve.Materials.Interfaces
{
    public interface IContinuumMaterial3D : IFiniteElementMaterial
    {
        double[] Stresses { get; }
        ElasticityTensorContinuum3D ConstitutiveMatrix { get; }
        void UpdateMaterial(double[] strains);
        void ClearState();
        void SaveState();
        void ClearStresses();
    }
}
