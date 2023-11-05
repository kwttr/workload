using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace workload_Models.ModelBinders
{
    public class CustomDoubleModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None) {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            try {
                var doubleValue = valueProviderResult.FirstValue;

                var parsedDouble = decimal.Parse(doubleValue);

                bindingContext.Result = ModelBindingResult.Success(parsedDouble);
            } catch (Exception ex) {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, ex.Message);
            }
            return Task.CompletedTask;
        }
    }
}
