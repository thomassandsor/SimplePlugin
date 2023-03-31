using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace SimplePlugin
{
    public class SimplePlugin : IPlugin
    {
        IOrganizationService service;
        Entity currentEntity;
        Entity preImage;
        IPluginExecutionContext context;
        ITracingService tracingService;

        public void Execute(IServiceProvider serviceProvider)
        {
            context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            service = ((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory))).CreateOrganizationService(context.UserId);
            tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            currentEntity = (Entity)context.InputParameters["Target"];
            tracingService.Trace("before image");
            tracingService.Trace("target: " + currentEntity.LogicalName);
            



            if (context.PreEntityImages.Contains("PreImage2"))
            {
                preImage = context.PreEntityImages["PreImage2"];
                tracingService.Trace("preimagebefore: " + context.PreEntityImages["PreImage2"].GetAttributeValue<int>("plugin_integera"));
            }
            else
            {
                tracingService.Trace("PreImage not found");
                return;
            }
            //preImage = context.PreEntityImages["PreImage"];

                //Enter Custom Code
                tracingService.Trace("First tracing test");
                throw new InvalidPluginExecutionException("test");

                CalculateSum();

        }
        private void CalculateSum()
        {
            int sum = 0;
            tracingService.Trace("Beginning Sum: " + sum);

            tracingService.Trace("currentEntity.LogicalName: " + preImage.LogicalName);
            tracingService.Trace("currentEntity.Attributes: " + string.Join(",", preImage.Attributes.Keys));

            // Obtain the values of the integer fields
            int fieldA = preImage.GetAttributeValue<int>("plugin_integera");
            tracingService.Trace("Field A: " + fieldA);
            int fieldB = preImage.GetAttributeValue<int>("plugin_integerb");
            tracingService.Trace("Field B: " + fieldB);
            
            // Calculate the sum of the integer fields
            sum = fieldA + fieldB;
            tracingService.Trace("Total SUm: " + sum);

            //currentEntity["plugin_resultintegercalculation"] = sum;

            // Update the record in Dynamics 365
            //service.Update(currentEntity);


            Entity updatedEntity = new Entity(preImage.LogicalName, preImage.Id);
            updatedEntity["plugin_resultintegercalculation"] = sum;
            service.Update(updatedEntity);
        }
    }
}
