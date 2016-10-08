using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISAAR.MSolve.PreProcessor.Interfaces
{
    public interface IFiniteElementMaterial : ICloneable
    {
        int ID { get; }
        bool Modified { get; }
        void ResetModified();
        //double YoungModulus { get; set; } //TODO remove
        //double PoissonRatio { get; set; } //TODO remove
        double[] Coordinates { get; set; }
    }
}
