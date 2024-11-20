provider "azurerm" {
  features {
  }
  subscription_id            = "subscription_id"
  environment                = "public"
  use_msi                    = false
  use_cli                    = true
  use_oidc                   = false
  skip_provider_registration = true
}
