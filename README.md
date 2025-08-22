# MaduveSite Backend API

A comprehensive matrimonial site backend API built with ASP.NET Core, Entity Framework Core, and PostgreSQL. This API provides user management, admin functionality, connect requests, and a complete approval workflow.

## Features

- **User Management**: Registration, login, profile management
- **Admin System**: Admin creation, user approval/rejection, dashboard
- **Connect Requests**: Users can send and manage connection requests
- **Password Security**: SHA256 password hashing
- **Application Status Tracking**: Users can check their application status
- **Profile Photos**: Upload, retrieve, and delete profile photos
- **Comprehensive Error Handling**: Detailed error messages and validation

## Technology Stack

- **Framework**: ASP.NET Core 8.0
- **ORM**: Entity Framework Core
- **Database**: PostgreSQL
- **Authentication**: Custom password-based authentication
- **Documentation**: Swagger/OpenAPI
- **Architecture**: Repository Pattern, Service Layer, DTOs

## Prerequisites

- .NET 8.0 SDK
- PostgreSQL 12+
- Visual Studio 2022 or VS Code

## Getting Started

### 1. Clone the Repository
```bash
git clone <repository-url>
cd MaduveSite
```

### 2. Database Setup
1. Create a PostgreSQL database
2. Update connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=maduvesite;Username=your_username;Password=your_password"
  }
}
```

### 3. Run the Application
```bash
cd MaduveSiteBackend
dotnet restore
dotnet run
```

The API will be available at `http://localhost:5000`
Swagger documentation: `http://localhost:5000/swagger`

## API Documentation

### Base URL
```
http://localhost:5000/api
```

### Authentication
Most endpoints require authentication. Admin-only endpoints require admin credentials passed via:
- Query parameter: `?adminId=guid`
- Header: `X-Admin-Id: guid`
- Route parameter: `{adminId}`

---

## User Management

### 1. User Registration (Signup)
**Endpoint**: `POST /api/users/signup`

Creates a user signup request that requires admin approval.

**Request Body**:
```json
{
  "fullName": "John Doe",
  "email": "john@example.com",
  "password": "password123",
  "phone": "1234567890",
  "ecclesia": "St. Mary's Church",
  "language": "English",
  "education": "Bachelor's Degree",
  "bio": "I am a software developer looking for a life partner."
}
```

**Response**:
```json
{
  "message": "Signup request submitted successfully. Please wait for admin approval.",
  "requestId": "123e4567-e89b-12d3-a456-426614174000"
}
```

### 2. User Login
**Endpoint**: `POST /api/login/user`

**Request Body**:
```json
{
  "email": "john@example.com",
  "password": "password123"
}
```

**Response**:
```json
{
  "success": true,
  "message": "Login successful",
  "userId": "123e4567-e89b-12d3-a456-426614174000",
  "userType": "User",
  "userName": "John Doe",
  "status": "Active"
}
```

### 3. Check Application Status
**Endpoint**: `GET /api/login/status/{email}`

**Response Examples**:

*Pending Application*:
```json
{
  "email": "john@example.com",
  "status": "Pending",
  "message": "Your application is pending review",
  "createdAt": "2024-01-01T10:00:00Z",
  "processedAt": null,
  "adminName": null
}
```

*Approved Application*:
```json
{
  "email": "john@example.com",
  "status": "Approved",
  "message": "Your account has been approved and is active",
  "createdAt": "2024-01-01T10:00:00Z",
  "processedAt": "2024-01-02T15:30:00Z",
  "adminName": "Admin User"
}
```

*Rejected Application*:
```json
{
  "email": "john@example.com",
  "status": "Rejected",
  "message": "Your application has been rejected",
  "createdAt": "2024-01-01T10:00:00Z",
  "processedAt": "2024-01-02T15:30:00Z",
  "adminName": "Admin User"
}
```

### 4. Get User by Email
**Endpoint**: `GET /api/users/email/{email}`

