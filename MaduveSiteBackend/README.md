# Database Setup

This folder contains SQL scripts for setting up the database schema.

## Setup Instructions

1. Create a PostgreSQL database
2. Run the scripts in order:

```sql
-- Run this script to create the users table
\i 001_CreateUsersTable.sql
```

## Scripts

- `001_CreateUsersTable.sql` - Creates the users table with all necessary columns and indexes

## Notes

- The scripts use `IF NOT EXISTS` to prevent errors if run multiple times
- UUIDs are used for primary keys with automatic generation
- Timestamps are automatically set to current time
- Email addresses have unique constraints
