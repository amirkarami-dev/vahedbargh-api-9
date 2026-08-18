BEGIN TRANSACTION;
ALTER TABLE [ElectProjects] ADD [SamtIdentityCode] nvarchar(max) NULL;

ALTER TABLE [ElectProjects] ADD [SamtLicenseDate] nvarchar(max) NULL;

ALTER TABLE [ElectProjects] ADD [SamtLicenseExpireDate] nvarchar(max) NULL;

ALTER TABLE [ElectProjects] ADD [SamtLicenseNumber] nvarchar(max) NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260818151420_AddSamtFields', N'9.0.0');

COMMIT;
GO