**Response**:
```json
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "fullName": "John Doe",
  "email": "john@example.com",
  "phone": "1234567890",
  "ecclesia": "St. Mary's Church",
  "language": "English",
  "education": "Bachelor's Degree",
  "bio": "I am a software developer looking for a life partner.",
  "hasProfilePhoto": true,
  "status": "Active",
  "createdAt": "2024-01-01T10:00:00Z",
  "updatedAt": "2024-01-01T10:00:00Z"
}
```

### 5. Update User Profile
**Endpoint**: `PUT /api/users/{id}`

**Request Body**:
```json
{
  "fullName": "John Doe Updated",
  "email": "john@example.com",
  "phone": "1234567890",
  "ecclesia": "St. Mary's Church",
  "language": "English",
  "education": "Master's Degree",
  "bio": "Updated bio information."
}
```

### 6. Change User Status
**Endpoint**: `PATCH /api/users/{id}/status`

**Request Body**:
```json
{
  "status": "Active"
}
```

**Available Statuses**: `Pending`, `Active`, `Inactive`, `Blocked`, `InTalks`

---

## Admin Management

### 1. Admin Login
**Endpoint**: `POST /api/login/admin`

**Request Body**:
```json
{
  "email": "admin@example.com",
  "password": "adminpassword"
}
```

**Response**:
```json
{
  "success": true,
  "message": "Login successful",
  "userId": "admin-guid-here",
  "userType": "Admin",
  "userName": "Admin User",
  "status": "Active"
}
```

### 2. Create Admin
**Endpoint**: `POST /api/admin/signup`

**Request Body**:
```json
{
  "fullName": "Admin User",
  "email": "admin@example.com",
  "password": "adminpassword",
  "phone": "1234567890"
}
```

### 3. Get Admin Dashboard
**Endpoint**: `GET /api/admin/dashboard`

**Response**:
```json
[
  "Total Users: 150",
  "Active Users: 120",
  "Pending Approvals: 5",
  "Total Admins: 3",
  "Active Admins: 2"
]
```

### 4. Get Pending User Requests
**Endpoint**: `GET /api/admin/requests/pending`

**Response**:
```json
[
  {
    "id": "request-guid-here",
    "fullName": "Jane Smith",
    "email": "jane@example.com",
    "phone": "9876543210",
    "ecclesia": "St. Joseph's Church",
    "language": "English",
    "education": "Bachelor's Degree",
    "bio": "Looking for a life partner.",
    "status": "Pending",
    "createdAt": "2024-01-01T10:00:00Z"
  }
]
```

### 5. Approve User Request
**Endpoint**: `POST /api/admin/requests/{requestId}/approve`

**Request Body**:
```json
{
  "adminId": "admin-guid-here"
}
```

**Response**:
```json
{
  "id": "request-guid-here",
  "status": "Approved",
  "message": "User approved successfully"
}
```

### 6. Reject User Request
**Endpoint**: `POST /api/admin/requests/{requestId}/reject`

**Request Body**:
```json
{
  "adminId": "admin-guid-here"
}
```

**Response**:
```json
{
  "id": "request-guid-here",
  "status": "Rejected",
  "message": "User rejected successfully"
}
```

### 7. Get Admin by Email
**Endpoint**: `GET /api/admin/email/{email}`

**Response**:
```json
{
  "id": "admin-guid-here",
  "fullName": "Admin User",
  "email": "admin@example.com",
  "phone": "1234567890",
  "isActive": true,
  "createdAt": "2024-01-01T10:00:00Z"
}
```

### 8. Check User Auth Level
**Endpoint**: `GET /api/admin/auth-level/{userId}`

**Response**:
```json
{
  "userId": "user-guid-here",
  "authLevel": "User"
}
```

**Possible Values**: `Admin`, `User`, `Unknown`

### 9. Delete User (Admin Only)
**Endpoint**: `DELETE /api/users/{id}?adminId={adminId}`

**Response**: `204 No Content`

---

## Connect Requests

### 1. Send Connect Request
**Endpoint**: `POST /api/connect-requests/sender/{senderId}/send`

**Request Body**:
```json
{
  "receiverId": "receiver-guid-here",
  "message": "Hi, I would like to connect with you."
}
```

