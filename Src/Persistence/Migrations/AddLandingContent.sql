BEGIN TRANSACTION;
CREATE TABLE [Announcements] (
    [Id] uniqueidentifier NOT NULL,
    [Slug] nvarchar(450) NOT NULL,
    [Title] nvarchar(max) NOT NULL,
    [Excerpt] nvarchar(max) NULL,
    [Content] nvarchar(max) NULL,
    [Category] nvarchar(max) NULL,
    [Priority] nvarchar(max) NOT NULL,
    [JalaliDate] nvarchar(max) NULL,
    [PublishedAt] datetime2 NOT NULL,
    [ImageUrl] nvarchar(max) NULL,
    [Featured] bit NOT NULL,
    CONSTRAINT [PK_Announcements] PRIMARY KEY ([Id])
);

CREATE TABLE [ContactMessages] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NULL,
    [Mobile] nvarchar(max) NULL,
    [Subject] nvarchar(max) NULL,
    [Message] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_ContactMessages] PRIMARY KEY ([Id])
);

CREATE TABLE [Documents] (
    [Id] uniqueidentifier NOT NULL,
    [Title] nvarchar(max) NOT NULL,
    [Category] nvarchar(max) NOT NULL,
    [JalaliDate] nvarchar(max) NULL,
    [Date] datetime2 NOT NULL,
    [Version] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [FileSize] nvarchar(max) NULL,
    [DownloadCount] int NOT NULL,
    [Tags] nvarchar(max) NULL,
    [FileUrl] nvarchar(max) NULL,
    [Featured] bit NOT NULL,
    CONSTRAINT [PK_Documents] PRIMARY KEY ([Id])
);

CREATE TABLE [Meetings] (
    [Id] uniqueidentifier NOT NULL,
    [SessionNumber] int NOT NULL,
    [Subject] nvarchar(max) NOT NULL,
    [JalaliDate] nvarchar(max) NULL,
    [Date] datetime2 NOT NULL,
    [Status] nvarchar(max) NULL,
    [Type] nvarchar(max) NULL,
    [PdfUrl] nvarchar(max) NULL,
    [Attendees] nvarchar(max) NULL,
    [Notes] nvarchar(max) NULL,
    CONSTRAINT [PK_Meetings] PRIMARY KEY ([Id])
);

CREATE TABLE [StatItems] (
    [Id] uniqueidentifier NOT NULL,
    [Label] nvarchar(max) NOT NULL,
    [Value] int NOT NULL,
    [Suffix] nvarchar(max) NULL,
    [IconName] nvarchar(max) NULL,
    [SortOrder] int NOT NULL,
    CONSTRAINT [PK_StatItems] PRIMARY KEY ([Id])
);

