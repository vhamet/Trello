DROP TABLE tblUserBoard
DROP TABLE tblCard
DROP TABLE tblList
DROP TABLE tblBoard
DROP TABLE tblUser

CREATE TABLE tblBoard (
	BoardId int IDENTITY(1,1) PRIMARY KEY,
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

CREATE TABLE tblUser (
	UserId int IDENTITY(1,1) PRIMARY KEY,
	Username NVARCHAR(255) NOT NULL,
	Email NVARCHAR(255) NOT NULL,
	Password NVARCHAR(255) NOT NULL,
)
INSERT INTO tblUser(Username, Email, Password) VALUES ('TrelloUser', 'trello@user.com', '1234')
INSERT INTO tblUser(Username, Email, Password) VALUES ('usr', 'usr@user.com', '1234')

CREATE TABLE tblUserBoard (
	userId int FOREIGN KEY REFERENCES tblUser(UserId),
	boardId int FOREIGN KEY REFERENCES tblBoard(BoardId),
    PRIMARY KEY (userId, boardID)
)
INSERT INTO tblUserBoard(userId, boardId) VALUES (1,1)
INSERT INTO tblUserBoard(userId, boardId) VALUES (1,2)
INSERT INTO tblUserBoard(userId, boardId) VALUES (1,5)
INSERT INTO tblUserBoard(userId, boardId) VALUES (1,7)
INSERT INTO tblUserBoard(userId, boardId) VALUES (2,3)
INSERT INTO tblUserBoard(userId, boardId) VALUES (2,4)
INSERT INTO tblUserBoard(userId, boardId) VALUES (2,5)
INSERT INTO tblUserBoard(userId, boardId) VALUES (2,6)
INSERT INTO tblUserBoard(userId, boardId) VALUES (2,7)

CREATE TABLE tblList (
	ListId int IDENTITY(1,1) PRIMARY KEY,
	BoardId int FOREIGN KEY REFERENCES tblBoard(BoardId),
	Title NVARCHAR(255) NOT NULL,
	Position int NOT NULL
)
INSERT INTO tblList(BoardId, Title, Position) VALUES (5, 'JS', 0)
INSERT INTO tblList(BoardId, Title, Position) VALUES (5, 'C#', 1)
INSERT INTO tblList(BoardId, Title, Position) VALUES (5, 'Redux', 2)


CREATE TABLE tblCard (
	CardId int IDENTITY(1,1) PRIMARY KEY,
	ListId int FOREIGN KEY REFERENCES tblList(ListId),
	Title NVARCHAR(255) NOT NULL,
	Position int NOT NULL
)
INSERT INTO tblCard(ListId, Title, Position) VALUES (1, 'Closures', 0)
INSERT INTO tblCard(ListId, Title, Position) VALUES (1, 'Strict mode', 1)
INSERT INTO tblCard(ListId, Title, Position) VALUES (2, 'Events', 0)
INSERT INTO tblCard(ListId, Title, Position) VALUES (2, 'Threads', 1)
INSERT INTO tblCard(ListId, Title, Position) VALUES (2, 'Razor', 2)
INSERT INTO tblCard(ListId, Title, Position) VALUES (3, 'Tutorial', 0)

