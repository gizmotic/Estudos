CD C:\BACKUP_SQL\QUARTA\
DEL LOG.TXT
DEL VolpeApaf.bak

osql -S(local) -E -dmaster -iC:\BACKUP_SQL\QUARTA\BKP_QUARTA.SQL -oC:\BACKUP_SQL\QUARTA\LOG.TXT

EXIT