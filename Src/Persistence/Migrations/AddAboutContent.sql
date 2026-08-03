BEGIN TRANSACTION;
CREATE TABLE [AboutContents] (
    [Id] uniqueidentifier NOT NULL,
    [PageTitle] nvarchar(max) NOT NULL,
    [Intro] nvarchar(max) NULL,
    [MissionsTitle] nvarchar(max) NULL,
    [OrgChartTitle] nvarchar(max) NULL,
    [BoardTitle] nvarchar(max) NULL,
    [DutiesTitle] nvarchar(max) NULL,
    CONSTRAINT [PK_AboutContents] PRIMARY KEY ([Id])
);

CREATE TABLE [AboutBoardMembers] (
    [Id] uniqueidentifier NOT NULL,
    [AboutContentId] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Role] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [SortOrder] int NOT NULL,
    CONSTRAINT [PK_AboutBoardMembers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AboutBoardMembers_AboutContents_AboutContentId] FOREIGN KEY ([AboutContentId]) REFERENCES [AboutContents] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AboutDuties] (
    [Id] uniqueidentifier NOT NULL,
    [AboutContentId] uniqueidentifier NOT NULL,
    [Text] nvarchar(max) NOT NULL,
    [SortOrder] int NOT NULL,
    CONSTRAINT [PK_AboutDuties] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AboutDuties_AboutContents_AboutContentId] FOREIGN KEY ([AboutContentId]) REFERENCES [AboutContents] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AboutMissions] (
    [Id] uniqueidentifier NOT NULL,
    [AboutContentId] uniqueidentifier NOT NULL,
    [IconName] nvarchar(max) NULL,
    [Title] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NULL,
    [SortOrder] int NOT NULL,
    CONSTRAINT [PK_AboutMissions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AboutMissions_AboutContents_AboutContentId] FOREIGN KEY ([AboutContentId]) REFERENCES [AboutContents] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AboutOrgNodes] (
    [Id] uniqueidentifier NOT NULL,
    [AboutContentId] uniqueidentifier NOT NULL,
    [Level] int NOT NULL,
    [Title] nvarchar(max) NOT NULL,
    [SortOrder] int NOT NULL,
    CONSTRAINT [PK_AboutOrgNodes] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AboutOrgNodes_AboutContents_AboutContentId] FOREIGN KEY ([AboutContentId]) REFERENCES [AboutContents] ([Id]) ON DELETE CASCADE
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'BoardTitle', N'DutiesTitle', N'Intro', N'MissionsTitle', N'OrgChartTitle', N'PageTitle') AND [object_id] = OBJECT_ID(N'[AboutContents]'))
    SET IDENTITY_INSERT [AboutContents] ON;
INSERT INTO [AboutContents] ([Id], [BoardTitle], [DutiesTitle], [Intro], [MissionsTitle], [OrgChartTitle], [PageTitle])
VALUES ('1a000000-0000-0000-0000-000000000001', N'هیئت رئیسه', N'شرح وظایف', N'دفتر اجرایی نظارت برق، یکی از واحدهای تخصصی سازمان نظام مهندسی ساختمان استان کردستان است که با هدف نظارت بر حسن اجرای پروژه‌های برق ساختمانی و ارتقاء ایمنی تأسیسات الکتریکی در استان فعالیت می‌نماید. این دفتر بر اساس نظام‌نامه مصوب مورخ ۱۳۹۳/۰۵/۰۲ هیئت مدیره سازمان تشکیل شده و فعالیت‌های خود را در چارچوب قوانین و مقررات ملی ساختمان انجام می‌دهد.', N'اهداف و مأموریت', N'ساختار سازمانی', N'درباره دفتر اجرایی');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'BoardTitle', N'DutiesTitle', N'Intro', N'MissionsTitle', N'OrgChartTitle', N'PageTitle') AND [object_id] = OBJECT_ID(N'[AboutContents]'))
    SET IDENTITY_INSERT [AboutContents] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AboutContentId', N'Description', N'Name', N'Role', N'SortOrder') AND [object_id] = OBJECT_ID(N'[AboutBoardMembers]'))
    SET IDENTITY_INSERT [AboutBoardMembers] ON;
INSERT INTO [AboutBoardMembers] ([Id], [AboutContentId], [Description], [Name], [Role], [SortOrder])
VALUES ('1d000000-0000-0000-0000-000000000001', '1a000000-0000-0000-0000-000000000001', N'منتخب هیئت مدیره سازمان', N'مدیر اجرایی دفتر', N'رئیس هیئت رئیسه', 1),
('1d000000-0000-0000-0000-000000000002', '1a000000-0000-0000-0000-000000000001', N'نماینده انتصابی هیئت مدیره', N'نماینده هیئت مدیره', N'عضو هیئت رئیسه', 2),
('1d000000-0000-0000-0000-000000000003', '1a000000-0000-0000-0000-000000000001', N'منتخب مجمع عمومی', N'نماینده مجمع عمومی', N'عضو هیئت رئیسه', 3);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AboutContentId', N'Description', N'Name', N'Role', N'SortOrder') AND [object_id] = OBJECT_ID(N'[AboutBoardMembers]'))
    SET IDENTITY_INSERT [AboutBoardMembers] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AboutContentId', N'SortOrder', N'Text') AND [object_id] = OBJECT_ID(N'[AboutDuties]'))
    SET IDENTITY_INSERT [AboutDuties] ON;
