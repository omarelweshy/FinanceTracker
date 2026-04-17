SELECT tr.* FROM transfers tr
INNER JOIN transactions t ON t.id = tr.from_transaction_id
INNER JOIN accounts a ON a.id = t.account_id
WHERE a.user_id = @UserId
ORDER BY tr.created_at DESC
