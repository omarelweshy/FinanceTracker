UPDATE accounts
SET name = @Name, type = @Type, balance = @Balance, is_active = @IsActive
WHERE id = @Id
