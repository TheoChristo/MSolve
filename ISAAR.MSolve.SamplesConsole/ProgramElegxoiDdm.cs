using ISAAR.MSolve.Analyzers;
using ISAAR.MSolve.Logging;
using ISAAR.MSolve.Numerical.LinearAlgebra;//using ISAAR.MSolve.Matrices;
using ISAAR.MSolve.PreProcessor;
using ISAAR.MSolve.Problems;
using ISAAR.MSolve.Solvers.Skyline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ISAAR.MSolve.FEM; 
using ISAAR.MSolve.FEM.Elements; 
using ISAAR.MSolve.FEM.Entities; 
using ISAAR.MSolve.FEM.Materials; 
using ISAAR.MSolve.Materials; 
using ISAAR.MSolve.SamplesConsole; 
using ISAAR.MSolve.Solvers.Interfaces; 

namespace ISAAR.MSolve.SamplesConsole
{
    public class ProgramElegxoiDdm
    {
        public static void SolveRVEExample()
        {
            VectorExtensions.AssignTotalAffinityCount();
            Model model = new Model();
            model.SubdomainsDictionary.Add(1, new Subdomain() { ID = 1 });

            // EPILOGH MONTELOU
            int model__builder_choice;
            model__builder_choice =2;   // 9 einai to megalo me to renumbering pou tsekaretai

            
            if (model__builder_choice == 1) // 
            { DddmExamplesBuilder.Reference1RVEExample10000(model); }
            if (model__builder_choice == 2) // 
            { DddmExamplesBuilder.Reference1RVEExample10000_Hexaonly(model); }
            if (model__builder_choice == 8) // 
            { RVEExamplesBuilder.Reference2RVEExample10000withRenumbering(model); }
            //if (model__builder_choice == 4) // 
            //{ DddmExamplesBuilder.Reference1RVEExample10000_Hexaonly(model); }

            //Α




            //Β
            //for (int i1 = 0; i1 < model.ElementsDictionary.Count; i1++)
            //{ model.ElementsDictionary[i1 + 1].ID += -1; }
            //for (int i1 = 0; i1 < model.NodesDictionary.Count; i1++)
            //{ model.NodesDictionary[i1 + 1].ID += -1; }

            //C
            model.SubdomainsDictionary[1].ID = 0;
            Subdomain subdomain_ini = model.SubdomainsDictionary[1];
            model.SubdomainsDictionary.Remove(1);
            model.SubdomainsDictionary.Add(0, subdomain_ini);

            Dictionary<int, Element> ElementsDictionary_2 = new Dictionary<int, Element>(model.ElementsDictionary.Count);
            for (int i1 = 0; i1 < model.ElementsDictionary.Count; i1++)
            {
                ElementsDictionary_2.Add(model.ElementsDictionary[i1 + 1].ID - 1, model.ElementsDictionary[i1 + 1]);
                ElementsDictionary_2[model.ElementsDictionary[i1 + 1].ID - 1].ID += -1;
            }
            int nElement = model.ElementsDictionary.Count;
            for (int i1 = 0; i1 < nElement; i1++)
            {
                model.ElementsDictionary.Remove(i1 + 1);
                model.SubdomainsDictionary[0].ElementsDictionary.Remove(i1 + 1);
            }
            for (int i1 = 0; i1 < nElement; i1++)
            {
                model.ElementsDictionary.Add(ElementsDictionary_2[i1].ID, ElementsDictionary_2[i1]);
                model.SubdomainsDictionary[0].ElementsDictionary.Add(ElementsDictionary_2[i1].ID, ElementsDictionary_2[i1]);
            }

            Dictionary<int, Node> NodesDictionary_2 = new Dictionary<int, Node>(model.NodesDictionary.Count);
            for (int i1 = 0; i1 < model.NodesDictionary.Count; i1++)
            {
                NodesDictionary_2.Add(model.NodesDictionary[i1 + 1].ID - 1, model.NodesDictionary[i1 + 1]);
                NodesDictionary_2[model.NodesDictionary[i1 + 1].ID - 1].ID += -1;
            }

            int nNode = model.NodesDictionary.Count;
            for (int i1 = 0; i1 < nNode; i1++)
            {
                model.NodesDictionary.Remove(i1 + 1);
                //model.SubdomainsDictionary[0].NodesDictionary.Remove(i1 + 1);
            }
            for (int i1 = 0; i1 < nNode; i1++)
            {
                model.NodesDictionary.Add(NodesDictionary_2[i1].ID, NodesDictionary_2[i1]);
                //model.SubdomainsDictionary[0].NodesDictionary.Add(NodesDictionary_2[i1].ID, NodesDictionary_2[i1]);
            }

            model.ConnectDataStructures();

            //PITHANH PROSTHIKI
            AutomaticDomainDecomposer domainDecomposer = new AutomaticDomainDecomposer(model, 2); //2o orisma arithmoos subdomains
            domainDecomposer.UpdateModel();


            //comment section 1 palaia version
            //SolverSkyline solver = new SolverSkyline(model);
            //ProblemStructural provider = new ProblemStructural(model, solver.SubdomainsDictionary);

            var linearSystems = new Dictionary<int, ILinearSystem>(); //I think this should be done automatically 
            linearSystems[1] = new SkylineLinearSystem(1, model.Subdomains[0].Forces);
            SolverSkyline solver = new SolverSkyline(linearSystems[1]);
            ProblemStructural provider = new ProblemStructural(model, linearSystems);


            // PARADEIGMA A: LinearAnalyzer analyzer = new LinearAnalyzer(solver, solver.SubdomainsDictionary);
            LinearAnalyzer analyzer = new LinearAnalyzer(solver, linearSystems);
            //---------------------------------------------------------------------------------------------------------------------------------

            // PARADEIGMA B: Analyzers.NewtonRaphsonNonLinearAnalyzer3 analyzer = new NewtonRaphsonNonLinearAnalyzer3(solver, solver.SubdomainsDictionary, provider, 17, model.TotalDOFs);//1. increments einai to 17 (arxika eixame thesei2 26 incr)
            //NewtonRaphsonNonLinearAnalyzer analyzer = new NewtonRaphsonNonLinearAnalyzer(solver, linearSystems, provider, 10, 48);
            //---------------------------------------------------------------------------------------------------------------------------------


            StaticAnalyzer parentAnalyzer = new StaticAnalyzer(provider, analyzer, linearSystems);

            analyzer.LogFactories[1] = new LinearAnalyzerLogFactory(new int[] { 47 });







            //comment section 2 palaia version
            //int increments = 1;
            //Analyzers.NewtonRaphsonNonLinearAnalyzer3 analyzer = new NewtonRaphsonNonLinearAnalyzer3(solver, solver.SubdomainsDictionary, provider, increments, model.TotalDOFs);//1. increments einai to 1 (arxika eixame thesei2 26 incr)
            //StaticAnalyzer parentAnalyzer = new StaticAnalyzer(provider, analyzer, solver.SubdomainsDictionary);
            //analyzer.SetMaxIterations = 100;
            //analyzer.SetIterationsForMatrixRebuild = 1;



            parentAnalyzer.BuildMatrices();
            parentAnalyzer.Initialize();
            parentAnalyzer.Solve();


        }

        //static void Main(string[] args)
        //{
        //    SolveRVEExample(); //|
        //}

    }
}
