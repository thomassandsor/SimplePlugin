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

            CalculateSumCurrency();


            /************* --------------    Call Functions   ------------------------**************/

        }

        /************* --------------    Begin Custom Functions   ------------------------**************/
        private void CalculateSumCurrency()
        {
            // Obtain the values of the integer fields
            Money fieldA = RecordAfterUpdate.GetAttributeValue<Money>("plugin_currencya");
            Money fieldB = RecordAfterUpdate.GetAttributeValue<Money>("plugin_currencyb");

            // Calculate the sum of the currency fields
            decimal sumCurrency = fieldA.Value + fieldB.Value;

            // Create a new money object for the calculated value
            Money sumMoney = new Money(sumCurrency);

            // Update the record with the new field
            Entity updatedEntity = new Entity(RecordAfterUpdate.LogicalName, RecordAfterUpdate.Id);
            updatedEntity["plugin_resultcurrencycalculation"] = sumMoney;
            service.Update(updatedEntity);
        }


        /************* --------------    End Custom Functions  ------------------------**************/


    }
}
