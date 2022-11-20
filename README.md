# Shopping System

1. All users are allowed to see pages products and supermarkets
1. Only user with role "admin" can create/edit/delete supermarket
1. Only user with role "admin" can create/edit/delete product
1. Only authenticated users can see the content of the Home page
1. Only users with role "admin" can see page Customers
1. User with role "admin" can see page *Orders* with orders from all users
1. User with role "buyer" can see page *Orders* only with his own orders and cannot modify, create or delete orders
1. Every buyer receives claim "buyerType" with possible values: "none", "regular", "golden", "wholesale".
1. Only buyers with "golden", "wholesale" claim values have access to Discount page (*My Discount* tab in the main menu)
