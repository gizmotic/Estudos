		DECLARE @TABLE_NAME SYSNAME
				DECLARE @TMPTABLESIZE TABLE (
				NAME       SYSNAME     NULL
				, ROWS       INT         NULL
				, RESERVED   VARCHAR(30) NULL
				, DATA       VARCHAR(30) NULL
				, INDEX_SIZE VARCHAR(30) NULL
				, UNUSED     VARCHAR(30) NULL )

				DECLARE TMPCURSOR CURSOR LOCAL FAST_FORWARD READ_ONLY FOR
				SELECT NAME
				FROM SYSOBJECTS
				WHERE TYPE = 'U'
				ORDER BY NAME

				OPEN TMPCURSOR
				FETCH NEXT FROM TMPCURSOR INTO @TABLE_NAME

				WHILE @@FETCH_STATUS = 0
				BEGIN
					INSERT INTO @TMPTABLESIZE (NAME, ROWS, RESERVED, DATA, INDEX_SIZE, UNUSED) EXEC SP_SPACEUSED @TABLE_NAME
					FETCH NEXT FROM TMPCURSOR INTO @TABLE_NAME
				END
				CLOSE TMPCURSOR
				DEALLOCATE TMPCURSOR

				SELECT NAME AS 'TABLENAME'
				   , ROWS AS 'ROWS'
				   , CONVERT(INT, REPLACE(RESERVED, ' KB','')) AS 'RESERVED_KB'
				   , CONVERT(INT, REPLACE(DATA, ' KB',''))AS 'DATA_KB'
				   , CONVERT(INT, REPLACE(INDEX_SIZE, ' KB',''))AS 'INDEX_KB'
				   , CONVERT(INT, REPLACE(UNUSED, ' KB',''))AS 'UNUSED_KB'
				FROM @TMPTABLESIZE
				ORDER BY 2 DESC