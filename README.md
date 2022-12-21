# PayPal-Integration-DotNet
This Repository has code about how you can integrate PayPal into your DOTNET Application and the main focus is on PayPal Subscription. I'm using the .NET 6 version but I'm not using any PayPal SDK. So, you can use the same code in any DOTNET Version.

There are three Projects:
1. TechnicalVoiceApp:This is the console application just for the demo to send requests to PayPal using REST API. You can consider as the UI of your web application.

2. PayPal:This Project contains everything about how to send a REST request to PayPal. Please change the "Client Id" and "Client Secret" in the config helper.

3. Webhook:This is the API project to receive the webhook events from PayPal.




This repo contains the following:

1- How to send REST API Requests to PayPal?

2- How to parse REST API Response from PayPal?

3- How to create the plan?

4- How to GET plan details?

5- How to create a subscription?

6- How to configure webhook and test in the local environment using ngrok?

7- How to cancel, activate and suspend subscriptions.

Youtube Tutorials:
https://www.youtube.com/watch?v=-e2CFvBhh8s&list=PL908CcBgcZDe5mRwJe-YbAj0lKO2zkJt-
