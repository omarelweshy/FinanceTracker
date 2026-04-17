CREATE TABLE transactions (
    id UUID PRIMARY KEY,
    account_id UUID NOT NULL REFERENCES accounts(id),
    category_id UUID NOT NULL REFERENCES categories(id),
    type VARCHAR(50) NOT NULL,
    amount NUMERIC(18,2) NOT NULL,
    description TEXT,
    transaction_date TIMESTAMP NOT NULL,
    created_at TIMESTAMP NOT NULL
);

CREATE INDEX ix_transactions_account_id ON transactions(account_id);
CREATE INDEX ix_transactions_category_id ON transactions(category_id);
CREATE INDEX ix_transactions_transaction_date ON transactions(transaction_date);
CREATE INDEX ix_transactions_account_date ON transactions(account_id, transaction_date);
