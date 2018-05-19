﻿using System;
using System.Collections.Generic;
using System.Text;
using ISAAR.MSolve.Analyzers;
using ISAAR.MSolve.Discretization.Interfaces;
using ISAAR.MSolve.IGA;
using ISAAR.MSolve.IGA.Entities;
using ISAAR.MSolve.IGA.Entities.Loads;
using ISAAR.MSolve.IGA.Readers;
using ISAAR.MSolve.Numerical.LinearAlgebra;
using ISAAR.MSolve.Problems;
using ISAAR.MSolve.Solvers.Interfaces;
using ISAAR.MSolve.Solvers.Skyline;
using Xunit;

namespace ISAAR.MSolve.Tests.IGA
{
    public class IsogeometricBenchmarks
    {
        //[Fact]
        //public void IsogeometricPlateWithHole()
        //{
        //    // Model
        //    VectorExtensions.AssignTotalAffinityCount();
        //    Model model = new Model();
        //    ModelCreator modelCreator = new ModelCreator(model);
        //    string filename = "C:\\Users\\user\\Desktop\\MSolve-master\\ISAAR.MSolve.SamplesConsole\\IGA\\Input Files\\PlateWithHoleIGAFEM.txt";
        //    IsogeometricReader modelReader = new IsogeometricReader(modelCreator, filename);
        //    modelReader.CreateModelFromFile();

        //    // Forces and Boundary Conditions
        //    // Loading Conditions - Pressure
        //    Value plateNeumannValue = delegate (double x, double y, double z)
        //    {
        //        double r = Math.Sqrt(x * x + y * y);
        //        double theta = Math.Atan(y / x);

        //        double c2t = Math.Cos(2 * theta);
        //        double c4t = Math.Cos(4 * theta);
        //        double s2t = Math.Sin(2 * theta);
        //        double s4t = Math.Sin(4 * theta);
        //        double fac1 = Math.Pow((1 / r), 2);
        //        double fac2 = fac1 * fac1;
        //        double[] exactStress = new double[3];
        //        exactStress[0] = 1 - fac1 * (1.5 * c2t + c4t) + 1.5 * fac2 * c4t;
        //        exactStress[1] = -fac1 * (0.5 * c2t - c4t) - 1.5 * fac2 * c4t;
        //        exactStress[2] = -fac1 * (0.5 * s2t + s4t) + 1.5 * fac2 * s4t;
        //        return exactStress;
        //    };

        //    model.PatchesDictionary[0].EdgesDictionary[3].LoadingConditions.Add(new NeumannBoundaryCondition(plateNeumannValue));

        //    // Boundary Conditions - Dirichlet
        //    foreach (ControlPoint controlPoint in model.PatchesDictionary[0].EdgesDictionary[0].ControlPointsDictionary.Values)
        //        model.ControlPointsDictionary[controlPoint.ID].Constrains.Add(DOFType.Y);

        //    foreach (ControlPoint controlPoint in model.PatchesDictionary[0].EdgesDictionary[1].ControlPointsDictionary.Values)
        //        model.ControlPointsDictionary[controlPoint.ID].Constrains.Add(DOFType.X);

        //    model.ConnectDataStructures();

        //    // Solvers
        //    var linearSystems = new Dictionary<int, ILinearSystem>(); //I think this should be done automatically
        //    linearSystems[0] = new SkylineLinearSystem(0, model.PatchesDictionary[0].Forces);
        //    SolverSkyline solver = new SolverSkyline(linearSystems[0]);
        //    ProblemStructural provider = new ProblemStructural(model, linearSystems);
        //    LinearAnalyzer analyzer = new LinearAnalyzer(solver, linearSystems);
        //    StaticAnalyzer parentAnalyzer = new StaticAnalyzer(provider, analyzer, linearSystems);

        //    parentAnalyzer.BuildMatrices();
        //    parentAnalyzer.Initialize();
        //    parentAnalyzer.Solve();

