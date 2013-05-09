using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Tournament.Infrastructure
{
    public class StringArrayModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            List<string> result = new List<string>();

            var modelName = bindingContext.ModelName;

            // assumes there will only be one field/parameter named the same (usual c# rules apply)
            var key = controllerContext.HttpContext.Request.Params.Keys.OfType<string>().FirstOrDefault(k => k.StartsWith(modelName, StringComparison.InvariantCultureIgnoreCase));

            // key needs to be checked
            if (key != null)
            {
                var val = bindingContext.ValueProvider.GetValue(key);
                result.AddRange(val.AttemptedValue.Split(','));
            }

            return result.ToArray();
        }
    }
}