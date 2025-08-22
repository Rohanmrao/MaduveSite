-- Create users table (idempotent)
CREATE TABLE IF NOT EXISTS users (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    full_name VARCHAR(100) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL DEFAULT '',
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

-- Create index on email (idempotent)
CREATE INDEX IF NOT EXISTS ix_users_email ON users(email);

-- Create enum type for profile status (if not exists)
DO $$ BEGIN
    CREATE TYPE profile_status AS ENUM ('Pending', 'Active', 'Inactive', 'Blocked', 'InTalks');
EXCEPTION
    WHEN duplicate_object THEN null;
END $$;
