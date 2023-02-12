CREATE TABLE [dbo].[route]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [region_id] UNIQUEIDENTIFIER NULL, 
    CONSTRAINT [fk_route_toregion] FOREIGN KEY ([region_id]) REFERENCES [region]([Id])
)
