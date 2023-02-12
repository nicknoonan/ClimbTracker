CREATE TABLE [dbo].[media]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [type] INT NOT NULL, 
    [region_id] UNIQUEIDENTIFIER NULL, 
    [route_id] UNIQUEIDENTIFIER NULL, 
    CONSTRAINT [fk_media_toregion] FOREIGN KEY (region_id) REFERENCES [region]([id]), 
    CONSTRAINT [fk_media_toroute] FOREIGN KEY ([route_id]) REFERENCES [route]([id]) 
)
