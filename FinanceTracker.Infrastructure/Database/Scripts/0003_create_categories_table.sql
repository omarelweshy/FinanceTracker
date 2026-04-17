CREATE TABLE categories (
    id UUID PRIMARY KEY,
    user_id UUID REFERENCES users(id),
    name VARCHAR(255) NOT NULL,
    type VARCHAR(50) NOT NULL,
    icon VARCHAR(100),
    is_default BOOLEAN NOT NULL DEFAULT FALSE,
    created_at TIMESTAMP NOT NULL,
    CONSTRAINT uq_categories_user_name UNIQUE (user_id, name)
);

CREATE INDEX ix_categories_user_id ON categories(user_id);
