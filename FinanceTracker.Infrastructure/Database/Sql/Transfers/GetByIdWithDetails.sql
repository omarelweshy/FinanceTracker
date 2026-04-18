SELECT tr.id, tr.from_transaction_id, tr.to_transaction_id, tr.amount, tr.note, tr.created_at,
       fa.id AS from_account_id, fa.name AS from_account_name,
       ta.id AS to_account_id, ta.name AS to_account_name
FROM transfers tr
INNER JOIN transactions ft ON ft.id = tr.from_transaction_id
INNER JOIN transactions tt ON tt.id = tr.to_transaction_id
INNER JOIN accounts fa ON fa.id = ft.account_id
INNER JOIN accounts ta ON ta.id = tt.account_id
WHERE tr.id = @Id AND fa.user_id = @UserId;
