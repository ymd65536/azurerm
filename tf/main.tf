resource "azurerm_resource_group" "res" {
  location = "eastus"
  name     = "myResourceGroup"
}
resource "azurerm_virtual_network" "res-1" {
  address_space       = ["10.0.0.0/16"]
  location            = "eastus"
  name                = "myVnet"
  resource_group_name = "myResourceGroup"
  depends_on = [
    azurerm_resource_group.res,
  ]
}
resource "azurerm_subnet" "res-2" {
  address_prefixes     = ["10.0.1.0/24"]
  name                 = "myVnetmySubnet"
  resource_group_name  = "myResourceGroup"
  virtual_network_name = "myVnet"
  depends_on = [
    azurerm_virtual_network.res-1,
  ]
}
