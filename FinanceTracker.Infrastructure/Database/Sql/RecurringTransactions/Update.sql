UPDATE recurring_transactions
SET amount = @Amount, description = @Description, frequency = @Frequency,
    next_run_date = @NextRunDate, is_active = @IsActive
WHERE id = @Id
