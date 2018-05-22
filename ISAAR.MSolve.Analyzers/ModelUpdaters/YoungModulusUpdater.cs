using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISAAR.MSolve.Analyzers.Interfaces;
using ISAAR.MSolve.FEM.Elements;
using ISAAR.MSolve.FEM.Entities;
using ISAAR.MSolve.FEM.Interfaces;

namespace ISAAR.MSolve.Analyzers.ModelUpdaters
{
    public class YoungModulusUpdater : IModelUpdater
    {
        private Model model;
        private double youngModulus;

        IList<IFiniteElement> listofFiniteElements;
        public YoungModulusUpdater(Model model, IStochasticCoefficientsProvider coefficientsProvider)
        {
            this.model = model;


        }
        public void UpdateModel<T>() where T:IFiniteElement
        {
            foreach (var element in model.Elements)
            {
                
            }
            throw new NotImplementedException();
        }

        public void UpdateModel()
        {
            foreach (var element in model.Elements.Where(e=> listofFiniteElements.Contains(e.ElementType.GetType().)))
            {
                element.ElementType.
            }
        }
    }
}

