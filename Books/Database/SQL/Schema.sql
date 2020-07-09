CREATE TABLE IF NOT EXISTS Authors
(
  AuthorID   INTEGER NOT NULL CONSTRAINT PK_Authors PRIMARY KEY AUTOINCREMENT,
  LastName   TEXT    NOT NULL,
  FirstName  TEXT    NULL,
  MiddleName TEXT    NULL,
  Search     TEXT    NOT NULL
);
CREATE INDEX IF NOT EXISTS IX_Authors_Search ON Authors(Search);

CREATE TABLE IF NOT EXISTS Genres
(
  GenreID    INTEGER NOT NULL CONSTRAINT PK_Genres PRIMARY KEY AUTOINCREMENT,
  Fb2Code    TEXT    NOT NULL,
  GenreAlias TEXT    NOT NULL,
  Search     TEXT    NOT NULL
);
CREATE INDEX IF NOT EXISTS IX_Genres_Search ON Genres(Search);

CREATE TABLE IF NOT EXISTS Series
(
  SeriesID    INTEGER NOT NULL CONSTRAINT PK_Series PRIMARY KEY AUTOINCREMENT,
  SeriesTitle TEXT    NOT NULL,
  Search      TEXT    NOT NULL
);
CREATE INDEX IF NOT EXISTS IX_Series_Search ON Series(Search);

CREATE TABLE IF NOT EXISTS Books
(
  BookID     INTEGER NOT NULL CONSTRAINT PK_Books PRIMARY KEY AUTOINCREMENT,
  LibID      TEXT    NOT NULL,
  Title      TEXT    NOT NULL,
  SeriesID   INTEGER NULL,
  SeqNumber  TEXT    NULL,
  UpdateDate TEXT    NOT NULL,
  LibRate    INTEGER NULL,
  Lang       TEXT    NOT NULL,
  Folder     TEXT    NOT NULL,
  FileName   TEXT    NOT NULL,
  InsideNo   INTEGER NOT NULL,
  Ext        TEXT    NOT NULL,
  BookSize   INTEGER NOT NULL,
  KeyWords   TEXT    NULL,
  Annotation TEXT    NULL,
  Review     TEXT    NULL,
  Search     TEXT    NOT NULL,
  CONSTRAINT FK_Books_Series_SeriesId FOREIGN KEY (SeriesID) REFERENCES Series (SeriesID) ON DELETE RESTRICT
);
CREATE INDEX IF NOT EXISTS IX_Books_Search ON Books (Search);
CREATE INDEX IF NOT EXISTS IX_Books_UpdateDate ON Books (UpdateDate DESC);

CREATE TABLE IF NOT EXISTS Author_List
(
  BookID   INTEGER NOT NULL,
  AuthorID INTEGER NOT NULL,
  CONSTRAINT PK_Author_List PRIMARY KEY (BookID, AuthorID),
  CONSTRAINT FK_Author_List_Authors_AuthorID FOREIGN KEY (AuthorID) REFERENCES Authors (AuthorID) ON DELETE RESTRICT,
  CONSTRAINT FK_Author_List_Books_BookID FOREIGN KEY (BookID) REFERENCES Books (BookID) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Genre_List
(
  BookID  INTEGER NOT NULL,
  GenreID INTEGER NOT NULL,
  CONSTRAINT PK_Genre_List PRIMARY KEY (BookID, GenreID),
  CONSTRAINT FK_Genre_List_Books_BookID FOREIGN KEY (BookID) REFERENCES Books (BookID) ON DELETE CASCADE,
  CONSTRAINT FK_Genre_List_Genres_GenreCode FOREIGN KEY (GenreID) REFERENCES Genres (GenreID) ON DELETE RESTRICT
);

CREATE VIEW IF NOT EXISTS v_Book_Authors
AS
SELECT Author_List.BookId, Author_List.AuthorID, Authors.LastName, Authors.FirstName, Authors.MiddleName
FROM Author_List INNER JOIN
  Authors ON Authors.AuthorID = Author_List.AuthorID;

CREATE VIEW IF NOT EXISTS v_Book_Genres
AS
SELECT Genre_List.BookId, Genre_List.GenreID, Genres.GenreAlias
FROM Genre_List INNER JOIN
  Genres ON Genres.GenreID = Genre_List.GenreID;
