-- Add password_hash column to users table
ALTER TABLE users 
ADD COLUMN password_hash VARCHAR(255) NOT NULL DEFAULT '';

-- Add password_hash column to user_requests table
ALTER TABLE user_requests 
ADD COLUMN password_hash VARCHAR(255) NOT NULL DEFAULT '';

-- Update existing records to have a default password hash (users should change this)
-- This is just for existing data compatibility
UPDATE users SET password_hash = 'e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855' WHERE password_hash = '';
UPDATE user_requests SET password_hash = 'e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855' WHERE password_hash = '';
