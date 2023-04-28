CREATE TABLE brands (
  id SERIAL PRIMARY KEY,
  name VARCHAR(45) UNIQUE NOT NULL
);

CREATE TABLE sections (
  id SERIAL PRIMARY KEY,
  name VARCHAR(45) UNIQUE NOT NULL
);

CREATE TABLE categories (
  id SERIAL PRIMARY KEY,
  name VARCHAR(45) UNIQUE NOT NULL,
  parent_category_id INT NULL,
  CONSTRAINT fk_category_category_id
    FOREIGN KEY (parent_category_id)
    REFERENCES categories (id)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT
);


CREATE TABLE products (
  id SERIAL PRIMARY KEY,
  name VARCHAR(45) NOT NULL,
  price MONEY NOT NULL,
  description VARCHAR(200) NOT NULL,
  quantity INT NOT NULL,
  category_id INT NOT NULL,
  brand_id INT NOT NULL,
  CONSTRAINT fk_products_category_id
    FOREIGN KEY (category_id)
    REFERENCES categories (id)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT fk_products_brand_id
    FOREIGN KEY (brand_id)
    REFERENCES brands (id)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT
);

CREATE TABLE categories_sections (
  category_id INT NOT NULL,
  section_id INT NOT NULL,
  PRIMARY KEY (category_id, section_id),
  CONSTRAINT fk_categories_sections_category_id
    FOREIGN KEY (category_id)
    REFERENCES categories (id)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT fk_categories_sections_section_id
    FOREIGN KEY (section_id)
    REFERENCES sections (id)
    ON DELETE CASCADE
    ON UPDATE CASCADE
);

CREATE TABLE roles (
  id SERIAL PRIMARY KEY,
  name VARCHAR(45) NOT NULL
);

CREATE TABLE users (
  id SERIAL PRIMARY KEY,
  email VARCHAR(45) UNIQUE NOT NULL,
  password_hash VARCHAR(64) NOT NULL,
  role_id INT NOT NULL,
  name VARCHAR(45) NOT NULL,
  surname VARCHAR(45) NOT NULL,
  CONSTRAINT fk_users_role_id
    FOREIGN KEY (role_id)
    REFERENCES roles (id)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT
);

CREATE TABLE contacts (
  id SERIAL PRIMARY KEY,
  user_id INT NOT NULL,
  phone_number VARCHAR(20) NOT NULL,
  FOREIGN KEY (user_id)
    REFERENCES users (id)
    ON DELETE CASCADE
    ON UPDATE CASCADE
);

CREATE TABLE addresses (
  id SERIAL PRIMARY KEY,
  user_id INT NOT NULL,
  full_address VARCHAR(200) NOT NULL,
  FOREIGN KEY (user_id)
    REFERENCES users (id)
    ON DELETE CASCADE
    ON UPDATE CASCADE
);


CREATE TABLE reviews (
  id SERIAL PRIMARY KEY,
  rating INT NOT NULL,
  comment VARCHAR(200) NOT NULL,
  user_id INT NULL,
  product_id INT NOT NULL,
  CONSTRAINT fk_reviews_user_id
    FOREIGN KEY (user_id)
    REFERENCES users (id)
    ON DELETE SET NULL
    ON UPDATE RESTRICT,
  CONSTRAINT fk_reviews_product_id
    FOREIGN KEY (product_id)
    REFERENCES products (id)
    ON DELETE CASCADE
    ON UPDATE CASCADE
);


CREATE TABLE images (
  id SERIAL PRIMARY KEY,
  path VARCHAR(256) NOT NULL,
  product_id INT NOT NULL,
  CONSTRAINT fk_images_product_id
    FOREIGN KEY (product_id)
    REFERENCES products (id)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT
);


CREATE TABLE statuses (
  id SERIAL PRIMARY KEY,
  name VARCHAR(45) NOT NULL
);


CREATE TABLE orders (
  id SERIAL PRIMARY KEY,
  user_id INT NOT NULL,
  total_cost DECIMAL(10,2) NOT NULL,
  CONSTRAINT fk_orders_user_id
    FOREIGN KEY (user_id)
    REFERENCES users (id)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT
);

CREATE TABLE order_histories (
  id SERIAL PRIMARY KEY,
  order_id INT NOT NULL,
  status_id INT NOT NULL,
  set_time TIMESTAMP NOT NULL,
  CONSTRAINT fk_orders_histories_order_id
    FOREIGN KEY (order_id)
    REFERENCES orders (id)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT fk_orders_histories_status_id
    FOREIGN KEY (status_id)
    REFERENCES statuses (id)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT
);

CREATE TABLE orders_products (
  order_id INT NOT NULL,
  product_id INT NOT NULL,
  count INT NOT NULL,
  PRIMARY KEY (order_id, product_id),
  CONSTRAINT fk_orders_products_order_id
    FOREIGN KEY (order_id)
    REFERENCES orders (id)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT fk_orders_products_product_id
    FOREIGN KEY (product_id)
    REFERENCES products (id)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT
);