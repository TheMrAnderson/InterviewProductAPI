# Interview Take Home Project

For a second round interview for a C# backend software engineer position I was provided with a readme with the instructions below and a repo.  This is the project I created for that interview within roughly 18 hours, and this repo serves to illustrate my programming skill.

## My Decisions

- Went with a SQLite database for portability, because I wasn't sure if they were going to run it prior to the interview, and I wanted it self-contained in case they were.  The SQL script to create and populate the database is also included for the same reasons.
- Added my ApiError class I have been using for years, and I added logging just like an enterprise-grade application would have.
- Mocked up the authentication using a Bearer token and created my own authentation classes and injected them via DI so the authentication works, but it is self-contained.
- Used Swagger to document the API, like I have been doing for many years.
- Used Dapper for the first time to keep it lightweight and have more control over the data.  Even though I knew this was just for an interview, I built it with scale, performance and longevity in mind.  We all know programs are asked to go longer than intended due to costs, handle more than expected and continue doing it as fast as it was on day 1.  By controlling my queries from the beginning, I have control over the sliding performance scale and can add indexing, update the query and continue keeping the performance at the best it can be much easier.

## Instructions

Build a RESTful API that allows a user to create, read, update, and delete (CRUD) data related to a specific domain. Your API should be designed to store and retrieve data from a database, which should also be set up and configured as part of the assignment.
Requirements:

- You may use any backend framework or tool of your choice to build the API.
- The API should be designed to handle the CRUD operations for the product domain.
  - A product consists of the following attributes: product name, product category, and product price.
  - For generation of fake data to seed your database, we recommend a service like [Mockaroo](https://www.mockaroo.com), although you may use any means you see fit.
- The API should support searching and sorting on the product domain. Specifically, the API should support searching for a product by name, category, or price. Additionally, products should be sortable by name, category, and price.
- The API should use a database to store and retrieve data. The type of database and the schema of the data are up to you.
  - We suggest using something like SQLite as the setup is simplified and easy to showcase, but if you feel like setting up a fully functional database, that is also fine, just be aware that you will be required to showcase and run your application in the code review stage after you submit the project.
  - Make sure this database is properly seeded at application startup or before hand, so the API has data to return for demonstration purposes.
- The API should have appropriate error handling and validation for all requests.
- The API should have proper authentication and authorization mechanisms in place to ensure that only authorized users can perform certain actions, such as creating or deleting data.
  - You don't need to go crazy here, something simple will suffice. For example, you may have the Create call require an "authenticated user". We are not testing your ability to work with the creation and management of authorization systems, just authorization of the API itself.
- The API should be well-documented, including information about the endpoints, the expected input and output formats, and any error messages that may be returned.
  - We suggest using something like Swagger for this, or other related tools.
Upon completion, please open a pull request into the main branch of this repository containing all of your project code. We will schedule a review time with you promptly following you opening the pull request.