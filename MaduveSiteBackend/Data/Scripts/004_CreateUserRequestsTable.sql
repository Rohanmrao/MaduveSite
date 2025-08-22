CREATE TABLE IF NOT EXISTS user_requests (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    full_name VARCHAR(100) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL DEFAULT '',
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

CREATE INDEX IF NOT EXISTS idx_user_requests_email ON user_requests(email);
CREATE INDEX IF NOT EXISTS idx_user_requests_status ON user_requests(status);
