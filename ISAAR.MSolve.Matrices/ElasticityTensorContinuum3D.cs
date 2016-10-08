using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISAAR.MSolve.Matrices
{
    public class ElasticityTensorContinuum3D : Matrix2D<Double>
    {
        public ElasticityTensorContinuum3D() : base(6, 6)
        {
            // question: after I create a Matrix2D I cannot change its dimensions, right? otherwise creating this class is futile
        }
    }
}