**Response**:
```json
{
  "id": "connect-request-guid",
  "status": "Sent",
  "message": "Connect request sent successfully"
}
```

### 2. Accept Connect Request
**Endpoint**: `POST /api/connect-requests/receiver/{receiverId}/accept/{senderId}`

**Response**:
```json
{
  "id": "connect-request-guid",
  "status": "Accepted",
  "message": "Connect request accepted successfully"
}
```

### 3. Reject Connect Request
**Endpoint**: `POST /api/connect-requests/receiver/{receiverId}/reject/{senderId}`

**Response**:
```json
{
  "id": "connect-request-guid",
  "status": "Rejected",
  "message": "Connect request rejected successfully"
}
```

### 4. Get Pending Requests for User
**Endpoint**: `GET /api/connect-requests/receiver/{receiverId}/pending`

**Response**:
```json
[
  {
    "id": "connect-request-guid",
    "senderId": "sender-guid",
    "receiverId": "receiver-guid",
    "senderName": "John Doe",
    "receiverName": "Jane Smith",
    "message": "Hi, I would like to connect with you.",
    "status": "Pending",
    "createdAt": "2024-01-01T10:00:00Z"
  }
]
```

### 5. Get Sent Requests by User
**Endpoint**: `GET /api/connect-requests/sender/{senderId}/sent`

**Response**:
```json
[
  {
    "id": "connect-request-guid",
    "senderId": "sender-guid",
    "receiverId": "receiver-guid",
    "senderName": "John Doe",
    "receiverName": "Jane Smith",
    "message": "Hi, I would like to connect with you.",
    "status": "Pending",
    "createdAt": "2024-01-01T10:00:00Z"
  }
]
```

### 6. Check Connection Status
**Endpoint**: `GET /api/connect-requests/check/{senderId}/{receiverId}`

**Response**:
```json
{
  "hasActiveConnection": true
}
```

---

## Profile Photos

### 1. Upload Profile Photo
**Endpoint**: `POST /api/users/{id}/photo`

**Request**: `multipart/form-data` with file

**Response**:
```json
{
  "message": "Photo uploaded successfully",
  "size": 1024000
}
```

### 2. Get Profile Photo
**Endpoint**: `GET /api/users/{id}/photo`

**Response**: Image file

### 3. Delete Profile Photo
**Endpoint**: `DELETE /api/users/{id}/photo`

**Response**: `204 No Content`

---

## Additional Profile Images

### 1. Upload Profile Image
**Endpoint**: `POST /api/profile-image/{userId}/upload/{imageNumber}`

**Request**: `multipart/form-data` with file
**Note**: `imageNumber` must be 1, 2, or 3

**Response**:
```json
{
  "message": "Profile image 1 uploaded successfully",
  "size": 1024000
}
```

### 2. Get Profile Image
**Endpoint**: `GET /api/profile-image/{userId}/image/{imageNumber}`

**Response**: Image file

### 3. Delete Profile Image
**Endpoint**: `DELETE /api/profile-image/{userId}/image/{imageNumber}`

**Response**: `204 No Content`

### 4. Get Available Image Slots
**Endpoint**: `GET /api/profile-image/{userId}/available-slots`

**Response**:
```json
{
  "availableSlots": [1, 2, 3]
}
```

### 5. Get All Profile Images
**Endpoint**: `GET /api/profile-image/{userId}/all`

**Description**: Get information about all profile images for a user.

**Response**:
```json
{
  "userId": "user-guid-here",
  "totalImages": 2,
  "images": [
    {
      "imageNumber": 1,
      "contentType": "image/jpeg",
      "size": 1024000
    },
    {
      "imageNumber": 3,
      "contentType": "image/png",
      "size": 2048000
    }
  ]
}
```

**Error Response**:
```json
{
  "error": "No profile images found for this user"
}
```

or

```json
{
  "error": "Failed to retrieve profile images",
  "details": "Error details"
}
```

---

## General Endpoints

### 1. Get All Users
**Endpoint**: `GET /api/users`

