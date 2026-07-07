BEGIN TRANSACTION;
ALTER TABLE [ContactMessages] ADD [IsRead] bit NOT NULL DEFAULT CAST(0 AS bit);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260707210004_AddContactMessageIsRead', N'9.0.0');

COMMIT;
GO

