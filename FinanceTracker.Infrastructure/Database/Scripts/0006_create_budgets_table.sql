CREATE TABLE budgets (
    id UUID PRIMARY KEY,
    user_id UUID NOT NULL REFERENCES users(id),
    category_id UUID NOT NULL REFERENCES categories(id),
    amount NUMERIC(18,2) NOT NULL,
    month SMALLINT NOT NULL,
    year SMALLINT NOT NULL,
    created_at TIMESTAMP NOT NULL,
    CONSTRAINT uq_budgets_user_category_month_year UNIQUE (user_id, category_id, month, year)
);

CREATE INDEX ix_budgets_user_id ON budgets(user_id);
