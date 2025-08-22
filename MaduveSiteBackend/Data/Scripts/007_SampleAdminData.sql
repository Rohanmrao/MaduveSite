INSERT INTO admins (full_name, email, password_hash, phone, is_active) VALUES
('Admin User', 'admin@maduvesite.com', 'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=', '1234567890', true)
ON CONFLICT (email) DO NOTHING;