CREATE TABLE [MeetingResolutions] (
    [Id] uniqueidentifier NOT NULL,
    [MeetingId] uniqueidentifier NOT NULL,
    [Text] nvarchar(max) NOT NULL,
    [Status] nvarchar(max) NULL,
    CONSTRAINT [PK_MeetingResolutions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MeetingResolutions_Meetings_MeetingId] FOREIGN KEY ([MeetingId]) REFERENCES [Meetings] ([Id]) ON DELETE CASCADE
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Category', N'Content', N'Excerpt', N'Featured', N'ImageUrl', N'JalaliDate', N'Priority', N'PublishedAt', N'Slug', N'Title') AND [object_id] = OBJECT_ID(N'[Announcements]'))
    SET IDENTITY_INSERT [Announcements] ON;
INSERT INTO [Announcements] ([Id], [Category], [Content], [Excerpt], [Featured], [ImageUrl], [JalaliDate], [Priority], [PublishedAt], [Slug], [Title])
VALUES ('a0000000-0000-0000-0000-000000000001', N'تعرفه و مالی', N'تعرفه جدید اجرای الکترود زمین برای سال ۱۴۰۵ توسط هیئت رئیسه دفتر اجرایی نظارت برق استان کردستان تصویب و ابلاغ گردید.', N'تعرفه‌های پیشنهادی اجرای ارت برای سال ۱۴۰۵ ابلاغ شد.', CAST(1 AS bit), NULL, N'1405/02/01', N'urgent', '2026-04-21T00:00:00.0000000', N'tarife-electrode-zamin-1405', N'ابلاغ تعرفه جدید اجرای الکترود زمین — اردیبهشت ۱۴۰۵'),
('a0000000-0000-0000-0000-000000000002', N'اسناد سازمانی', N'نظام‌نامه نحوه تشکیل و اداره دفتر اجرایی نظارت برق در بخش آرشیو اسناد این سامانه قابل دسترسی می‌باشد.', N'نظام‌نامه نحوه تشکیل و اداره دفتر اجرایی نظارت برق مصوب هیئت مدیره سازمان در سایت منتشر شد.', CAST(0 AS bit), NULL, N'1405/01/20', N'info', '2026-04-09T00:00:00.0000000', N'nazamnameh-daftar-ejrayi', N'انتشار نظام‌نامه نحوه تشکیل و اداره دفتر اجرایی'),
('a0000000-0000-0000-0000-000000000003', N'کارشناسان', N'کارشناسانی که درخواست تمدید صلاحیت دارند باید مدارک لازم را تا پایان اردیبهشت ماه ارائه دهند.', N'ارزیابی دوره‌ای عملکرد کلیه کارشناسان فعال در بهار ۱۴۰۵ انجام می‌شود.', CAST(0 AS bit), NULL, N'1405/01/15', N'important', '2026-04-04T00:00:00.0000000', N'bazresi-parvane-barq-1405', N'فراخوان ارزیابی عملکرد کارشناسان برق — بهار ۱۴۰۵'),
('a0000000-0000-0000-0000-000000000004', N'فرم‌ها و مدارک', N'فرم‌های جدید شامل چک‌لیست بازرسی، فرم تأییدیه اجرا و گزارش پایان کار در بخش آرشیو اسناد قابل دانلود می‌باشد.', N'فرم‌های استاندارد نظارت و پذیرش پروژه‌های برق برای سال ۱۴۰۵ به‌روزرسانی شد.', CAST(0 AS bit), NULL, N'1405/01/10', N'info', '2026-03-30T00:00:00.0000000', N'dowload-form-namazh-paziresh', N'انتشار فرم‌های جدید نظارت و پذیرش پروژه‌های برق');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Category', N'Content', N'Excerpt', N'Featured', N'ImageUrl', N'JalaliDate', N'Priority', N'PublishedAt', N'Slug', N'Title') AND [object_id] = OBJECT_ID(N'[Announcements]'))
    SET IDENTITY_INSERT [Announcements] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Category', N'Date', N'Description', N'DownloadCount', N'Featured', N'FileSize', N'FileUrl', N'JalaliDate', N'Tags', N'Title', N'Version') AND [object_id] = OBJECT_ID(N'[Documents]'))
    SET IDENTITY_INSERT [Documents] ON;
INSERT INTO [Documents] ([Id], [Category], [Date], [Description], [DownloadCount], [Featured], [FileSize], [FileUrl], [JalaliDate], [Tags], [Title], [Version])
VALUES ('d0000000-0000-0000-0000-000000000001', N'نظام‌نامه', '2014-07-24T00:00:00.0000000', N'مصوب جلسه مورخ ۱۳۹۳/۰۵/۰۲ هیئت مدیره سازمان نظام مهندسی ساختمان استان کردستان', 847, CAST(1 AS bit), N'2.4 MB', NULL, N'1393/05/02', N'هیئت رئیسه,شرح وظایف,مدیر اجرایی', N'نظام‌نامه نحوه تشکیل و اداره دفتر اجرایی نظارت برق', N'1.0'),
('d0000000-0000-0000-0000-000000000002', N'تعرفه', '2026-04-21T00:00:00.0000000', N'تعرفه اجرای الکترود زمین — شامل الکترود ساده، اساسی، افقی و فونداسیون', 1243, CAST(1 AS bit), N'1.1 MB', NULL, N'1405/02/01', N'الکترود زمین,تعرفه,اجرا', N'قیمت‌های پیشنهادی اجرای ارت (اردیبهشت ماه ۱۴۰۵)', N'3.0'),
('d0000000-0000-0000-0000-000000000003', N'چک‌لیست', '2025-09-01T00:00:00.0000000', N'چک‌لیست جامع بازرسی تأسیسات برقی ساختمان‌های مسکونی و تجاری', 632, CAST(0 AS bit), N'0.8 MB', NULL, N'1404/06/10', N'بازرسی,تأسیسات برقی,ساختمان', N'چک‌لیست بازرسی تأسیسات برقی ساختمان', N'2.1'),
('d0000000-0000-0000-0000-000000000004', N'فرم اجرایی', '2026-01-01T00:00:00.0000000', N'فرم استاندارد گزارش پایان کار پروژه‌های برقی و تأییدیه نظارت', 415, CAST(0 AS bit), N'0.5 MB', NULL, N'1404/10/11', N'فرم,پذیرش,پایان کار', N'فرم گزارش پایان کار و پذیرش نظارت برق', N'2.0');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Category', N'Date', N'Description', N'DownloadCount', N'Featured', N'FileSize', N'FileUrl', N'JalaliDate', N'Tags', N'Title', N'Version') AND [object_id] = OBJECT_ID(N'[Documents]'))
    SET IDENTITY_INSERT [Documents] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Attendees', N'Date', N'JalaliDate', N'Notes', N'PdfUrl', N'SessionNumber', N'Status', N'Subject', N'Type') AND [object_id] = OBJECT_ID(N'[Meetings]'))
    SET IDENTITY_INSERT [Meetings] ON;
INSERT INTO [Meetings] ([Id], [Attendees], [Date], [JalaliDate], [Notes], [PdfUrl], [SessionNumber], [Status], [Subject], [Type])
VALUES ('b0000000-0000-0000-0000-000000000010', NULL, '2026-04-03T00:00:00.0000000', N'1405/01/14', NULL, N'/documents/meeting-10.pdf', 10, N'مصوبه صادر شد', N'تصویب برنامه آموزشی کارشناسان برق — نیمه اول ۱۴۰۵', N'هیئت رئیسه'),
('b0000000-0000-0000-0000-000000000011', NULL, '2026-04-17T00:00:00.0000000', N'1405/01/28', NULL, NULL, 11, N'در حال پیگیری', N'ارزیابی عملکرد کارشناسان و بررسی تخلفات گزارش‌شده', N'هیئت رئیسه'),
('b0000000-0000-0000-0000-000000000012', N'مدیر اجرایی,نماینده هیئت مدیره,کارشناس فنی ارشد', '2026-05-05T00:00:00.0000000', N'1405/02/15', NULL, N'/documents/meeting-12.pdf', 12, N'مصوبه صادر شد', N'بررسی تعرفه اجرای الکترود زمین و تصویب نرخ‌های جدید برای سال ۱۴۰۵', N'هیئت رئیسه');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Attendees', N'Date', N'JalaliDate', N'Notes', N'PdfUrl', N'SessionNumber', N'Status', N'Subject', N'Type') AND [object_id] = OBJECT_ID(N'[Meetings]'))
    SET IDENTITY_INSERT [Meetings] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'IconName', N'Label', N'SortOrder', N'Suffix', N'Value') AND [object_id] = OBJECT_ID(N'[StatItems]'))
    SET IDENTITY_INSERT [StatItems] ON;
