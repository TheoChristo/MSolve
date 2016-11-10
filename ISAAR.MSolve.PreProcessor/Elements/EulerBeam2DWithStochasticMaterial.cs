using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISAAR.MSolve.PreProcessor.Interfaces;
using ISAAR.MSolve.PreProcessor.Materials;
using ISAAR.MSolve.Matrices.Interfaces;
using ISAAR.MSolve.Matrices;

namespace ISAAR.MSolve.PreProcessor.Elements
{

    public class EulerBeam2DWithStochasticMaterial : EulerBeam2D
    {
        //protected readonly EulerBeam2DMemoizer memoizer;
        public IStochasticMaterialCoefficientsProvider CoefficientsProvider { get; set; }


        //public EulerBeam2DWithStochasticMaterial(double youngModulus, EulerBeam2DMemoizer memoizer) : this(youngModulus)
        //{
        //    this.memoizer = memoizer;
        //}

        public EulerBeam2DWithStochasticMaterial(double youngModulus, IStochasticMaterialCoefficientsProvider coefficientsProvider)
            : base(youngModulus)
        {
            this.CoefficientsProvider = coefficientsProvider;
        }

        public EulerBeam2DWithStochasticMaterial(double youngModulus, IFiniteElementDOFEnumerator dofEnumerator, IStochasticMaterialCoefficientsProvider coefficientsProvider)
            : base(youngModulus, dofEnumerator)
        {
            this.CoefficientsProvider = coefficientsProvider;
        }

        public override IMatrix2D<double> StiffnessMatrix(Element element)
        {
            double x2 = Math.Pow(element.Nodes[1].X - element.Nodes[0].X, 2);
            double y2 = Math.Pow(element.Nodes[1].Y - element.Nodes[0].Y, 2);
            double L = Math.Sqrt(x2 + y2);
            double c = (element.Nodes[1].X - element.Nodes[0].X) / L;
            double c2 = c * c;
            double s = (element.Nodes[1].Y - element.Nodes[0].Y) / L;
            double s2 = s * s;
            double[] coordinates = GetStochasticPoints(element);
            //double EL = (material as StochasticElasticMaterial).GetStochasticMaterialProperties(coordinates)[0] / L;
            double EL = CoefficientsProvider.GetCoefficient(base.youngModulus, coordinates) / L;
            //double EL = this.youngModulus;
            double EAL = EL * SectionArea;
            double EIL = EL * MomentOfInertia;
            double EIL2 = EIL / L;
            double EIL3 = EIL2 / L;
            return DOFEnumerator.GetTransformedMatrix(new SymmetricMatrix2D<double>(new double[] { c2*EAL+12*s2*EIL3, c*s*EAL-12*c*s*EIL3, -6*s*EIL2, -c2*EAL-12*s2*EIL3, -c*s*EAL+12*c*s*EIL3, -6*s*EIL2,
                s2*EAL+12*c2*EIL3, 6*c*EIL2, -s*c*EAL+12*c*s*EIL3, -s2*EAL-12*c2*EIL3, 6*c*EIL2,
                4*EIL, 6*s*EIL2, -6*c*EIL2, 2*EIL,
                c2*EAL+12*s2*EIL3, s*c*EAL-12*c*s*EIL3, 6*s*EIL2,
                s2*EAL+12*c2*EIL3, -6*c*EIL2,
                4*EIL }));
        }

        private double[] GetStochasticPoints(Element element)
        {
            // Calculate for element centroid
            double X = 0;
            double Y = 0;
            double minX = element.Nodes[0].X;
            double minY = element.Nodes[0].Y;

            minX = minX > element.Nodes[0].X ? element.Nodes[0].X : minX;
            minY = minY > element.Nodes[0].Y ? element.Nodes[0].Y : minY;
            X = X < Math.Abs(element.Nodes[1].X - element.Nodes[0].X) ? Math.Abs(element.Nodes[1].X - element.Nodes[0].X) : X;
            Y = Y < Math.Abs(element.Nodes[1].Y - element.Nodes[0].Y) ? Math.Abs(element.Nodes[1].Y - element.Nodes[0].Y) : Y;

            double pointX = minX + X / 2;
            double pointY = minY + Y / 2;

            return new double[] { pointX, pointY };

            //// Calculate for individual gauss point
            ////if (iInt != 2) throw new ArgumentException("Stochastic provided functions with integration order of 2.");

            //double X = 0;
            //double Y = 0;
            //double Z = 0;
            //double minX = element.Nodes[0].X;
            //double minY = element.Nodes[0].Y;
            //double minZ = element.Nodes[0].Z;

            //for (int i = 0; i < 8; i++)
            //{
            //    minX = minX > element.Nodes[i].X ? element.Nodes[i].X : minX;
            //    minY = minY > element.Nodes[i].Y ? element.Nodes[i].Y : minY;
            //    minZ = minZ > element.Nodes[i].Z ? element.Nodes[i].Z : minZ;
            //    for (int j = i + 1; j < 8; j++)
            //    {
            //        X = X < Math.Abs(element.Nodes[j].X - element.Nodes[i].X) ? Math.Abs(element.Nodes[j].X - element.Nodes[i].X) : X;
            //        Y = Y < Math.Abs(element.Nodes[j].Y - element.Nodes[i].Y) ? Math.Abs(element.Nodes[j].Y - element.Nodes[i].Y) : Y;
            //        Z = Z < Math.Abs(element.Nodes[j].Z - element.Nodes[i].Z) ? Math.Abs(element.Nodes[j].Z - element.Nodes[i].Z) : Z;
            //    }
            //}

            //double pointX = minX + X / 2 * (integrationPoints[iInt][iX] + 1);
            //double pointY = minY + Y / 2 * (integrationPoints[iInt][iY] + 1);
            //double pointZ = minZ + Z / 2 * (integrationPoints[iInt][iZ] + 1);

            //return new double[] { pointX, pointY, pointZ };
        }
    }
}
