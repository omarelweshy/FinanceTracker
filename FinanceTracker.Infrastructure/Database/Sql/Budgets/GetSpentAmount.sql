SELECT COALESCE(SUM(t.amount), 0)
FROM transactions t
INNER JOIN accounts a ON a.id = t.account_id
WHERE a.user_id = @UserId
  AND t.category_id = @CategoryId
  AND t.type = 'Expense'
  AND EXTRACT(MONTH FROM t.transaction_date) = @Month
  AND EXTRACT(YEAR FROM t.transaction_date) = @Year
