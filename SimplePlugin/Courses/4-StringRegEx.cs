using System;
using System.Text.RegularExpressions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

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

            /* --------------    Start Custom Functions   ------------------------*/

            tracingService.Trace("You are awesome!");
            RegExString();

            /* --------------    End Custom Functions   ------------------------*/

        }

        /************* --------------    Begin Custom Functions   ------------------------**************/
        private void RegExString()
        {
            //String before regex
            tracingService.Trace($"Input string: {inputString}");

            // Get the value of the input field and apply the regular expression
            var inputString = RecordAfterUpdate.GetAttributeValue<string>("plugin_stringinput");
            var regexResult = Regex.Replace(inputString, @"\s+", string.Empty).ToLower();

            // Create a new entity with the regex result and update the output field
            var outputEntity = new Entity(RecordAfterUpdate.LogicalName, RecordAfterUpdate.Id);
            outputEntity["plugin_afterregex"] = regexResult;
            service.Update(outputEntity);

            //String after regex
            tracingService.Trace($"Output string: {regexResult}");

        }


        /************* --------------    End Custom Functions  ------------------------**************/


    }
}
