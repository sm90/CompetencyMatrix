@echo off
call java -jar liquibase.jar --changeLogFile=dbMigrations/update.xml update