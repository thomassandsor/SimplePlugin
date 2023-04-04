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

            CalculateSumCurrency();
            CalculateSumInteger();
            RegExString();

            /* --------------    Call Functions   ------------------------*/
        }

        private void RegExString()
        {
            // Get the value of the input field and apply the regular expression
            var inputString = RecordAfterUpdate.GetAttributeValue<string>("plugin_stringregexbefore");
            var regexResult = Regex.Replace(inputString, @"\s+", string.Empty).ToLower();

            // Create a new entity with the regex result and update the output field
            var updatedEntity = new Entity(RecordAfterUpdate.LogicalName, RecordAfterUpdate.Id);
            updatedEntity["plugin_stringregexafter"] = regexResult;
            service.Update(updatedEntity);
        }

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
        private void CalculateSumCurrency()
        {
            tracingService.Trace("Inside");
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
    }
}