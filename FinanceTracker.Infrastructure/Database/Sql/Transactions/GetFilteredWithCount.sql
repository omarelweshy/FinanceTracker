SELECT COUNT(*) FROM transactions t
INNER JOIN accounts a ON a.id = t.account_id
WHERE a.user_id = @UserId
  AND (@AccountId IS NULL OR t.account_id = @AccountId)
  AND (@CategoryId IS NULL OR t.category_id = @CategoryId)
  AND (@Type IS NULL OR t.type = @Type)
  AND (@From IS NULL OR t.transaction_date >= @From)
  AND (@To IS NULL OR t.transaction_date <= @To);

SELECT t.id, t.account_id, a.name AS account_name, t.category_id, c.name AS category_name,
       t.type, t.amount, t.description, t.transaction_date, t.created_at
FROM transactions t
INNER JOIN accounts a ON a.id = t.account_id
LEFT JOIN categories c ON c.id = t.category_id
WHERE a.user_id = @UserId
  AND (@AccountId IS NULL OR t.account_id = @AccountId)
  AND (@CategoryId IS NULL OR t.category_id = @CategoryId)
  AND (@Type IS NULL OR t.type = @Type)
  AND (@From IS NULL OR t.transaction_date >= @From)
  AND (@To IS NULL OR t.transaction_date <= @To)
ORDER BY t.transaction_date DESC
LIMIT @PageSize OFFSET @Offset;
