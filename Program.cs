using Azure;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;

var credential = new DefaultAzureCredential(
  new DefaultAzureCredentialOptions()
  {
    ExcludeVisualStudioCredential = false,
  }
);
var armClient = new ArmClient(credential);
var subscription = await armClient.GetDefaultSubscriptionAsync();

// リソースグループの作成
string resourceGroupName = "MyResourceGroup";
string location = "eastus";

try
{
	ResourceGroupData resourceGroupData = new ResourceGroupData(location);
	ArmOperation<ResourceGroupResource> operation = await subscription.GetResourceGroups().CreateOrUpdateAsync(WaitUntil.Completed, resourceGroupName, resourceGroupData);
	ResourceGroupResource resourceGroup = operation.Value;
	Console.WriteLine($"Resource Group '{resourceGroup.Data.Name}' created in location '{resourceGroup.Data.Location}'.");  
}
catch (System.Exception)
{
  
  throw;
}
