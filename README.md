Agent API
Overview
Agent API is a robust API designed to streamline various functionalities for web and mobile platforms. This project leverages modern technologies to deliver secure, efficient, and scalable endpoints for managing checklists, clients, organizations, users, and permissions.

Features
CheckList Endpoints
GET /api/CheckList: Retrieve all checklists
POST /api/CheckList: Create a new checklist
PUT /api/CheckList/{id}: Update an existing checklist
DELETE /api/CheckList/{id}: Delete a checklist
POST /api/CheckList/{id}/Form: Add a form to a checklist
GET /api/CheckList/{id}/Form: Retrieve forms of a checklist
Client Endpoints
POST /api/Client: Create a new client
GET /api/Client: Retrieve all clients
GET /api/Client/{id}: Retrieve a specific client
PUT /api/Client/{id}: Update an existing client
DELETE /api/Client/{id}: Delete a client
GET /api/Client/DirectoriesForAgent: Retrieve directories for agents
Organization Endpoints
GET /api/Organization: Retrieve all organizations
POST /api/Organization: Create a new organization
GET /api/Organization/{id}: Retrieve a specific organization
PUT /api/Organization/{id}: Update an existing organization
DELETE /api/Organization/{id}: Delete an organization
POST /api/Organization/{id}/Users: Add users to an organization
GET /api/Organization/{id}/Users: Retrieve users of an organization
GET /api/Organization/{id}/Users/{userId}: Retrieve a specific user in an organization
DELETE /api/Organization/{id}/Users/{userId}: Remove a user from an organization
PUT /api/Organization/{id}/Users/{userId}: Update a user's information in an organization
GET /api/Organization/{id}/CheckLists: Retrieve checklists of an organization
POST /api/Organization/{id}/OrganizationAdmin: Assign an organization admin
PUT /api/Organization/{id}/OrganizationAdmin: Update the organization admin
User Endpoints
GET /api/User: Retrieve all users
POST /api/User: Create a new user
GET /api/User/{id}/Forms: Retrieve forms associated with a user
Permission Endpoints
GET /api/Permission: Retrieve all permissions
GET /api/Permission/{id}: Retrieve a specific permission
Technologies Used
ASP.NET Core: Framework for building the web API
MongoDB: NoSQL database for modeling and querying data
JWT Authentication: Secure API endpoints with JSON Web Tokens
Role-Based Authorization: Manage user permissions and roles
Repository Design Pattern: Decouple business logic and data access layers
Getting Started
Prerequisites
.NET Core SDK
MongoDB
Installation
Clone the repository:

sh
Copy code
git clone https://github.com/YourUsername/AgentAPI.git
cd AgentAPI
Configure the MongoDB connection string in appsettings.json:

json
Copy code
"ConnectionStrings": {
  "MongoDb": "your-mongodb-connection-string"
}
Run the application:

sh
Copy code
dotnet run
Usage
Access the API documentation at http://localhost:5295/swagger/index.html to explore and test the available endpoints.

Contributing
Contributions are welcome! Please fork the repository and create a pull request with your changes.

License
This project is licensed under the MIT License. See the LICENSE file for more details.

Contact
For any inquiries or feedback, feel free to reach out or create an issue in the repository.
