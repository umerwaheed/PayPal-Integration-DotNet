// See https://aka.ms/new-console-template for more information

using System.Runtime.CompilerServices;
using PayPal;
using PayPal.Models.Requests;

Console.WriteLine("Select any Operation:");
Console.WriteLine("1. Authorization -> It wil return the Authorization Access Token");
Console.WriteLine("2. Create Any Plan");
Console.WriteLine("3. Show Details of plan -> input should be like 3,[plan id]");
Console.WriteLine("4. Create Subscription -> It will return the redirect URL");
Console.WriteLine("5. Suspend Subscription -> Input should be like 5,[Subscription id] ");
Console.WriteLine("6. Activate Subscription -> Input should be like 6,[Subscription id]");
Console.WriteLine("7. Cancel Subscription -> Input should be like 7,[Subscription id]");
Console.WriteLine("8. Refund Amount");
var userSelectedOption = Console.ReadLine();

var data = userSelectedOption?.Split(",");

var client = new PayPalClientApi();
var authorizationReponse = await client.GetAuthorizationRequest();
client.SetToken(authorizationReponse.access_token);

switch (data[0])
{
    case "1":
        var response = await client.GetAuthorizationRequest();
        client.SetToken(response.access_token);
        Console.WriteLine(response.access_token);
        break;
    case "2":
        var trialBillingCycle = new BillingCycle()
        {
            frequency = new Frequency()
            {
                interval_unit = "MONTH",
                interval_count = 1,
            },
            tenure_type = "TRIAL",
            sequence = 1,
            total_cycles = 1,
            pricing_scheme = new PricingScheme()
            {
                fixed_price = new FixedPrice()
                {
                    currency_code = "USD",
                    value = "10.00"
                }
            }

        };
        var regularBillingCycle = new BillingCycle()
        {

            frequency = new Frequency()
            {
                interval_unit = "MONTH",
                interval_count = 1,
            },
            tenure_type = "REGULAR",
            sequence = 2,
            total_cycles = 0,
            pricing_scheme = new PricingScheme()
            {
                fixed_price = new FixedPrice()
                {
                    currency_code = "USD",
                    value = "100.00"
                }
            }
        };

        var createPlanRequest = new CreatePlanRequest()
        {
            product_id = "1670568338", //Product Id
            name = "Technical Voice Plan",
            description = "Technical Voice Plan",
            status = "ACTIVE",
            billing_cycles = new List<BillingCycle>()
                            {
                                trialBillingCycle,
                                regularBillingCycle
                            },
            payment_preferences = new PaymentPreferences()
            {
                auto_bill_outstanding = true,
                setup_fee = new SetupFee()
                {
                    currency_code = "USD",
                    value = "0"
                },
                setup_fee_failure_action = "CONTINUE",
                payment_failure_threshold = 3
            }

        };

        var createPlanResponse = await client.CreatePlan(createPlanRequest);
        Console.WriteLine($"Generated Plan Id:{createPlanResponse.id}");
        break;
    case "3":
        var planDetails = await client.GetPlanDetails(data[1]);
        Console.WriteLine($"Plan Name:{planDetails.name}");
        break;
    case "4":
        var createSubscriptionRequest = new CreateSubscriptionRequest()
        {
            plan_id = "P-7MJ97580E9275283UMOJN2CQ",
            subscriber = new Subscriber()
            {
                email_address = "technicalvoice2013@outlook.com",
                name = new Name()
                {
                    full_name = $"Technical Voice",
                    given_name = "Technical",
                    surname = "Voice"
                },
                shipping_address = new ShippingAddress()
                {
                    name = new Name()
                    {
                        full_name = $"Technical Voice",
                        given_name = "Technical",
                        surname = "Voice"
                    },
                    address = new Address()
                    {
                        address_line_1 = "118-N Block",
                        country_code = "US",
                        postal_code = "21045"
                    }
                }
            },
            application_context = new ApplicationContext()
            {
                brand_name = "LocationsHub",
                locale = "en-US",
                payment_method = new PaymentMethod()
                {
                    payee_preferred = "IMMEDIATE_PAYMENT_REQUIRED",
                    payer_selected = "PAYPAL"
                },
                shipping_preference = "SET_PROVIDED_ADDRESS",
                user_action = "SUBSCRIBE_NOW",
                return_url = $"https://example.com/return", // Your app url success case
                cancel_url = $"https://example.com/return", // Your app url if user cancels
            }
        };

        var createSubscriptionResponse = await client.CreateSubscription(createSubscriptionRequest);
        var approvalUrl = createSubscriptionResponse.links.FirstOrDefault(x => x.rel == "approve");
        Console.WriteLine(approvalUrl.href);
        break;
    case "5":
        // Suspend Subscription
        await client.SuspendSubscription(data[1], new SubscriptionStatusChangeRequest()
        {
            reason = "Reason of the suspend"
        });
        break;
    case "6":
        // Activate Subscription
        await client.ActiveSubscription(data[1], new SubscriptionStatusChangeRequest()
        {
            reason = "Reason of the suspend"
        });
        break;
    case "7":
        // Cancel Subscription
        await client.CancelSubscription(data[1], new SubscriptionStatusChangeRequest()
        {
            reason = "Reason of the suspend"
        });
        break;
    case "8":
        // Refund the Amount
        await client.RefundSubscriptionAmount("https://api.sandbox.paypal.com/v1/payments/sale/94B86548SC282320N/refund",new RefundRequest()
        {
            amount = new Amount()
            {
                currency = "USD",
                total = "0.50"
            },
            invoice_number = "123456789" // Your business generated invoice
        });
        break;
    default:
        Console.WriteLine("No Option Available....");
        break;
}

Console.ReadLine();
    


