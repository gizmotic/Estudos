-- LIMPANDO LOG ( Sql Server 2005 )
BACKUP LOG [[NM_BANCO]] WITH TRUNCATE_ONLY
DBCC SHRINKDATABASE ('VOLPEHBUSTERAM',0)

-- LIMPANDO LOG ( Sql Server 2008 )
ALTER DATABASE VOLPEHBUSTERAM SET RECOVERY SIMPLE
DBCC SHRINKFILE ('volpehbusteram_log' , 1)
DBCC SHRINKDATABASE ( '[[NM_BANCO]]' , 0 )

-- ANALISAR INTEGRIDADE DO BANCO DE DADOS
dbcc checkdb