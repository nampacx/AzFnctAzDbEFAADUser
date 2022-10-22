CREATE TABLE dbo.ToDo (
    Id uniqueidentifier primary key,
    [order] int null,
    title nvarchar(200) not null,
    [url] nvarchar(200) not null,
    completed bit not null
);

INSERT INTO [dbo].[ToDo] (Id, [order], title, [url], completed) values (NewID(), 1, 'test', 'https://bing.com', 0)