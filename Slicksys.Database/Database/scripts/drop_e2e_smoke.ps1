[CmdletBinding()]
param(
    [string]$Server = '(localdb)\ProjectModels',
    [string]$Database = 'managementdata_e2e'
)

$ErrorActionPreference = 'Stop'

Write-Host "Dropping database [$Database] on [$Server] if it exists..."
$dropOnly = @"
if db_id(N'$Database') is not null
begin
    alter database [$Database] set single_user with rollback immediate;
    drop database [$Database];
end;
"@

sqlcmd -S $Server -d master -b -Q $dropOnly | Out-Host
if ($LASTEXITCODE -ne 0) {
    throw 'failed to drop database.'
}

Write-Host 'Database drop completed.'