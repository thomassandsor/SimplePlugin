using System;
using System.Text.RegularExpressions;
using Microsoft.Xrm.Sdk;

namespace SimplePlugin
{
    public class SimplePlugin : IPlugin
    {
        IOrganizationService service;
        Entity RecordAfterUpdate;
        IPluginExecutionContext context;
        ITracingService tracingService;

        public void Execute(IServiceProvider serviceProvider)
        {
            context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            service = ((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory))).CreateOrganizationService(context.UserId);
            tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            RecordAfterUpdate = context.PostEntityImages["PostImageMain"];

            /* --------------    Call Functions   ------------------------*/

            RegExString();

            /* --------------    Call Functions   ------------------------*/
        }

        private void RegExString()
        {
            // Get the value of the input field and apply the regular expression
            var inputString = RecordAfterUpdate.GetAttributeValue<string>("plugin_stringregexbefore");
            var regexResult = Regex.Replace(inputString, @"\s+", string.Empty).ToLower();

            // Create a new entity with the regex result and update the output field
            Entity updatedEntity = new Entity(RecordAfterUpdate.LogicalName, RecordAfterUpdate.Id);
            updatedEntity["plugin_stringregexafter"] = regexResult;
            service.Update(updatedEntity);
        }
    }
}