INSERT INTO [AboutDuties] ([Id], [AboutContentId], [SortOrder], [Text])
VALUES ('1e000000-0000-0000-0000-000000000001', '1a000000-0000-0000-0000-000000000001', 1, N'نظارت بر اجرای صحیح ضوابط و مقررات فنی مربوط به تأسیسات برقی'),
('1e000000-0000-0000-0000-000000000002', '1a000000-0000-0000-0000-000000000001', 2, N'بررسی و تأیید صلاحیت کارشناسان برق'),
('1e000000-0000-0000-0000-000000000003', '1a000000-0000-0000-0000-000000000001', 3, N'صدور پروانه اشتغال و کنترل کیفیت'),
('1e000000-0000-0000-0000-000000000004', '1a000000-0000-0000-0000-000000000001', 4, N'رسیدگی به شکایات و تخلفات'),
('1e000000-0000-0000-0000-000000000005', '1a000000-0000-0000-0000-000000000001', 5, N'همکاری با ادارات و سازمان‌های دولتی ذیربط'),
('1e000000-0000-0000-0000-000000000006', '1a000000-0000-0000-0000-000000000001', 6, N'برگزاری دوره‌های آموزشی و مهارت‌افزایی'),
('1e000000-0000-0000-0000-000000000007', '1a000000-0000-0000-0000-000000000001', 7, N'تهیه و به‌روزرسانی دستورالعمل‌های فنی'),
('1e000000-0000-0000-0000-000000000008', '1a000000-0000-0000-0000-000000000001', 8, N'تدوین تعرفه‌های خدمات مهندسی برق');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AboutContentId', N'SortOrder', N'Text') AND [object_id] = OBJECT_ID(N'[AboutDuties]'))
    SET IDENTITY_INSERT [AboutDuties] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AboutContentId', N'Description', N'IconName', N'SortOrder', N'Title') AND [object_id] = OBJECT_ID(N'[AboutMissions]'))
    SET IDENTITY_INSERT [AboutMissions] ON;
INSERT INTO [AboutMissions] ([Id], [AboutContentId], [Description], [IconName], [SortOrder], [Title])
VALUES ('1b000000-0000-0000-0000-000000000001', '1a000000-0000-0000-0000-000000000001', N'نظارت بر حسن اجرای پروژه‌های برق ساختمانی مطابق با مقررات ملی و استانداردهای فنی', N'Shield', 1, N'نظارت فنی'),
('1b000000-0000-0000-0000-000000000002', '1a000000-0000-0000-0000-000000000001', N'ارتقاء کیفیت اجرای کارهای برق و ایمنی تأسیسات الکتریکی در استان کردستان', N'Target', 2, N'ارتقاء کیفیت'),
('1b000000-0000-0000-0000-000000000003', '1a000000-0000-0000-0000-000000000001', N'آموزش و توانمندسازی کارشناسان و ناظران حوزه برق ساختمان', N'BookOpen', 3, N'آموزش و توسعه'),
('1b000000-0000-0000-0000-000000000004', '1a000000-0000-0000-0000-000000000001', N'هماهنگی بین سازمان‌های مرتبط و یکپارچه‌سازی فرآیندهای اجرایی', N'Users', 4, N'هماهنگی سازمانی');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AboutContentId', N'Description', N'IconName', N'SortOrder', N'Title') AND [object_id] = OBJECT_ID(N'[AboutMissions]'))
    SET IDENTITY_INSERT [AboutMissions] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AboutContentId', N'Level', N'SortOrder', N'Title') AND [object_id] = OBJECT_ID(N'[AboutOrgNodes]'))
    SET IDENTITY_INSERT [AboutOrgNodes] ON;
INSERT INTO [AboutOrgNodes] ([Id], [AboutContentId], [Level], [SortOrder], [Title])
VALUES ('1c000000-0000-0000-0000-000000000001', '1a000000-0000-0000-0000-000000000001', 0, 1, N'هیئت مدیره سازمان نظام مهندسی'),
('1c000000-0000-0000-0000-000000000002', '1a000000-0000-0000-0000-000000000001', 1, 1, N'هیئت رئیسه دفتر اجرایی (۳ نفر)'),
('1c000000-0000-0000-0000-000000000003', '1a000000-0000-0000-0000-000000000001', 2, 1, N'مدیر اجرایی'),
('1c000000-0000-0000-0000-000000000004', '1a000000-0000-0000-0000-000000000001', 2, 2, N'نمایندگان شهرستان‌ها');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AboutContentId', N'Level', N'SortOrder', N'Title') AND [object_id] = OBJECT_ID(N'[AboutOrgNodes]'))
    SET IDENTITY_INSERT [AboutOrgNodes] OFF;

CREATE INDEX [IX_AboutBoardMembers_AboutContentId] ON [AboutBoardMembers] ([AboutContentId]);

CREATE INDEX [IX_AboutDuties_AboutContentId] ON [AboutDuties] ([AboutContentId]);

CREATE INDEX [IX_AboutMissions_AboutContentId] ON [AboutMissions] ([AboutContentId]);

CREATE INDEX [IX_AboutOrgNodes_AboutContentId] ON [AboutOrgNodes] ([AboutContentId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260803203949_AddAboutContent', N'9.0.0');

COMMIT;
GO

