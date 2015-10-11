Create DataBase TestingSystem collate Cyrillic_General_CI_AS
GO

use TestingSystem
GO


Create table Question
(
	[QuestionId] int identity(1,1) NOT NULL,
	[QuestionValue] NVARCHAR (max) NOT NULL
)
GO

ALTER TABLE Question
add constraint 
PK_QUESTION_QuestionId PRIMARY KEY(QuestionId)
GO


CREATE TABLE Answer
(	
	[AnswerId] int identity(1,1) NOT NULL,
	[QuestionId] int NOT NULL,
	[AnswerValue]  NVARCHAR (max) NOT NULL
)
GO

ALTER TABLE Answer
add constraint 
PK_Answer_AnswerId PRIMARY KEY(AnswerId)
GO

ALTER TABLE Answer
add constraint 
FK_Answer_QuestionId FOREIGN KEY(QuestionId)
REFERENCES Question(QuestionId) 
GO


CREATE TABLE CorrectAnswer
(
	[CorrectAnswerId] int identity(1,1) NOT NULL,
	[QuestionId] int NOT NULL,
	[AnswerId] int NOT NULL
)
GO

ALTER TABLE CorrectAnswer
add constraint 
PK_CorrectAnswer_CorrectAnswerId PRIMARY KEY(CorrectAnswerId)
GO
	
ALTER TABLE CorrectAnswer
add constraint 
FK_CorrectAnswer_QuestionId FOREIGN KEY(QuestionId)
REFERENCES Question(QuestionId) 
GO

ALTER TABLE CorrectAnswer
add constraint 
FK_CorrectAnswer_AnswerId FOREIGN KEY(AnswerId)
REFERENCES Answer(AnswerId) 
GO


CREATE TABLE Result
(	
	[Id] int identity(1,1) NOT NULL,
	[Name] NVARCHAR (30) NOT NULL,
	[LastName] NVARCHAR (50) NOT NULL,
	[Group] NVARCHAR (50) NOT NULL,
	[DateAndTime] datetime2 (2) NOT NULL,
	[PCName] NVARCHAR(100) NULL,
	[IPAddress] NVARCHAR(100) NULL,
	[Points] int NOT NULL
)
GO

ALTER TABLE Result
add constraint 
PK_RESULT_Id PRIMARY KEY(Id)
GO