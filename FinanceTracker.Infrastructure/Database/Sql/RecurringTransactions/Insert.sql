INSERT INTO recurring_transactions (id, user_id, account_id, category_id, amount, description, frequency, next_run_date, is_active, created_at)
VALUES (@Id, @UserId, @AccountId, @CategoryId, @Amount, @Description, @Frequency, @NextRunDate, @IsActive, @CreatedAt)
