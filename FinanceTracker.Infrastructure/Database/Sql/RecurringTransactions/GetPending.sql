SELECT * FROM recurring_transactions WHERE is_active = TRUE AND next_run_date <= @AsOf
