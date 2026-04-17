CREATE TABLE users (
    id UUID PRIMARY KEY,
    email VARCHAR(255) NOT NULL UNIQUE,
    full_name VARCHAR(255) NOT NULL,
    password_hash TEXT NOT NULL,
    default_currency CHAR(3) NOT NULL,
    created_at TIMESTAMP NOT NULL
);
