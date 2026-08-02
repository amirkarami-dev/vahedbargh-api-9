BEGIN TRANSACTION;
UPDATE [ProcessSteps] SET [Number] = 2, [SortOrder] = 2
WHERE [Id] = 'f1000000-0000-0000-0000-000000000001';
SELECT @@ROWCOUNT;


UPDATE [ProcessSteps] SET [Number] = 3, [SortOrder] = 3
WHERE [Id] = 'f1000000-0000-0000-0000-000000000002';
SELECT @@ROWCOUNT;


UPDATE [ProcessSteps] SET [Number] = 4, [SortOrder] = 4
WHERE [Id] = 'f1000000-0000-0000-0000-000000000003';
SELECT @@ROWCOUNT;


UPDATE [ProcessSteps] SET [Number] = 5, [SortOrder] = 5
WHERE [Id] = 'f1000000-0000-0000-0000-000000000004';
SELECT @@ROWCOUNT;


UPDATE [ProcessSteps] SET [Number] = 6, [SortOrder] = 6
WHERE [Id] = 'f1000000-0000-0000-0000-000000000005';
SELECT @@ROWCOUNT;


UPDATE [ProcessSteps] SET [Number] = 7, [SortOrder] = 7
WHERE [Id] = 'f1000000-0000-0000-0000-000000000006';
SELECT @@ROWCOUNT;


UPDATE [ProcessSteps] SET [Number] = 8, [SortOrder] = 8
WHERE [Id] = 'f1000000-0000-0000-0000-000000000007';
SELECT @@ROWCOUNT;


UPDATE [ProcessSteps] SET [Number] = 9, [SortOrder] = 9
WHERE [Id] = 'f1000000-0000-0000-0000-000000000008';
SELECT @@ROWCOUNT;


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DecisionNo', N'DecisionYes', N'Description', N'Details', N'IsDecision', N'Note', N'Number', N'ProcessFlowId', N'RequiredDocs', N'SortOrder', N'Title', N'Tools') AND [object_id] = OBJECT_ID(N'[ProcessSteps]'))
    SET IDENTITY_INSERT [ProcessSteps] ON;
INSERT INTO [ProcessSteps] ([Id], [DecisionNo], [DecisionYes], [Description], [Details], [IsDecision], [Note], [Number], [ProcessFlowId], [RequiredDocs], [SortOrder], [Title], [Tools])
VALUES ('f1000000-0000-0000-0000-000000000000', NULL, NULL, N'برای بازرسی برق یا سیستم ارت، ابتدا مالک به شرکت توزیع مراجعه می‌کند. شرکت توزیع برای درخواست مالک یک پرونده در کارتابل دفتر اجرایی ایجاد می‌کند و ادامهٔ فرآیند در دفتر اجرایی انجام می‌شود.', CONCAT(CAST(N'مراجعهٔ مالک به شرکت توزیع برق' AS nvarchar(max)), nchar(10), N'ثبت درخواست و تشکیل پرونده در کارتابل دفتر اجرایی', nchar(10), N'ارجاع پرونده به دفتر اجرایی برای ادامهٔ فرآیند'), CAST(0 AS bit), NULL, 1, 'f0000000-0000-0000-0000-000000000001', NULL, 1, N'مراجعهٔ مالک و تشکیل پرونده', NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DecisionNo', N'DecisionYes', N'Description', N'Details', N'IsDecision', N'Note', N'Number', N'ProcessFlowId', N'RequiredDocs', N'SortOrder', N'Title', N'Tools') AND [object_id] = OBJECT_ID(N'[ProcessSteps]'))
    SET IDENTITY_INSERT [ProcessSteps] OFF;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260710054409_AddProcessIntakeStep', N'9.0.0');

COMMIT;
GO

