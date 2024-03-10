# .net-project
Welcome to the repository of a robust and secure .NET web service meticulously crafted to tackle diverse tasks with efficiency and reliability. Leveraging a blend of middleware and dependency injection, this application prioritizes modularity, scalability, and maintainability.

User Management:
- User Registration:
    - Implement user registration with validation checks and OTP verification for added security.
      
- User Activation and Deactivation:
    - Activate or deactivate user accounts as needed, ensuring control over user access.
      
- Reset Password:
    - Reset passwords securely by validating against password history to prevent unauthorized access.
      
- User Role Assignment:
    - Assign roles to users for role-based access control and permissions management.
      
- Forget Password:
    - Allow users to reset forgotten passwords securely through OTP validation.
      
- User Role Access Assignment:
    - Assign access rights to specific roles to control which functionalities users can access.
      
- Menu Access Control:
    - Implement functionality to get menus based on user role access and check menu access by role access, ensuring granular control over user permissions.

Image Handling:
- Upload:
    - Upload a single image and store it in the server path.
    - Upload multiple images and store them in the server path.
    - Upload multiple images and store them in the database.

- Retrieve (GET Image):
    - Retrieve images from both server path and database.

- Remove:
    - Remove images from both server path and database.

- Download Images:
    - Download images from both server path and database.

Key Features:

- Entity Framework Core:
    - Seamlessly interact with your database using this powerful ORM framework, enabling effortless CRUD operations.

- Auto Mapper:
    - Simplify object mapping between different types, streamlining development by eliminating repetitive manual mapping tasks.

- Logging with Serilog:
    - Capture and analyze application events effectively for debugging, auditing, and monitoring purposes, thanks to Serilog's flexible and structured logging capabilities.

- CORS Enablement:
    - Securely interact with resources from different origins, enhancing integration and communication with client-side applications.

- Rate Limiter:
    - Prevent resource abuse and ensure fair usage by limiting client requests within specified time frames.

- Authentication:
    - Safeguard access to resources with robust authentication mechanisms:

  - JWT Authentication:
      - Securely authenticate and access protected resources using JSON Web Tokens (JWT), facilitating secure communication.

  - Refresh Token:
      - Enhance security and user experience by extending access token validity without frequent re-authentication.

- Basic CRUD Actions:
    -Seamlessly manage application data with support for Create, Read, Update, and Delete operations, ensuring effortless resource interaction.


  
