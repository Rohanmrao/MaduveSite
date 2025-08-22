-- Fix timestamp columns to use TIMESTAMP WITH TIME ZONE
-- This script updates existing tables to match the expected column types

-- Fix connect_requests table
ALTER TABLE connect_requests 
ALTER COLUMN responded_at TYPE TIMESTAMP WITH TIME ZONE USING responded_at AT TIME ZONE 'UTC',
ALTER COLUMN created_at TYPE TIMESTAMP WITH TIME ZONE USING created_at AT TIME ZONE 'UTC',
ALTER COLUMN updated_at TYPE TIMESTAMP WITH TIME ZONE USING updated_at AT TIME ZONE 'UTC';

-- Fix user_requests table
ALTER TABLE user_requests 
ALTER COLUMN processed_at TYPE TIMESTAMP WITH TIME ZONE USING processed_at AT TIME ZONE 'UTC',
ALTER COLUMN created_at TYPE TIMESTAMP WITH TIME ZONE USING created_at AT TIME ZONE 'UTC',
ALTER COLUMN updated_at TYPE TIMESTAMP WITH TIME ZONE USING updated_at AT TIME ZONE 'UTC';

-- Fix admins table
ALTER TABLE admins 
ALTER COLUMN created_at TYPE TIMESTAMP WITH TIME ZONE USING created_at AT TIME ZONE 'UTC',
ALTER COLUMN updated_at TYPE TIMESTAMP WITH TIME ZONE USING updated_at AT TIME ZONE 'UTC';
