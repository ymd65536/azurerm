using System;
using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Compute;
using Azure.ResourceManager.Network;
using Azure.ResourceManager.Resources;

var credential = new DefaultAzureCredential(
    new DefaultAzureCredentialOptions()
    {
        ExcludeVisualStudioCredential = false,
    }
);

// リソースグループの作成
string resourceGroupName = "myResourceGroup";
var armClient = new ArmClient(credential);

SubscriptionResource subscription = armClient.GetDefaultSubscription();
bool exist = await subscription.GetResourceGroups().ExistsAsync(resourceGroupName);

if (!exist)
{
    Console.WriteLine("ResourceGroup does not exist.");
    await subscription.GetResourceGroups().CreateOrUpdateAsync(WaitUntil.Completed, resourceGroupName, new ResourceGroupData(AzureLocation.EastUS));
    Console.WriteLine("ResourceGroup created.");
}else{
    Console.WriteLine("ResourceGroup already exists.");
    ResourceGroupResource resourceGroup = subscription.GetResourceGroup(resourceGroupName);
    await resourceGroup.DeleteAsync(WaitUntil.Completed);
    Console.WriteLine("ResourceGroup deleted.");
    await subscription.GetResourceGroups().CreateOrUpdateAsync(WaitUntil.Completed, resourceGroupName, new ResourceGroupData(AzureLocation.EastUS));
    Console.WriteLine("ResourceGroup created.");
}

// ネットワークの作成

string vnetName = "myVnet";

VirtualNetworkData vnetData = new VirtualNetworkData()
{
    Location = AzureLocation.EastUS,
    AddressPrefixes = {"10.0.0.0/16"}
};

ResourceGroupResource Rg = await subscription.GetResourceGroups().GetAsync(resourceGroupName);
await Rg.GetVirtualNetworks().CreateOrUpdateAsync(WaitUntil.Completed, vnetName, vnetData);

string subnetName = $"{vnetName}mySubnet";
SubnetData subnetData = new SubnetData()
{
    Name = subnetName,
    AddressPrefix = "10.0.1.0/24",
};

await Rg.GetVirtualNetworks().Get(vnetName).Value.GetSubnets().CreateOrUpdateAsync(WaitUntil.Completed, subnetName, subnetData);
