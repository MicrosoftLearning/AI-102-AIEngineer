---
lab:
    title: 'Enable Resource Providers'
    module: 'Setup'
---

# Enable Resource Providers

There are some resource providers that must be registered in your Azure subscription. Follow these steps to ensure that they're registered.

1. Sign into the Azure portal at `https://portal.azure.com` using the Microsoft credentials associated with your subscription.
2. On the **Home** page, select **Subscriptions** (or expand the **&#8801;** menu, select **All Services**, and in the **General** category, select **Subscriptions**).
3. Select your Azure subscription (if you have multiple subscriptions, select the one you created by redeeming your Azure Pass).
4. In the blade for your subscription, in the pane on the left, in the **Settings** section, select **Resource providers**.
5. In the list of resource providers, ensure the following providers are registered (if not, select them and click **register**):
    - Microsoft.BotService
    - Microsoft.Web
    - Microsoft.ManagedIdentity
    - Microsoft.Search
    - Microsoft.Storage
    - Microsoft.CognitiveServices
    - Microsoft.AlertsManagement
    - microsoft.insights
    - Microsoft.KeyVault
    - Microsoft.ContainerInstance
