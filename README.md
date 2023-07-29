# Dapr Workflow with Service Invocation to Non-Dapr Endpoint 
An example of using Dapr Workflows with the new feature for invoking non-Dapr enabled endpoints

## Prerequisites

1. [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
1. [Dapr CLI](https://docs.dapr.io/getting-started/install-dapr-cli/)
   - Ensure that you're using **v1.11** of the Dapr runtime and the CLI, since there have been [breaking changes](https://github.com/dapr/dapr/pull/6218) to the Workflow API from v1.10 to v1.11.
1. A REST client, such as [cURL](https://curl.se/), or the VSCode [REST client extension](https://marketplace.visualstudio.com/items?itemName=humao.rest-client).
1. [Square Developer account](#Setting-up-your-connection-to-Square-Developer-Sandbox-APIs)

### Setting up your connection to Square Developer Sandbox APIs 

1. Navigate to the [Square Developer](https://developer.squareup.com/us/en) website and click "Get started"
1. If you don't already have an account, select "Sign up". Otherwise, enter existing credentials.
1. Add a new application and call it `workflow-payment-app` or select your own unique identifier.
1. For "What will you build", select "Accept payments" > "Next" or "Skip". 
1. For "Find your audience", select "Myself" > "Complete" or "Skip". 
1. On the Credentials page, find the `Sandbox Access token` > Click "show" > Copy the value.
1. Navigate to `Resources` > `httpEndpoint.yaml` and replace `[SQUARE_SANDBOX_TOKEN]` with the API token retrieved.

Once you have completed the above steps, you are ready to connect to the Square Payment API from your Dapr Workflow!
