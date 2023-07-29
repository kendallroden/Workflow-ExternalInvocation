using Dapr.Workflow;
using Dapr.Client;
using CheckoutServiceWorkflowSample.Models;

namespace CheckoutServiceWorkflowSample.Activities
{
    public class ProcessPaymentActivity : WorkflowActivity<PaymentRequest, PaymentResponse>
    {

        readonly ILogger _logger;

        public ProcessPaymentActivity(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ProcessPaymentActivity>();
        }


        public override async Task<PaymentResponse> RunAsync(WorkflowActivityContext context, PaymentRequest req)
        {
            // Use Dapr svc-to-svc to invoke Payment microservice 
            var invokeClient = DaprClient.CreateInvokeHttpClient(appId: "stripe-link");
            invokeClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            string[] sourceIds = { "cnon:card-nonce-ok", "cnon:card-nonce-declined" };
            var random = new Random();
            var n = random.Next(0, 2);
            try
            {
                // Simulate slow processing
                await Task.Delay(TimeSpan.FromSeconds(5));

                var data = new CreatePayment(
                    idempotency_key: Guid.NewGuid().ToString(),
                    amount_money: new AmountMoney(req.TotalCost, "USD"),
                    source_id: sourceIds[n],
                    autocomplete: true,
                    customer_id: req.Name + Guid.NewGuid().ToString(),
                    reference_id: context.InstanceId,
                    location_id: "L70EQ5Z85X6VE",
                    note: req.OrderItem,
                    app_fee_money: null);


                var response = await invokeClient.PostAsJsonAsync("/v2/payments", data);

                if (response.IsSuccessStatusCode)
                {
                    return new PaymentResponse(true);
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError(error);
                    return new PaymentResponse(false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public record AmountMoney(int amount, string currency);
        public record AppFeeMoney(int amount, string currency);
        public record CreatePayment(string idempotency_key, AmountMoney amount_money, string source_id, bool autocomplete, string customer_id,
        string reference_id, string location_id, string note, AppFeeMoney? app_fee_money);

    }
}