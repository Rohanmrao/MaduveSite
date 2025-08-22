-- Insert sample data only if table is empty
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM users LIMIT 1) THEN
        INSERT INTO users (full_name, email, phone, ecclesia, language, education, bio, status) VALUES
        ('John Doe', 'john.doe@example.com', '+1234567890', 'Central Ecclesia', 'English', 'Bachelor of Science', 'Looking for a life partner who shares my faith and values.', 1),
        ('Jane Smith', 'jane.smith@example.com', '+1234567891', 'North Ecclesia', 'English', 'Master of Arts', 'Passionate about serving the community and building meaningful relationships.', 1),
        ('Michael Johnson', 'michael.johnson@example.com', '+1234567892', 'South Ecclesia', 'English', 'Bachelor of Engineering', 'Seeking someone who values family and faith above all else.', 0);
    END IF;
END $$;
