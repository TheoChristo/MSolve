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
    //public class EulerBeam3DMemoizer
    //{
    //    private readonly Dictionary<int, Tuple<double[], double[,,]>> integrationDictionary = new Dictionary<int, Tuple<double[], double[,,]>>();

    //    public Tuple<double[], double[,,]> GetIntegrationData(int element)
    //    {
    //        if (integrationDictionary.ContainsKey(element))
    //            return integrationDictionary[element];
    //        else
    //            return new Tuple<double[], double[,,]>(null, null);
    //    }

    //    public void SetIntegrationData(int element, Tuple<double[], double[,,]> integrationData)
    //    {
    //        integrationDictionary.Add(element, integrationData);
    //    }
    //}

    public class EulerBeam3DWithStochasticMaterial : EulerBeam3D
    {
        private readonly double youngModulus;
        private readonly double poissonRatio;
        //protected readonly EulerBeam3DMemoizer memoizer;
        public IStochasticMaterialCoefficientsProvider CoefficientsProvider { get; set; }


        public EulerBeam3DWithStochasticMaterial(double youngModulus, double poissonRatio)
        {
            this.youngModulus = youngModulus;
            this.poissonRatio = poissonRatio;
        }

        //public EulerBeam3DWithStochasticMaterial(double youngModulus, EulerBeam3DMemoizer memoizer) : this(youngModulus)
        //{
        //    this.memoizer = memoizer;
        //}

        public EulerBeam3DWithStochasticMaterial(double youngModulus, double poissonRatio, Node[] rot1Nodes, Node[] rot2Nodes)
            : this(youngModulus, poissonRatio)
        {
            if (rot1Nodes != null && rot1Nodes.Length != 4)
                throw new ArgumentException("Dependent nodes quantity for rotation1 has to be four.");
            if (rot2Nodes != null && rot2Nodes.Length != 4)
                throw new ArgumentException("Dependent nodes quantity for rotation2 has to be four.");
            rotNodes[0] = rot1Nodes;
            rotNodes[1] = rot2Nodes;

            InitializeDOFsWhenNoRotations();
        }

        public EulerBeam3DWithStochasticMaterial(double youngModulus, double poissonRatio, IStochasticMaterialCoefficientsProvider coefficientsProvider)
            : this(youngModulus, poissonRatio)
        {
            this.CoefficientsProvider = coefficientsProvider;
        }

        public EulerBeam3DWithStochasticMaterial(double youngModulus, double poissonRatio, IFiniteElementDOFEnumerator dofEnumerator) : this(youngModulus, poissonRatio)
        {
            this.dofEnumerator = dofEnumerator;
        }

        protected override IMatrix2D<double> StiffnessMatrixPure(Element element)
        {
            //var m = (material as IFiniteElementMaterial3D);//TODO remove material
            double x2 = Math.Pow(element.Nodes[1].X - element.Nodes[0].X, 2);
            double y2 = Math.Pow(element.Nodes[1].Y - element.Nodes[0].Y, 2);
            double z2 = Math.Pow(element.Nodes[1].Z - element.Nodes[0].Z, 2);
            double L = 1 / Math.Sqrt(x2 + y2 + z2);
            double L2 = L * L;
            double L3 = L2 * L;
            double[] coordinates = GetStochasticPoints(element);
            //double EIx = m.YoungModulus * MomentOfInertiaX;

            /* deprecated calculations after the removal of the material class as input
            double EIy = m.YoungModulus * MomentOfInertiaY;
            double EIz = m.YoungModulus * MomentOfInertiaZ;
            double GJL = m.YoungModulus * L * MomentOfInertiaPolar / (2 * (1 + m.PoissonRatio));
            double EAL = m.YoungModulus * SectionArea * L;
            */

            double EIy = CoefficientsProvider.GetCoefficient(youngModulus, coordinates) * MomentOfInertiaY;
            double EIz = this.youngModulus * MomentOfInertiaZ;
            double GJL = this.youngModulus * L * MomentOfInertiaPolar / (2 * (1 + this.poissonRatio));
            double EAL = this.youngModulus * SectionArea * L;

            var stiffnessMatrix = new SymmetricMatrix2D<double>(new double[] { EAL, 0, 0, 0, 0, 0, -EAL, 0, 0, 0, 0, 0,
                12*EIz*L3, 0, 0, 0, 6*EIz*L2, 0, -12*EIz*L3, 0, 0, 0, 6*EIz*L2,
                12*EIy*L3, 0, -6*EIy*L2, 0, 0, 0, -12*EIy*L3, 0, -6*EIy*L2, 0,
                GJL, 0, 0, 0, 0, 0, -GJL, 0, 0,
                4*EIy*L, 0, 0, 0, 6*EIy*L2, 0, 2*EIy*L, 0,
                4*EIz*L, 0, -6*EIz*L2, 0, 0, 0, 2*EIz*L,
                EAL, 0, 0, 0, 0, 0,
                12*EIz*L3, 0, 0, 0, -6*EIz*L2,
                12*EIy*L3, 0, 6*EIy*L2, 0,
                GJL, 0, 0,
                4*EIy*L, 0,
                4*EIz*L
            });

            var refx = new double[] { 1, 1, 1 };
            var beamTransformation = new Matrix2D<double>(12, 12);
            beamTransformation[0, 0] = (element.Nodes[1].X - element.Nodes[0].X) * L;
            beamTransformation[0, 1] = (element.Nodes[1].Y - element.Nodes[0].Y) * L;
            beamTransformation[0, 2] = (element.Nodes[1].Z - element.Nodes[0].Z) * L;

            //beamTransformation[2, 0] = refx[0];
            //beamTransformation[2, 1] = refx[1];
            //beamTransformation[2, 2] = refx[2];

            //beamTransformation[1, 0] = beamTransformation[2, 1] * beamTransformation[0, 2] - beamTransformation[2, 2] * beamTransformation[0, 1];
            //beamTransformation[1, 1] = beamTransformation[2, 2] * beamTransformation[0, 0] - beamTransformation[2, 0] * beamTransformation[0, 2];
            //beamTransformation[1, 2] = beamTransformation[2, 0] * beamTransformation[0, 1] - beamTransformation[2, 1] * beamTransformation[0, 0];
            beamTransformation[1, 0] = refx[1] * beamTransformation[0, 2] - refx[2] * beamTransformation[0, 1];
            beamTransformation[1, 1] = refx[2] * beamTransformation[0, 0] - refx[0] * beamTransformation[0, 2];
            beamTransformation[1, 2] = refx[0] * beamTransformation[0, 1] - refx[1] * beamTransformation[0, 0];
            double dn = 1.0 / Math.Sqrt(beamTransformation[1, 0] * beamTransformation[1, 0] + beamTransformation[1, 1] * beamTransformation[1, 1] + beamTransformation[1, 2] * beamTransformation[1, 2]);
            beamTransformation[1, 0] = beamTransformation[1, 0] * dn;
            beamTransformation[1, 1] = beamTransformation[1, 1] * dn;
            beamTransformation[1, 2] = beamTransformation[1, 2] * dn;
            beamTransformation[2, 0] = beamTransformation[0, 1] * beamTransformation[1, 2] - beamTransformation[0, 2] * beamTransformation[1, 1];
            beamTransformation[2, 1] = beamTransformation[0, 2] * beamTransformation[1, 0] - beamTransformation[0, 0] * beamTransformation[1, 2];
            beamTransformation[2, 2] = beamTransformation[0, 0] * beamTransformation[1, 1] - beamTransformation[0, 1] * beamTransformation[1, 0];

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    beamTransformation[i + 3, j + 3] = beamTransformation[i, j];
                    beamTransformation[i + 6, j + 6] = beamTransformation[i, j];
                    beamTransformation[i + 9, j + 9] = beamTransformation[i, j];
                }

            return new SymmetricMatrix2D<double>(beamTransformation.Transpose() * stiffnessMatrix.ToMatrix2D() * beamTransformation);

            ////if (element.Nodes.Count(n => n.EmbeddedInElement != null) == 0) return stiffnessMatrix;
            //stiffnessMatrix = new SymmetricMatrix2D<double>(beamTransformation.Transpose() * stiffnessMatrix.ToMatrix2D() * beamTransformation);
            //if (embeddedNodes.Count == 0) return stiffnessMatrix;

            ////var hostElements = element.Nodes.Select(x => x.EmbeddedInElement).Distinct();
            //var size = GetElementDOFTypes(element).SelectMany(x => x).Count();
            //transformation = new Matrix2D<double>(dofs.SelectMany(d => d).Count(), size);
            //isNodeEmbedded = new bool[element.Nodes.Count];

            ////TODO : SEPARATE FROM ELEMENT!!
            ////TODO: Must match DOFs of host with embedded element
            //int row = 0;
            //int col = 0;
            //hostElementList = new List<Element>();
            //for (int i = 0; i < element.Nodes.Count; i++)
            //{
            //    var node = element.Nodes[i];
            //    var embeddedNode = embeddedNodes.Where(x => x.Node == node).FirstOrDefault();
            //    //var hostElement = node.EmbeddedInElement;
            //    Element hostElement = embeddedNode == null ? null : embeddedNode.EmbeddedInElement;
            //    if (hostElement == null)
            //    {
            //        isNodeEmbedded[i] = false;
            //        for (int j = 0; j < dofs[i].Length; j++)
            //        {
            //            transformation[row, col] = 1;
            //            row++;
            //            col++;
            //        }
            //    }
            //    else
            //    {
            //        isNodeEmbedded[i] = true;
            //        //double[] hostShapeFunctions = ((IEmbeddedHostElement)hostElement.ElementType).GetShapeFunctionsForNode(hostElement, node);
            //        double[] hostShapeFunctions = ((IEmbeddedHostElement)hostElement.ElementType).GetShapeFunctionsForNode(hostElement, embeddedNode);

            //        if (hostElementList.IndexOf(hostElement) < 0)
            //            hostElementList.Add(hostElement);
            //        else
            //            col -= hostShapeFunctions.Length * hostDofsPerNode;

            //        for (int j = 0; j < commonDofsPerNode; j++)
            //        {
            //            for (int k = 0; k < hostShapeFunctions.Length; k++)
            //                transformation[row, hostDofsPerNode * k + col + j] = hostShapeFunctions[k];
            //            row++;
            //        }
            //        row += embeddedDofsPerNode - commonDofsPerNode;
            //        col += hostShapeFunctions.Length * hostDofsPerNode;
            //    }
            //}

            //// Add identity matrix
            //int index = 0;
            //if (isNodeEmbedded[0])
            //{
            //    transformation[3, col] = 1;
            //    transformation[4, col + 1] = 1;
            //    transformation[5, col + 2] = 1;
            //    index += 3;
            //}
            //if (isNodeEmbedded[1])
            //{
            //    transformation[9, col + index] = 1;
            //    transformation[10, col + index + 1] = 1;
            //    transformation[11, col + index + 2] = 1;
            //}

            //var transformedMatrix = new SymmetricMatrix2D<double>(transformation.Transpose() * stiffnessMatrix.ToMatrix2D() * transformation);
            ////var sw = File.CreateText(@"d:\BeamTransformed.txt");
            ////for (int i = 0; i < 54; i++)
            ////{
            ////    var s = string.Empty;
            ////    for (int j = 0; j < 54; j++)
            ////        s += transformedMatrix[i, j].ToString() + ";";
            ////    sw.WriteLine(s);
            ////}
            ////sw.Close();
            //return transformedMatrix;
        }

        private double[] GetStochasticPoints(Element element)
        {
            // Calculate for element centroid
            double X = 0;
            double Y = 0;
            double Z = 0;
            double minX = element.Nodes[0].X;
            double minY = element.Nodes[0].Y;
            double minZ = element.Nodes[0].Z;

            for (int i = 0; i < 8; i++)
            {
                minX = minX > element.Nodes[i].X ? element.Nodes[i].X : minX;
                minY = minY > element.Nodes[i].Y ? element.Nodes[i].Y : minY;
                minZ = minZ > element.Nodes[i].Z ? element.Nodes[i].Z : minZ;
                for (int j = i + 1; j < 8; j++)
                {
                    X = X < Math.Abs(element.Nodes[j].X - element.Nodes[i].X) ? Math.Abs(element.Nodes[j].X - element.Nodes[i].X) : X;
                    Y = Y < Math.Abs(element.Nodes[j].Y - element.Nodes[i].Y) ? Math.Abs(element.Nodes[j].Y - element.Nodes[i].Y) : Y;
                    Z = Z < Math.Abs(element.Nodes[j].Z - element.Nodes[i].Z) ? Math.Abs(element.Nodes[j].Z - element.Nodes[i].Z) : Z;
                }
            }

            double pointX = minX + X / 2;
            double pointY = minY + Y / 2;
            double pointZ = minZ + Z / 2;

            return new double[] { pointX, pointY, pointZ };

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
