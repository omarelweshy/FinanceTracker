UPDATE transactions
SET amount = @Amount, description = @Description, transaction_date = @TransactionDate
WHERE id = @Id
