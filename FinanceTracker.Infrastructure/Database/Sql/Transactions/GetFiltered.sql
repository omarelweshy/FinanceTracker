SELECT t.* FROM transactions t
INNER JOIN accounts a ON a.id = t.account_id
WHERE a.user_id = @UserId
  AND (@AccountId IS NULL OR t.account_id = @AccountId)
  AND (@CategoryId IS NULL OR t.category_id = @CategoryId)
  AND (@Type IS NULL OR t.type = @Type)
  AND (@From IS NULL OR t.transaction_date >= @From)
  AND (@To IS NULL OR t.transaction_date <= @To)
ORDER BY t.transaction_date DESC
LIMIT @PageSize OFFSET @Offset
