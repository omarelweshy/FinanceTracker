SELECT COUNT(1) FROM budgets
WHERE user_id = @UserId AND category_id = @CategoryId AND month = @Month AND year = @Year
