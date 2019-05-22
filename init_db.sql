DROP TABLE tblBoard
CREATE TABLE tblBoard (
	Id int IDENTITY(1,1) PRIMARY KEY,
	Name NVARCHAR(255) NOT NULL,
	Background NVARCHAR(50),
	isFavorite bit
)

INSERT INTO tblBoard(Name, Background, isFavorite) VALUES ('Things to do', 'blue', 1)
INSERT INTO tblBoard(Name, Background, isFavorite) VALUES ('Done', 'green', 0)
INSERT INTO tblBoard(Name, Background, isFavorite) VALUES ('Doing', 'space', 0)
INSERT INTO tblBoard(Name, Background, isFavorite) VALUES ('Shopping', 'blue', 0)
INSERT INTO tblBoard(Name, Background, isFavorite) VALUES ('Learning', 'orange', 0)
INSERT INTO tblBoard(Name, Background, isFavorite) VALUES ('Movies', 'movie', 1)
INSERT INTO tblBoard(Name, Background, isFavorite) VALUES ('Travels', 'paris', 1)