CREATE TABLE recurring_transactions (
    id UUID PRIMARY KEY,
    user_id UUID NOT NULL REFERENCES users(id),
    account_id UUID NOT NULL REFERENCES accounts(id),
    category_id UUID NOT NULL REFERENCES categories(id),
    amount NUMERIC(18,2) NOT NULL,
    description TEXT,
    frequency VARCHAR(50) NOT NULL,
    next_run_date TIMESTAMP NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL
);

CREATE INDEX ix_recurring_next_run_date_active ON recurring_transactions(next_run_date) WHERE is_active = TRUE;
