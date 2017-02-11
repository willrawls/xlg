CREATE TABLE [dbo].[QuickScript]
(
	QuickScriptId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	QuickScriptName varchar(100) NOT NULL,
	Category varchar(50) NOT NULL DEFAULT 'None',
	ProcessType varchar(20) NOT NULL DEFAULT 'By line',
	Delimiter varchar(10) NOT NULL DEFAULT '[NewLine]',
	Content varchar(max),
	CreatedOn datetime NOT NULL,
	ModifiedOn datetime NULL,
	CreatedBy varchar(50) NOT NULL,
	ModifiedBy varchar(50) NULL,
)
