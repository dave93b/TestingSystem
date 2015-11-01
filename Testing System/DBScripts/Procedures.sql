CREATE PROC DelQuestionAndAnswers 
@QuestionId int
AS
DELETE From dbo.CorrectAnswer
WHERE CorrectAnswer.QuestionId = @QuestionId;

DELETE From dbo.Answer
WHERE Answer.QuestionId = @QuestionId;

DELETE From dbo.Question
WHERE Question.QuestionId = @QuestionId;

GO


CREATE PROC AddQuestion
@QuestionText nvarchar(max)
as
insert into Question
values (@QuestionText);
go

CREATE PROC AddAnswers
@QuestionId int,
@AnswerText1 nvarchar(max),
@AnswerText2 nvarchar(max),
@AnswerText3 nvarchar(max),
@AnswerText4 nvarchar(max)
as
insert into Answer
values (@QuestionId, @AnswerText1),
	   (@QuestionId, @AnswerText2),
	   (@QuestionId, @AnswerText3),
	   (@QuestionId, @AnswerText4);
go

CREATE PROC AddCorrectAnswer
@QuestionId int,
@CorrectAnswer int
as
insert into CorrectAnswer
values (@QuestionId, @CorrectAnswer);
go
