using System;
using Microsoft.Xrm.Sdk;

namespace SimplePlugin
{
    public class SimplePlugin : IPlugin
    {
        IOrganizationService service;
        Entity currentEntity;
        IPluginExecutionContext context;
        ITracingService tracingService;

        public void Execute(IServiceProvider serviceProvider)
        {
            try
            {
                context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                service = ((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory))).CreateOrganizationService(context.UserId);
                tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
                tracingService.Trace("First tracing test");
                throw new InvalidPluginExecutionException("test");

            }
            catch (Exception ex)
            {
                tracingService.Trace(ex.Message);
                throw;
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
    }
}