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

        public ElasticityTensorContinuum3D(double[,] data) : base(data)
        {
            if (data.GetLength(0) != 6 || data.GetLength(1) != 6) throw new ArgumentException(String.Format("input array (dimensions: {0} by {1}) is not 6 by 6", data.GetLength(0), data.GetLength(1)));
        }
    }
}
