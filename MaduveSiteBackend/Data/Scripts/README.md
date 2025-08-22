# Database Scripts

This folder contains SQL scripts that are automatically executed on application startup.

## Automatic Execution

The application will automatically run all `.sql` files in this directory on startup, in alphabetical order.

## Scripts

- `001_CreateUsersTable.sql` - Creates the users table with all necessary columns and indexes
- `002_SampleData.sql` - Inserts sample data (only if table is empty)

## Script Requirements

- Scripts must be idempotent (safe to run multiple times)
- Use `IF NOT EXISTS` clauses where appropriate
- Scripts are executed in alphabetical order by filename
- Each script runs in its own transaction

## Manual Execution

If you need to run scripts manually:

```sql
-- Connect to your PostgreSQL database and run:
\i 001_CreateUsersTable.sql
\i 002_SampleData.sql
```

## Notes

- Scripts use `IF NOT EXISTS` to prevent errors if run multiple times
- UUIDs are used for primary keys with automatic generation
- Timestamps are automatically set to current time
- Email addresses have unique constraints
- Sample data is only inserted if the table is empty
