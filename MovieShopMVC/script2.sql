BEGIN TRANSACTION;
GO

DROP INDEX [IX_Purchase_UserId] ON [Purchase];
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Purchase]') AND [c].[name] = N'PurchaseNumber');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Purchase] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Purchase] ALTER COLUMN [PurchaseNumber] uniqueidentifier NOT NULL;
GO

ALTER TABLE [Purchase] ADD CONSTRAINT [AK_Purchase_UserId_MovieId] UNIQUE ([UserId], [MovieId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220315214902_MakePurchaseUseUniqueConstrain', N'6.0.3');
GO

COMMIT;
GO