        //    double[] loadVectorExpected = new double[]
        //    {
        //        0, 0.0468750000000000, 0, 0.0937500000000001, 0, 0.140625000000000, 0, 0.187500000000000, 0,
        //        0.187500000000000, 0, 0.187500000000000, 0, 0.187500000000000, 0, 0.187500000000000, 0,
        //        0.140625000000000, 0, 0.0937500000000000, 0.0625000000000000, .0468750000000000, 0, 0, 0, 0, 0, 0, 0, 0,
        //        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.125000000000000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //        0, 0, 0, 0, 0, 0.187500000000000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //        0.250000000000000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.250000000000000, 0,
        //        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.250000000000000, 0, 0, 0, 0, 0, 0, 0, 0,
        //        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.250000000000000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //        0, 0, 0, 0, 0, 0, 0.250000000000000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //        0.187500000000000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.125000000000000, 0,
        //        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.0625000000000000, -0.0625000000000000, 0,
        //        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -0.125000000000000, 0, 0, 0, 0, 0, 0, 0, 0,
        //        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -0.187500000000000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //        0, 0, 0, 0, 0, 0, -0.250000000000000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //        -0.250000000000000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -0.250000000000000,
        //        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -0.250000000000000, 0, 0, 0, 0, 0, 0, 0,
        //        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -0.250000000000000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //        0, 0, 0, 0, 0, 0, 0, -0.187500000000000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //        -0.125000000000000, -0.0468750000000000, 0, -0.0937500000000001, 0, -0.140625000000000, 0,
        //        -0.187500000000000, 0, -0.187500000000000, 0, -0.187500000000000, 0, -0.187500000000000, 0,
        //        -0.187500000000000, 0, -0.140625000000000, 0, -0.0937500000000000, 0, -0.0468750000000000,
        //        -0.0625000000000000
        //    };
        //    for (int i = 0; i < loadVectorExpected.Length; i++)
        //        Assert.Equal(loadVectorExpected[i], model.PatchesDictionary[0].Forces[i]);

