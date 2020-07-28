DROP TABLE IF EXISTS "users" CASCADE;
DROP TABLE IF EXISTS "items" CASCADE;
DROP TABLE IF EXISTS "inventory" CASCADE;
DROP TRIGGER IF EXISTS sales_percent ON items CASCADE;
DROP FUNCTION IF EXISTS sales_percent;

CREATE TABLE users(
    user_id SERIAL Primary Key,
	user_email VARCHAR(30) UNIQUE,
    pw TEXT,
	isadmin BOOLEAN default false
);

CREATE TABLE items(
	item_id SERIAL Primary Key,
	item_name VARCHAR(50) UNIQUE,
	item_price INT,
	sale_percent INT,
	reg_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE inventory(
	user_id INT REFERENCES users(user_id) ON DELETE CASCADE,
	item_id INT REFERENCES items(item_id) ON DELETE CASCADE
);

CREATE FUNCTION sales_percent() returns trigger as $sales_percent$
Begin
	IF new.sale_percent>99 THEN
	raise 'You are not able to set the sale percent above 99.';
	end if;
	RETURN new ;
End;
$sales_percent$ LANGUAGE plpgsql;

INSERT INTO users(user_email,pw,isadmin)
VALUES ('admin','admin',true);
INSERT INTO users(user_email,pw)
VALUES ('user@gmail.com','user');
INSERT INTO users(user_email,pw)
VALUES ('user2@gmail.com','user2');

INSERT INTO items(item_name,item_price,sale_percent)
VALUES('eldobható bot',1300,10);
INSERT INTO items(item_name,item_price,sale_percent)
VALUES('visszaváltható sör',50,0);
INSERT INTO items(item_name,item_price,sale_percent)
VALUES('üvegtégla',790,0);

INSERT INTO inventory(user_id,item_id)
VALUES(2,2);
INSERT INTO inventory(user_id,item_id)
VALUES(2,3);






CREATE TRIGGER sales_percent
BEFORE UPDATE ON items
FOR EACH ROW 
EXECUTE PROCEDURE sales_percent();


SELECT items.item_name FROM items JOIN inventory ON (items.item_id = inventory.item_id)
JOIN users ON (inventory.user_id = users.user_id) WHERE users.user_id=2;

UPDATE items 
SET sale_percent = 99 
WHERE item_id = 2; 


select * from inventory;