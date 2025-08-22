CREATE TABLE IF NOT EXISTS connect_requests (
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

CREATE INDEX IF NOT EXISTS idx_connect_requests_sender ON connect_requests(sender_id);
CREATE INDEX IF NOT EXISTS idx_connect_requests_receiver ON connect_requests(receiver_id);
CREATE INDEX IF NOT EXISTS idx_connect_requests_status ON connect_requests(status);
CREATE UNIQUE INDEX IF NOT EXISTS idx_connect_requests_sender_receiver ON connect_requests(sender_id, receiver_id);
