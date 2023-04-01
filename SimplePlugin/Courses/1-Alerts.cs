using System;
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

            tracingService.Trace("You are awesome! Congratulations on the first plugin");


            /* --------------    End Custom Functions   ------------------------*/

        }
    }
}