        //    double[] displacementVectorExpected = new double[]
        //    {
        //        0, 0, 0.00481735260579304, 0.101536431169020, 0.117006286633555, 0.142730387934754, 0.230975270826430,
        //        0.169258885940461, 0.345563993403474, 0.176778919796814, 0.448716642887631, 0.180910534896632,
        //        0.550479325381047, 0.183005024767851, 0.649342949217821, 0.184996535692699, 0.747170602650319,
        //        0.187426569251078, 0.812081663430104, 0.189671078791389, 0.844578703549238, 0.190742691978957, 0, 0,
        //        0.0343358352855472, 0.0869091741469076, 0.113006000211281, 0.138341342772621, 0.240934527489856,
        //        0.151319906443669, 0.346476647682297, 0.158379753108970, 0.453364249424311, 0.154903849981601,
        //        0.552708656027396, 0.153893030577380, 0.652172025843557, 0.150290225782017, 0.749225699049430,
        //        0.148786070336517, 0.813870398527037, 0.147676139474594, 0.846012296713418, 0.147431529313174, 0, 0,
        //        0.0546370341900025, 0.0646470803151172, 0.130174237814789, 0.117765534539006, 0.249761492313803,
        //        0.120141559498393, 0.357006030743452, 0.116073282763496, 0.459951043118726, 0.104730512864069,
        //        0.560207002185981, 0.0930405048200049, 0.657758546850001, 0.0815383734421303, 0.754234029566131,
        //        0.0703900466322226, 0.817510652420797, 0.0639834481963511, 0.849125620694754, 0.0609501990474154, 0, 0,
        //        0.0663065269196735, 0.0431429813017755, 0.150224593113968, 0.0800847319501943, 0.264923971488804,
        //        0.0696055276178762, 0.370395510563081, 0.0498704443168015, 0.472112765040618, 0.0250621791249483,
        //        0.570670808476465, 0.000252684367590920, 0.667314084252187, -0.0245097133597309, 0.761947564807992,
        //        -0.0480760929913125, 0.824208991984592, -0.0630898690091334, 0.854994473500896, -0.0701040293269889, 0,
        //        0, 0.0726522238074209, 0.0252859363229946, 0.163658097522906, 0.0418387105855262, 0.276346719833494,
        //        0.0156463792724173, 0.381010946591694, -0.0193073836213833, 0.481546438471903, -0.0573038095998339,
        //        0.579510734687630, -0.0952162579717554, 0.675102671074280, -0.132238065754921, 0.768706993732314,
        //        -0.168223999624793, 0.829819172787761, -0.191448764951708, 0.859927710262354, -0.203001182629023, 0, 0,
        //        0.0769170177386975, 0.00828878878428267, 0.170865666037976, 0.00358201333404808, 0.282832566762164,
        //        -0.0401467842961048, 0.386819537210809, -0.0898822294673713, 0.487246684332804, -0.141324643392653,
        //        0.584902853540035, -0.191800878637547, 0.680141504085267, -0.241107894768096, 0.772808807687865,
        //        -0.289017312716502, 0.833017754434794, -0.320647952457229, 0.862588996111137, -0.336495777324850, 0, 0,
        //        0.0789921298358280, -0.00925699205989430, 0.172629339315303, -0.0347023041377422, 0.283596859619257,
        //        -0.0962878167286482, 0.387732573101267, -0.160717107666940, 0.488595729739037, -0.225493756167380,
        //        0.586661393644265, -0.288653038732293, 0.681895403722327, -0.350058879885451, 0.774122316582081,
        //        -0.409973924359997, 0.833793175595805, -0.449593718353889, 0.863115108432098, -0.469433075274349, 0, 0,
        //        0.0785059735053303, -0.0277228831535068, 0.168787010413703, -0.0726802951687856, 0.278985755768258,
        //        -0.151674772701902, 0.383972207363583, -0.230633996685856, 0.485972779674298, -0.308955058924067,
        //        0.584951619336036, -0.384858010625595, 0.680778155259177, -0.458585185787591, 0.773199694039620,
        //        -0.530421360704826, 0.832979065198302, -0.577712662488008, 0.862399113164262, -0.601152591503087, 0, 0,
        //        0.0747289195735525, -0.0469759966146647, 0.159395794264770, -0.109602603530024, 0.269626777925090,
        //        -0.205306832389377, 0.376199989740503, -0.298887504243407, 0.479851047169427, -0.390956420511867,
        //        0.580388054133587, -0.479923182635044, 0.677413341865712, -0.566225567921330, 0.770963964269808,
        //        -0.650045087426064, 0.831667056492214, -0.704760066662262, 0.861572691557201, -0.731636244436304, 0, 0,
        //        0.0696053275738199, -0.0599229556035623, 0.149642355767952, -0.132853873144325, 0.260870019864993,
        //        -0.239401595424122, 0.368677877648884, -0.342847808744090, 0.473898407341409, -0.444282048296719,
        //        0.575807140995728, -0.542265323035814, 0.674110868803648, -0.637146513094087, 0.769267573073749,
        //        -0.729085102142054, 0.831079618576272, -0.788900519098016, 0.861511413054596, -0.818185803807449, 0, 0,
        //        0.0662913560588443, -0.0662913560588467, 0.144007887341310, -0.144007887341317, 0.256009158510726,
        //        -0.256009158510743, 0.364371862714365, -0.364371862714394, 0.470603722981923, -0.470603722981956,
        //        0.573134250633869, -0.573134250633918, 0.672362139548220, -0.672362139548276, 0.768441935611751,
        //        -0.768441935611819, 0.830893190156285, -0.830893190156353, 0.861534153371889, -0.861534153371961, 0, 0,
        //        0.0599229556035606, -0.0696053275738224, 0.132853873144316, -0.149642355767959, 0.239401595424106,
        //        -0.260870019865009, 0.342847808744059, -0.368677877648910, 0.444282048296688, -0.473898407341441,
        //        0.542265323035763, -0.575807140995772, 0.637146513094032, -0.674110868803702, 0.729085102141987,
        //        -0.769267573073812, 0.788900519097947, -0.831079618576338, 0.818185803807377, -0.861511413054663, 0, 0,
        //        0.0469759966146622, -0.0747289195735540, 0.109602603530015, -0.159395794264777, 0.205306832389359,
        //        -0.269626777925103, 0.298887504243379, -0.376199989740527, 0.390956420511831, -0.479851047169454,
        //        0.479923182634998, -0.580388054133625, 0.566225567921274, -0.677413341865761, 0.650045087425999,
        //        -0.770963964269865, 0.704760066662191, -0.831667056492276, 0.731636244436233, -0.861572691557264, 0, 0,
        //        0.0277228831535044, -0.0785059735053313, 0.0726802951687764, -0.168787010413708, 0.151674772701885,
        //        -0.278985755768268, 0.230633996685827, -0.383972207363600, 0.308955058924031, -0.485972779674321,
        //        0.384858010625548, -0.584951619336064, 0.458585185787536, -0.680778155259213, 0.530421360704762,
        //        -0.773199694039665, 0.577712662487938, -0.832979065198354, 0.601152591503013, -0.862399113164314, 0, 0,
        //        0.00925699205989171, -0.0789921298358288, 0.0347023041377332, -0.172629339315305, 0.0962878167286295,
        //        -0.283596859619264, 0.160717107666912, -0.387732573101279, 0.225493756167343, -0.488595729739054,
        //        0.288653038732249, -0.586661393644287, 0.350058879885396, -0.681895403722353, 0.409973924359935,
        //        -0.774122316582113, 0.449593718353820, -0.833793175595841, 0.469433075274277, -0.863115108432139, 0, 0,
        //        -0.00828878878428549, -0.0769170177386974, -0.00358201333405704, -0.170865666037978, 0.0401467842960861,
        //        -0.282832566762168, 0.0898822294673431, -0.386819537210816, 0.141324643392617, -0.487246684332815,
        //        0.191800878637501, -0.584902853540049, 0.241107894768043, -0.680141504085285, 0.289017312716439,
        //        -0.772808807687886, 0.320647952457161, -0.833017754434816, 0.336495777324779, -0.862588996111161, 0, 0,
        //        -0.0252859363229973, -0.0726522238074205, -0.0418387105855353, -0.163658097522906, -0.0156463792724363,
        //        -0.276346719833495, 0.0193073836213553, -0.381010946591697, 0.0573038095997977, -0.481546438471908,
        //        0.0952162579717106, -0.579510734687638, 0.132238065754867, -0.675102671074290, 0.168223999624731,
        //        -0.768706993732327, 0.191448764951642, -0.829819172787775, 0.203001182628956, -0.859927710262370, 0, 0,
        //        -0.0431429813017784, -0.0663065269196726, -0.0800847319502038, -0.150224593113966, -0.0696055276178953,
        //        -0.264923971488802, -0.0498704443168290, -0.370395510563079, -0.0250621791249847, -0.472112765040618,
        //        -0.000252684367635323, -0.570670808476466, 0.0245097133596780, -0.667314084252189, 0.0480760929912528,
        //        -0.761947564807998, 0.0630898690090683, -0.824208991984599, 0.0701040293269211, -0.854994473500903, 0,
        //        0, -0.0646470803151200, -0.0546370341900009, -0.117765534539017, -0.130174237814784, -0.120141559498412,
        //        -0.249761492313798, -0.116073282763524, -0.357006030743445, -0.104730512864105, -0.459951043118720,
        //        -0.0930405048200491, -0.560207002185975, -0.0815383734421825, -0.657758546849996, -0.0703900466322825,
        //        -0.754234029566125, -0.0639834481964164, -0.817510652420792, -0.0609501990474828, -0.849125620694749, 0,
        //        0, -0.0869091741469109, -0.0343358352855440, -0.138341342772632, -0.113006000211275, -0.151319906443688,
        //        -0.240934527489847, -0.158379753108998, -0.346476647682287, -0.154903849981637, -0.453364249424300,
        //        -0.153893030577424, -0.552708656027385, -0.150290225782068, -0.652172025843546, -0.148786070336578,
        //        -0.749225699049420, -0.147676139474657, -0.813870398527027, -0.147431529313241, -0.846012296713409, 0,
        //        0, -0.101536431169024, -0.00481735260578848, -0.142730387934765, -0.117006286633547, -0.169258885940480,
        //        -0.230975270826420, -0.176778919796841, -0.345563993403462, -0.180910534896668, -0.448716642887619,
        //        -0.183005024767895, -0.550479325381034, -0.184996535692750, -0.649342949217807, -0.187426569251139,
        //        -0.747170602650306, -0.189671078791453, -0.812081663430091, -0.190742691979024, -0.844578703549225,
        //    };
        //    for (int i = 0; i < loadVectorExpected.Length; i++) Assert.Equal(displacementVectorExpected[i], linearSystems[0].Solution[i], 6);
        //}

