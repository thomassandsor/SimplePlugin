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

            /************* --------------    Call Functions   ------------------------**************/

            CalculateSumInteger();


            /************* --------------    Call Functions   ------------------------**************/

        }

        /************* --------------    Begin Custom Functions   ------------------------**************/
        private void CalculateSumInteger()
        {
            // Obtain the values of the integer fields
            int fieldA = RecordAfterUpdate.GetAttributeValue<int>("plugin_integera");
            int fieldB = RecordAfterUpdate.GetAttributeValue<int>("plugin_integerb");

            // Calculate the sum of the integer fields
            int SumInteger = fieldA + fieldB;

            // Updating the record with the new field ONLY. Don't update alle fields on a record, because it can cause infinite loop. 
            Entity updatedEntity = new Entity(RecordAfterUpdate.LogicalName, RecordAfterUpdate.Id);
            updatedEntity["plugin_resultintegercalculation"] = SumInteger;
            service.Update(updatedEntity);
        }


        /************* --------------    End Custom Functions  ------------------------**************//


    }
}
