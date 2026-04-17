CREATE TABLE accounts (
    id UUID PRIMARY KEY,
    user_id UUID NOT NULL REFERENCES users(id),
    name VARCHAR(255) NOT NULL,
    type VARCHAR(50) NOT NULL,
    balance NUMERIC(18,2) NOT NULL DEFAULT 0,
    currency CHAR(3) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL,
    CONSTRAINT uq_accounts_user_name UNIQUE (user_id, name)
);

CREATE INDEX ix_accounts_user_id ON accounts(user_id);