**Response**:
```json
[
  {
    "id": "user-guid-here",
    "fullName": "John Doe",
    "email": "john@example.com",
    "phone": "1234567890",
    "ecclesia": "St. Mary's Church",
    "language": "English",
    "education": "Bachelor's Degree",
    "bio": "I am a software developer looking for a life partner.",
    "hasProfilePhoto": true,
    "hasProfileImage1": true,
    "hasProfileImage2": false,
    "hasProfileImage3": true,
    "status": "Active",
    "createdAt": "2024-01-01T10:00:00Z",
    "updatedAt": "2024-01-01T10:00:00Z"
  }
]
```

### 2. Get User by ID
**Endpoint**: `GET /api/users/{id}`

**Response**: Same as above for single user

### 3. Get All Admins
**Endpoint**: `GET /api/admin`

**Response**:
```json
[
  {
    "id": "admin-guid-here",
    "fullName": "Admin User",
    "email": "admin@example.com",
    "phone": "1234567890",
    "isActive": true,
    "createdAt": "2024-01-01T10:00:00Z"
  }
]
```

---

## Error Handling

The API returns consistent error responses:

### 400 Bad Request
```json
{
  "error": "Request body is required"
}
```

### 401 Unauthorized
```json
{
  "success": false,
  "message": "Invalid email or password"
}
```

### 403 Forbidden
```json
{
  "error": "Access denied"
}
```

### 404 Not Found
```json
{
  "error": "User not found"
}
```

### 500 Internal Server Error
```json
{
  "error": "An unexpected error occurred",
  "details": "Technical error details"
}
```

---

## Configuration

The application uses the following configuration settings in `appsettings.json`:

### Database Connection
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5433;Database=DB_prod;Username=postgres;Password=root"
  }
}
```

### Application Settings
```json
{
  "AppSettings": {
    "ApplicationUrl": "http://localhost:5000",
    "MaxImageSizeBytes": 5242880,
    "MaxProfileImagesPerUser": 3,
    "AllowedImageExtensions": [".jpg", ".jpeg", ".png", ".gif", ".webp"],
    "AllowedImageMimeTypes": ["image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp"],
    "MinimumPasswordLength": 6,
    "DefaultPasswordHash": "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855"
  }
}
```

### Kestrel Server Configuration
```json
{
  "KestrelPort": {
    "Http": 5000,
    "Https": 5001
  },
  "UseHttps": "false",
  "SSL": {
    "SSLCertPath": "",
    "SSLCertPwd": ""
  }
}
```

**Configuration Options:**
- **ApplicationUrl**: The base URL for the application
- **MaxImageSizeBytes**: Maximum file size for image uploads (default: 5MB)
- **MaxProfileImagesPerUser**: Maximum number of additional profile images per user (default: 3)
- **AllowedImageExtensions**: File extensions allowed for image uploads
- **AllowedImageMimeTypes**: MIME types allowed for image uploads
- **MinimumPasswordLength**: Minimum required password length
- **DefaultPasswordHash**: Default hash for existing users without passwords
- **KestrelPort:Http**: HTTP port for the application (default: 5000)
- **KestrelPort:Https**: HTTPS port for the application (default: 5001)
- **UseHttps**: Enable HTTPS (true/false)
- **SSL:SSLCertPath**: Path to SSL certificate file
- **SSL:SSLCertPwd**: Password for SSL certificate

## Database Schema

### Users Table
```sql
CREATE TABLE users (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    full_name VARCHAR(100) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    phone VARCHAR(20) NOT NULL DEFAULT '',
    ecclesia VARCHAR(100) NOT NULL DEFAULT '',
    language VARCHAR(50) NOT NULL DEFAULT '',
    education VARCHAR(100) NOT NULL DEFAULT '',
    bio VARCHAR(1000) NOT NULL DEFAULT '',
    profile_photo_data BYTEA,
    profile_photo_content_type VARCHAR(100),
    profile_image_1_data BYTEA,
    profile_image_1_content_type VARCHAR(100),
    profile_image_2_data BYTEA,
    profile_image_2_content_type VARCHAR(100),
    profile_image_3_data BYTEA,
    profile_image_3_content_type VARCHAR(100),
    status INTEGER NOT NULL DEFAULT 0,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);
