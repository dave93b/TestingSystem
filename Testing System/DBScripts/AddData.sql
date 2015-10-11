INSERT Question
(QuestionValue)
VALUES
('��� ����� HTML?'),
('��� ����� CSS?'),
('��� ����� HTTP?'),
('��� ����� JavaScript?')
GO

INSERT Answer
(QuestionId, AnswerValue)
VALUES
('1', '���� ����������������'),
('1', '���� �������������� ��������'),
('1', '��������'),
('1', '���������� ������'),
('2', '���� ����������������'),
('2', '���� �������������� ��������'),
('2', '��������� ������� ������'),
('2', '������� ������ RGB'),
('3', '���� ����������������'),
('3', '���� �������������� ��������'),
('3', '��������'),
('3', '���'),
('4', '���'),
('4', '��������'),
('4', '���� ����������������'),
('4', '���������� ������')
GO

INSERT CorrectAnswer
(QuestionId, AnswerId)
VALUES
('1', '2'),
('2', '8'),
('3', '11'),
('4', '15')
GO