        [Fact]
        public void IsogeometricQuadraticCantilever2D()
        {
            // Model
            VectorExtensions.AssignTotalAffinityCount();
            Model model = new Model();
            ModelCreator modelCreator = new ModelCreator(model);
            string filename = "..\\..\\..\\IGA\\InputFiles\\Cantilever2D.txt";
            IsogeometricReader modelReader = new IsogeometricReader(modelCreator, filename);
            modelReader.CreateModelFromFile();

            // Forces and Boundary Conditions
            foreach (ControlPoint controlPoint in model.PatchesDictionary[0].EdgesDictionary[1].ControlPointsDictionary.Values)
                model.Loads.Add(new Load() { Amount = -100, ControlPoint = model.ControlPoints[controlPoint.ID], DOF = DOFType.Y });

            // Boundary Conditions - Dirichlet
            foreach (ControlPoint controlPoint in model.PatchesDictionary[0].EdgesDictionary[0].ControlPointsDictionary.Values)
            {
                model.ControlPointsDictionary[controlPoint.ID].Constrains.Add(DOFType.X);
                model.ControlPointsDictionary[controlPoint.ID].Constrains.Add(DOFType.Y);
            }
            model.ConnectDataStructures();

            // Solvers
            var linearSystems = new Dictionary<int, ILinearSystem>(); //I think this should be done automatically
            linearSystems[0] = new SkylineLinearSystem(0, model.PatchesDictionary[0].Forces);
            SolverSkyline solver = new SolverSkyline(linearSystems[0]);
            ProblemStructural provider = new ProblemStructural(model, linearSystems);
            LinearAnalyzer analyzer = new LinearAnalyzer(solver, linearSystems);
            StaticAnalyzer parentAnalyzer = new StaticAnalyzer(provider, analyzer, linearSystems);

            parentAnalyzer.BuildMatrices();
            parentAnalyzer.Initialize();
            parentAnalyzer.Solve();


            //Test for Load Vector
            int[] loadVectorExpected = new int[108];
            loadVectorExpected[110 - 13] = -100;
            loadVectorExpected[112 - 13] = -100;
            loadVectorExpected[114 - 13] = -100;
            loadVectorExpected[116 - 13] = -100;
            loadVectorExpected[118 - 13] = -100;
            loadVectorExpected[120 - 13] = -100;

            for (int i = 0; i < loadVectorExpected.Length; i++)
                Assert.Equal(loadVectorExpected[i], model.PatchesDictionary[0].Forces[i]);

            //Test fro Displacement Vector
            double[] displacementVectorExpected = new double[]
            {
                -0.0368738351302207, -0.0131499592237545, -0.0236187564608231,
                -0.00591458123276355, -0.00760062703427646, -0.000566403168697526, 0.00760062703427637,
                -0.000566403168697477, 0.0236187564608231, -0.00591458123276345, 0.0368738351302207,
                -0.0131499592237545, -0.0987784914203237, -0.0861605620323561, -0.0733694959613918, -0.0825449187386709,
                -0.0237898071790881, -0.0779453691704672, 0.0237898071790880, -0.0779453691704673, 0.0733694959613916,
                -0.0825449187386711, 0.0987784914203237, -0.0861605620323562, -0.153074952778768, -0.220348665003477,
                -0.112824242793523, -0.216334418527862, -0.0373252643977994, -0.212814901275071, 0.0373252643977993,
                -0.212814901275071, 0.112824242793523, -0.216334418527862, 0.153074952778767, -0.220348665003477,
                -0.197987950979100, -0.403380316332308, -0.146994646367387, -0.400466656395154, -0.0484809128603281,
                -0.397304037977320, 0.0484809128603277, -0.397304037977320, 0.146994646367387, -0.400466656395153,
                0.197987950979101, -0.403380316332307, -0.233979100719325, -0.627050378576386, -0.173875915391434,
                -0.624621548530984, -0.0575789187734497, -0.622325573362235, 0.0575789187734493, -0.622325573362235,
                0.173875915391433, -0.624621548530985, 0.233979100719325, -0.627050378576387, -0.261062696370552,
                -0.882149456584904, -0.194054136046524, -0.880534402771693, -0.0641987082249154, -0.878857804164279,
                0.0641987082249148, -0.878857804164278, 0.194054136046524, -0.880534402771692, 0.261062696370551,
                -0.882149456584903, -0.278878750048815, -1.16007242947546, -0.207837054153937, -1.15891585124500,
                -0.0689580379467459, -1.15768229264197, 0.0689580379467447, -1.15768229264197, 0.207837054153936,
                -1.15891585124500, 0.278878750048814, -1.16007242947546, -0.288294648281518, -1.45007762926002,
                -0.213983781845837, -1.45008375093483, -0.0704137097578954, -1.45093051606850, 0.0704137097578944,
                -1.45093051606850, 0.213983781845836, -1.45008375093483, 0.288294648281518, -1.45007762926002,
                -0.289593315112231, -1.60204696081608, -0.214138050751406, -1.60097881234833, -0.0706978642320891,
                -1.59760921075339, 0.0706978642320882, -1.59760921075338, 0.214138050751405, -1.60097881234833,
                0.289593315112230, -1.60204696081608
            };

            for (int i = 0; i < displacementVectorExpected.Length; i++)
                Assert.Equal(displacementVectorExpected[i], linearSystems[0].Solution[i], 6);

        }

        [Fact]
        public void TSplinesShellsBenchmark()
        {
            VectorExtensions.AssignTotalAffinityCount();
            Model model = new Model();
            string filename = "..\\..\\..\\IGA\\InputFiles\\tspline.iga";
            IGAFileReader modelReader = new IGAFileReader(model, filename);
            modelReader.CreateTSplineShellsModelFromFile();
            model.Loads.Add(new Load() { Amount = -100, ControlPoint = model.ControlPointsDictionary[2], DOF = DOFType.Y });
            model.ConnectDataStructures();

            //var linearSystems = new Dictionary<int, ILinearSystem>();
            //linearSystems[0] = new SkylineLinearSystem(0, model.PatchesDictionary[0].Forces);
            //SolverSkyline solver = new SolverSkyline(linearSystems[0]);
            //ProblemStructural provider = new ProblemStructural(model, linearSystems);
            //LinearAnalyzer analyzer = new LinearAnalyzer(solver, linearSystems);
            //StaticAnalyzer parentAnalyzer = new StaticAnalyzer(provider, analyzer, linearSystems);

            //parentAnalyzer.BuildMatrices();
            //parentAnalyzer.Initialize();
            //parentAnalyzer.Solve();
        }

    }
}
