CREATE TABLE transfers (
    id UUID PRIMARY KEY,
    from_transaction_id UUID NOT NULL REFERENCES transactions(id),
    to_transaction_id UUID NOT NULL REFERENCES transactions(id),
    amount NUMERIC(18,2) NOT NULL,
    note TEXT,
    created_at TIMESTAMP NOT NULL
);

CREATE INDEX ix_transfers_from_transaction_id ON transfers(from_transaction_id);
CREATE INDEX ix_transfers_to_transaction_id ON transfers(to_transaction_id);
