# OverView Of ShopXpress

ShopXpress is an Ecommerce platform that enables merchants to create and showcase a wide range of products for sale. Customers can browse through the available products and make purchases using Paystack as the online payment gateway.

The integration of Paystack ensures that customers can complete their purchases smoothly and securely on ShopXpress.

As this is an API Project various endpoints have been curated just for **YOU** to make the above come to life 

## Breakdown Of The Project

This project consists of four(4) layers:

1. The Presentation Layer has the controllers through which these endpoints would be accessed
- Transaction Controller initiates a payment transaction, returns a payment authorization URL verifies the status and details of a completed payment transaction
- Products Controller where merchants can perform CRUD operations on their products. This includes creating, deleting and viewing all products
- Authentication Controller handles user authentication and authorisation

2. The Business Logic Layer also known as the services layer **(the brain box)** handles all the logic behind the controller respectively. So each controller has a service file.
How cool is that???😎😎😎

3. The Data Access Layer: **A book without being placed on a shelf would be like data without being stored in a data storage system.** So this handles the database settings and interactions

4. The Models Layer: This consists of the entities which are mapped to the database as the collection name

## Technology Stack

- Language
  
  ![C#](https://camo.githubusercontent.com/bbae65b6de4a3ba26fbeaf00e347900385400dcd092e8b4e0f795853d24a24e3/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f632532332d2532333233393132302e7376673f7374796c653d666f722d7468652d6261646765266c6f676f3d632d7368617270266c6f676f436f6c6f723d7768697465)   ![.net](https://camo.githubusercontent.com/f36a579a7440dd2cd03da4903249f86d0d44cb7020fd902512bccd139784b363/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f2e4e45542d3543324439313f7374796c653d666f722d7468652d6261646765266c6f676f3d2e6e6574266c6f676f436f6c6f723d7768697465)
  
- Database

[![MongoDB](https://img.shields.io/badge/MongoDB-%234ea94b.svg?style=for-the-badge&logo=mongodb&logoColor=white)](https://www.mongodb.com/)


