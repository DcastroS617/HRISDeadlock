param (
    [string]$SqlScriptPath
)

# Asegúrate de que la ruta esté correctamente entre comillas
$quotedSqlScriptPath = "`"D:\azagent\A1\_work\67\s\HRIS WEB\DOLE.HRIS.NUnit.Test\Scripts\cleantestdata.sql`""

# Define los argumentos de SQLCMD
$sqlcmdArguments = @(
    "-i", $quotedSqlScriptPath,
    "-b",
    "-S", "DTP-MEU-TSQL001.CORP.DOLE.COM",
    "-d", "HRIS_DEV",
    "-U", "CORP\LSA-DTP-HRISServDev",
    "-o", "`"D:\azagent\A1\_work\67\s\HRIS WEB\DOLE.HRIS.NUnit.Test\Scripts\output.log`""
)

# Ejecuta SQLCMD
& sqlcmd @sqlcmdArguments

# Captura errores si hay alguno
if ($LASTEXITCODE -ne 0) {
    Write-Error "SQLCMD execution failed with exit code $LASTEXITCODE"
    exit $LASTEXITCODE
} else {
    Write-Output "SQLCMD execution succeeded"
}
