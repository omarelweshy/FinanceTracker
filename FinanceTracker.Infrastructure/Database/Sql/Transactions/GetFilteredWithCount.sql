SELECT COUNT(*) FROM transactions t
INNER JOIN accounts a ON a.id = t.account_id
WHERE a.user_id = @UserId
  AND (@AccountId::uuid IS NULL OR t.account_id = @AccountId::uuid)
  AND (@CategoryId::uuid IS NULL OR t.category_id = @CategoryId::uuid)
  AND (@Type::text IS NULL OR t.type = @Type::text)
  AND (@From::timestamptz IS NULL OR t.transaction_date >= @From::timestamptz)
  AND (@To::timestamptz IS NULL OR t.transaction_date <= @To::timestamptz);

SELECT t.id, t.account_id, a.name AS account_name, t.category_id, c.name AS category_name,
       t.type, t.amount, t.description, t.transaction_date, t.created_at
FROM transactions t
INNER JOIN accounts a ON a.id = t.account_id
LEFT JOIN categories c ON c.id = t.category_id
WHERE a.user_id = @UserId
  AND (@AccountId::uuid IS NULL OR t.account_id = @AccountId::uuid)
  AND (@CategoryId::uuid IS NULL OR t.category_id = @CategoryId::uuid)
  AND (@Type::text IS NULL OR t.type = @Type::text)
  AND (@From::timestamptz IS NULL OR t.transaction_date >= @From::timestamptz)
  AND (@To::timestamptz IS NULL OR t.transaction_date <= @To::timestamptz)
ORDER BY t.transaction_date DESC
LIMIT @PageSize OFFSET @Offset;
