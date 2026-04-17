UPDATE users
SET full_name = @FullName, default_currency = @Currency
WHERE id = @Id
