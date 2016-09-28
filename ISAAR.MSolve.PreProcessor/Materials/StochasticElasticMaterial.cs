using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISAAR.MSolve.PreProcessor.Interfaces;
using ISAAR.MSolve.Matrices.Interfaces;
using ISAAR.MSolve.Matrices;

namespace ISAAR.MSolve.PreProcessor.Materials
{
    public class StochasticElasticMaterial : IStochasticFiniteElementMaterial, IFiniteElementMaterial
    {
        public IStochasticMaterialCoefficientsProvider CoefficientsProvider { get; set; }
        public double YoungModulus { get; set; } 
        public double PoissonRatio { get; set; }
        public double[] Coordinates { get; set; }

        public StochasticElasticMaterial(IStochasticMaterialCoefficientsProvider coefficientsProvider)
        {
            this.CoefficientsProvider = coefficientsProvider;
        }

        private double GetStochasticYoungModulus(double[] coordinates)
        {
            double stochasticYoungModulus = CoefficientsProvider.GetCoefficient(YoungModulus, coordinates);
            return stochasticYoungModulus;
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

        public void ResetModified()
        {
            throw new NotImplementedException();
        }

        public object Clone()
        {
            return new StochasticElasticMaterial (CoefficientsProvider) { YoungModulus = this.YoungModulus, PoissonRatio = this.PoissonRatio };
        }
        #endregion

    }
}



