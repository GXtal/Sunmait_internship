CREATE VIEW product_reviews AS
SELECT reviews.product_id,
	reviews.rating,
	reviews.comment,
	CONCAT(users.name, ' ', users.surname) AS commentator_name,
	users.email AS commentator_email
FROM reviews
	JOIN users ON reviews.user_id = users.id