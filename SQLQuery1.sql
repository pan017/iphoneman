CREATE TABLE [dbo].[Admins] (
    [id]    INT            IDENTITY (1, 1) NOT NULL,
    [Login]  NVARCHAR (MAX) NULL,
    [Password] NVARCHAR (MAX) NULL,
);