```

### User Requests Table
```sql
CREATE TABLE user_requests (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    full_name VARCHAR(100) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    phone VARCHAR(20),
    ecclesia VARCHAR(100),
    language VARCHAR(50),
    education VARCHAR(100),
    bio VARCHAR(1000),
    profile_photo_data BYTEA,
    profile_photo_content_type VARCHAR(100),
    profile_image_1_data BYTEA,
    profile_image_1_content_type VARCHAR(100),
    profile_image_2_data BYTEA,
    profile_image_2_content_type VARCHAR(100),
    profile_image_3_data BYTEA,
    profile_image_3_content_type VARCHAR(100),
    status INTEGER DEFAULT 0,
    admin_id UUID,
    processed_at TIMESTAMP WITH TIME ZONE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);
```

### Admins Table
```sql
CREATE TABLE admins (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    full_name VARCHAR(100) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    phone VARCHAR(20),
    is_active BOOLEAN DEFAULT true,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);
```

### Connect Requests Table
```sql
CREATE TABLE connect_requests (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    sender_id UUID NOT NULL,
    receiver_id UUID NOT NULL,
    message VARCHAR(500),
    status INTEGER DEFAULT 0,
    responded_at TIMESTAMP WITH TIME ZONE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_connect_requests_sender FOREIGN KEY (sender_id) REFERENCES users(id) ON DELETE RESTRICT,
    CONSTRAINT fk_connect_requests_receiver FOREIGN KEY (receiver_id) REFERENCES users(id) ON DELETE RESTRICT
);
```

---

## Security Features

### Password Hashing
- **Algorithm**: SHA256
- **Storage**: Hashed passwords only (no plain text)
- **Verification**: Secure password comparison

### Admin Authentication
- **Admin-only endpoints** require valid admin ID
- **Multiple authentication methods**: Query params, headers, route params
- **Active admin validation**: Only active admins can perform actions

### Input Validation
- **Email validation**: Proper email format required
- **Password requirements**: Minimum 6 characters
- **String length limits**: Enforced on all text fields
- **Required field validation**: Essential fields must be provided

---

## Deployment

### Environment Variables
```bash
# Database
ConnectionStrings__DefaultConnection=Host=your_host;Database=your_db;Username=your_user;Password=your_pass

# Application
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://localhost:5000
```

### Docker Deployment
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MaduveSiteBackend/MaduveSiteBackend.csproj", "MaduveSiteBackend/"]
RUN dotnet restore "MaduveSiteBackend/MaduveSiteBackend.csproj"
COPY . .
WORKDIR "/src/MaduveSiteBackend"
RUN dotnet build "MaduveSiteBackend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MaduveSiteBackend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MaduveSiteBackend.dll"]
```

---

## API Testing

### Using Swagger UI
1. Navigate to `http://localhost:5000/swagger`
2. Explore available endpoints
3. Test API calls directly from the browser

### Using Postman
1. Import the API collection
2. Set base URL: `http://localhost:5000/api`
3. Use the provided examples for testing

### Using curl
```bash
# User signup
curl -X POST "http://localhost:5000/api/users/signup" \
  -H "Content-Type: application/json" \
  -d '{
    "fullName": "John Doe",
    "email": "john@example.com",
    "password": "password123",
    "phone": "1234567890",
    "ecclesia": "St. Mary's Church",
    "language": "English",
    "education": "Bachelor's Degree",
    "bio": "Looking for a life partner."
  }'

# User login
curl -X POST "http://localhost:5000/api/login/user" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "john@example.com",
    "password": "password123"
  }'
```

---

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

---

## License

This project is licensed under the MIT License - see the LICENSE file for details.

---

## Support

For support and questions:
- Create an issue in the repository
- Contact the development team
- Check the Swagger documentation for API details

---

## Version History

- **v1.0.0**: Initial release with basic user and admin functionality
- **v1.1.0**: Added connect requests and profile photos
- **v1.2.0**: Enhanced security with password hashing and admin permissions
- **v1.3.0**: Added application status tracking and comprehensive login system