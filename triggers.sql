CREATE OR REPLACE FUNCTION update_order_history_set_time()
RETURNS TRIGGER AS $$
BEGIN
	NEW.set_time := NOW();
	RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER tr_update_order_history_set_time
	BEFORE INSERT ON order_histories
	FOR EACH ROW
	EXECUTE FUNCTION update_order_history_set_time();