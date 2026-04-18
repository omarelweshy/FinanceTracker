SELECT t.id, t.account_id, a.name AS account_name, t.category_id, c.name AS category_name,
       t.type, t.amount, t.description, t.transaction_date, t.created_at
FROM transactions t
INNER JOIN accounts a ON a.id = t.account_id
LEFT JOIN categories c ON c.id = t.category_id
WHERE t.id = @Id AND a.user_id = @UserId;
