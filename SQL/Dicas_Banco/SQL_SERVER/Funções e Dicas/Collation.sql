SELECT 
	TAB.NAME AS TABELA,
	COL.NAME AS COLUNA,
	COL.XTYPE,
	COL.LENGTH
FROM 
	SYSCOLUMNS COL
	INNER JOIN SYSOBJECTS TAB ON TAB.ID=COL.ID
WHERE
	COLLATION <> 'SQL_Latin1_General_CP1_CI_AS'
ORDER BY
	TAB.NAME,
	COL.NAME
