--         prod by brand

CREATE OR REPLACE FUNCTION get_products_by_brand(wanted_brand INTEGER)
RETURNS TABLE (
    product_id INTEGER,
    product_name VARCHAR(45),
    product_price MONEY,
    product_description VARCHAR(200),
    product_quantity INTEGER,
    product_category VARCHAR(45),
    product_brand VARCHAR(45)
) AS $$
BEGIN
	RETURN QUERY
    SELECT p.id, p.name, p.price, p.description, p.quantity, c.name, b.name
    FROM products p
	JOIN categories c ON p.category_id=c.id
	JOIN brands b ON p.brand_id=b.id
    WHERE brand_id = wanted_brand;
END;
$$ LANGUAGE plpgsql;


-- Brands by the number of products

CREATE OR REPLACE FUNCTION get_brand_products()
RETURNS TABLE (b
	rand_id integer,
	brand_name VARCHAR(45),
	num_products BIGINT
) AS $$
BEGIN
  RETURN QUERY
  SELECT b.id, b.name, COUNT(p.id) AS num_products
  FROM brands b
  JOIN products p ON b.id = p.brand_id
  GROUP BY b.id
  ORDER BY num_products DESC;
END;
$$
LANGUAGE plpgsql;


-- Products by category and section?

CREATE OR REPLACE FUNCTION get_products_by_category_and_section(category_search_id integer,section_search_id integer)
RETURNS TABLE (
    product_id INT,
    product_name VARCHAR(45),
    product_price MONEY,
    product_description VARCHAR(200),
    product_quantity INT,
    category_name VARCHAR(45),
    brand_name VARCHAR(45)
) AS $$
BEGIN
    RETURN QUERY
    SELECT p.id, p.name, p.price, p.description, p.quantity, c.name, b.name
    FROM products p
    JOIN categories c ON p.category_id = c.id
    JOIN brands b ON p.brand_id = b.id
    JOIN categories_sections cs ON c.id = cs.category_id
    JOIN sections s ON cs.section_id = s.id
    WHERE c.id = category_search_id AND s.id = section_search_id;
END;
$$ LANGUAGE plpgsql;


-- completed orders for product

CREATE OR REPLACE FUNCTION get_completed_orders_for_product(p_product_id INT)
RETURNS TABLE (
  order_id INT,
  user_id INT,
  total_order_cost MONEY,
  count INT,
  complete_time TIMESTAMP
) AS $$
BEGIN
	RETURN QUERY
	SELECT o.id, o.user_id, o.total_cost, op.count, oh.set_time
    FROM orders o
    JOIN order_histories oh ON oh.order_id = o.id
    JOIN orders_products op ON op.order_id = o.id
    WHERE oh.status_id = 3
      AND op.product_id = p_product_id
    ORDER BY oh.set_time DESC;
END;
$$ LANGUAGE plpgsql;
