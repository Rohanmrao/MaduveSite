# Database Scripts

This directory contains SQL scripts for setting up the MaduveSite database.

## Script Execution Order

The scripts are executed in alphabetical order by the DatabaseInitializer service:

1. **001_CreateUsersTable.sql** - Creates the users table with all columns including password_hash and profile images
2. **002_SampleData.sql** - Inserts sample user data (only if table is empty)
3. **003_CreateAdminsTable.sql** - Creates the admins table
4. **004_CreateUserRequestsTable.sql** - Creates the user_requests table with all columns
5. **005_CreateConnectRequestsTable.sql** - Creates the connect_requests table
6. **007_SampleAdminData.sql** - Inserts sample admin data

## Notes

- All scripts use `CREATE TABLE IF NOT EXISTS` for idempotent execution
- All timestamp columns use `TIMESTAMP WITH TIME ZONE` for proper timezone handling
- All tables include `password_hash` columns with default values
- Profile image columns are included in both users and user_requests tables
- Sample data includes proper password hashes for testing

## Database Schema

The scripts create a complete database schema with:
- User management (users table)
- Admin management (admins table)
- User approval workflow (user_requests table)
- Connect requests (connect_requests table)
- Profile photos and additional images support
- Proper indexing and foreign key constraints
