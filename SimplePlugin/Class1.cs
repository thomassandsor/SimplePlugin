﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


                //Call Custom Function
                //AccountMath();

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}