using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompetencyMatrix.Infrastructure
{
    public class TrimmingModelBinder : ComplexTypeModelBinder
    {
        public TrimmingModelBinder(IDictionary<ModelMetadata, IModelBinder> propertyBinders) : base(propertyBinders)
        {
        }

        protected override void SetProperty(ModelBindingContext bindingContext, string modelName, ModelMetadata propertyMetadata, ModelBindingResult result)
        {
            if (result.Model is string)
            {
                string resultStr = (result.Model as string).Trim();
                result = ModelBindingResult.Success(resultStr);
            }

            base.SetProperty(bindingContext, modelName, propertyMetadata, result);
        }
    }
}