INSERT INTO [StatItems] ([Id], [IconName], [Label], [SortOrder], [Suffix], [Value])
VALUES ('e0000000-0000-0000-0000-000000000001', N'FileCheck', N'پروانه صادرشده', 1, N'+', 12480),
('e0000000-0000-0000-0000-000000000002', N'Users', N'کارشناس ثبت‌شده', 2, N'', 342),
('e0000000-0000-0000-0000-000000000003', N'Activity', N'پروژه فعال', 3, N'', 894),
('e0000000-0000-0000-0000-000000000004', N'ShieldCheck', N'بازرسی انجام‌شده', 4, N'+', 2156);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'IconName', N'Label', N'SortOrder', N'Suffix', N'Value') AND [object_id] = OBJECT_ID(N'[StatItems]'))
    SET IDENTITY_INSERT [StatItems] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'MeetingId', N'Status', N'Text') AND [object_id] = OBJECT_ID(N'[MeetingResolutions]'))
    SET IDENTITY_INSERT [MeetingResolutions] ON;
INSERT INTO [MeetingResolutions] ([Id], [MeetingId], [Status], [Text])
VALUES ('c0000000-0000-0000-0000-000000100001', 'b0000000-0000-0000-0000-000000000010', N'در دست اقدام', N'برگزاری ۴ دوره آموزشی در نیمه اول سال'),
('c0000000-0000-0000-0000-000000110001', 'b0000000-0000-0000-0000-000000000011', N'در دست اقدام', N'تشکیل کمیته ارزیابی سه‌نفره برای بررسی پرونده‌های تخلف'),
('c0000000-0000-0000-0000-000000120001', 'b0000000-0000-0000-0000-000000000012', N'اجرا شده', N'تصویب تعرفه‌های اجرای الکترود زمین برای اردیبهشت ۱۴۰۵'),
('c0000000-0000-0000-0000-000000120002', 'b0000000-0000-0000-0000-000000000012', N'در دست اقدام', N'الزام کارشناسان به استفاده از تعرفه جدید از ابتدای خرداد ۱۴۰۵');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'MeetingId', N'Status', N'Text') AND [object_id] = OBJECT_ID(N'[MeetingResolutions]'))
    SET IDENTITY_INSERT [MeetingResolutions] OFF;

CREATE UNIQUE INDEX [IX_Announcements_Slug] ON [Announcements] ([Slug]);

CREATE INDEX [IX_MeetingResolutions_MeetingId] ON [MeetingResolutions] ([MeetingId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260707173219_AddLandingContent', N'9.0.0');

COMMIT;
GO

