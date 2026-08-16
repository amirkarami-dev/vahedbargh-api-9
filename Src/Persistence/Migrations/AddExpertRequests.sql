BEGIN TRANSACTION;
CREATE TABLE [ExpertRequests] (
    [Id] uniqueidentifier NOT NULL,
    [FullName] nvarchar(120) NOT NULL,
    [MobileNumber] nvarchar(20) NOT NULL,
    [NaCode] nvarchar(10) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [IsRead] bit NOT NULL,
    CONSTRAINT [PK_ExpertRequests] PRIMARY KEY ([Id])
);

CREATE INDEX [IX_ExpertRequests_CreatedAt] ON [ExpertRequests] ([CreatedAt]);

CREATE INDEX [IX_ExpertRequests_IsRead] ON [ExpertRequests] ([IsRead]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260816214750_AddExpertRequests', N'9.0.0');

COMMIT;
GO

