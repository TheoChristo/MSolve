using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISAAR.MSolve.PreProcessor.Interfaces;
using ISAAR.MSolve.Matrices.Interfaces;
using ISAAR.MSolve.Matrices;

namespace ISAAR.MSolve.PreProcessor.Materials
{
    public class StochasticElasticMaterial : IStochasticContinuumMaterial2D, IFiniteElementMaterial
    {
        public IStochasticMaterialCoefficientsProvider CoefficientsProvider { get; set; }
        public double YoungModulus { get; set; }
        public double PoissonRatio { get; set; }
        public double[] Coordinates { get; set; }

        public StochasticElasticMaterial(IStochasticMaterialCoefficientsProvider coefficientsProvider)
        {
            this.CoefficientsProvider = coefficientsProvider;
        }

        public double[] GetStochasticMaterialProperties(double[] coordinates)
        {
            double stochasticYoungModulus = CoefficientsProvider.GetCoefficient(YoungModulus, coordinates);
            return new double[] { stochasticYoungModulus, PoissonRatio };
        }

        #region IFiniteElementMaterialMembers

        public int ID
        {
            get { return 1; }
        }

        public bool Modified
        {
            get { return false; }
        }

        public StressStrainVectorContinuum2D Stresses
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ElasticityTensorContinuum2D ConstitutiveMatrix
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void ResetModified()
        {
            throw new NotImplementedException();
        }

        public object Clone()
        {
            return new StochasticElasticMaterial(CoefficientsProvider) { YoungModulus = this.YoungModulus, PoissonRatio = this.PoissonRatio };
        }
        #endregion

        #region IStochasticContinuumMaterial2DMembers

        ElasticityTensorContinuum2D IStochasticContinuumMaterial2D.GetConstitutiveMatrix(double[] coordinates)
        {
            throw new NotImplementedException();
        }

        public void UpdateMaterial(StressStrainVectorContinuum2D strains)
        {
            throw new NotImplementedException();
        }

        public void ClearState()
        {
            throw new NotImplementedException();
        }

        public void SaveState()
        {
            throw new NotImplementedException();
        }

        public void ClearStresses()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}



