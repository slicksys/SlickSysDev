[CmdletBinding()]
param(
    [string]$Server = '(localdb)\ProjectModels',
    [string]$Database = 'managementdata_e2e',
    [switch]$SkipBuild
)

$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $PSScriptRoot
$sqlProjPath = Join-Path $repoRoot 'ManagementData.sqlproj'
$postDeployPath = Join-Path $repoRoot 'Script.PostDeployment.sql'
$smokePath = Join-Path $PSScriptRoot 'smoke_test_procedures.sql'

function Invoke-SqlFile {
    param(
        [Parameter(Mandatory = $true)]
        [string]$ServerName,
        [Parameter(Mandatory = $true)]
        [string]$DatabaseName,
        [Parameter(Mandatory = $true)]
        [string]$FilePath
    )

    Write-Host "Running: $FilePath"
    sqlcmd -S $ServerName -d $DatabaseName -I -b -i $FilePath | Out-Host
    if ($LASTEXITCODE -ne 0) {
        throw "sqlcmd failed for file: $FilePath"
    }
}

if (-not $SkipBuild) {
    Write-Host 'Building SQL project...'
    dotnet build $sqlProjPath | Out-Host
    if ($LASTEXITCODE -ne 0) {
        throw 'dotnet build failed.'
    }
}

Write-Host "Recreating database [$Database] on [$Server]..."
$dropCreate = @"
if db_id(N'$Database') is not null
begin
    alter database [$Database] set single_user with rollback immediate;
  
end;
create database [$Database];
"@
sqlcmd -S $Server -d master -b -Q $dropCreate | Out-Host
if ($LASTEXITCODE -ne 0) {
    throw 'failed to recreate database.'
}

$tableDir = Join-Path $repoRoot 'dbo\tables'
$orderedTableFiles = @(
    'practice.sql',
    'client.sql',
    'aspnet_roles.sql',
    'aspnet_users.sql',
    'aspnet_role_claims.sql',
    'aspnet_user_claims.sql',
    'aspnet_user_logins.sql',
    'aspnet_user_roles.sql',
    'aspnet_user_tokens.sql',
    'user_practice_role.sql',
    'user_practice_invitation.sql',
    'principal_context_label.sql',
    'resource_type.sql',
    'appointment_status.sql',
    'reservation_status.sql',
    'invoice_status_lookup.sql',
    'payment_method.sql',
    'principal.sql',
    'resource.sql',
    'invoice.sql',
    'appointment.sql',
    'reservation.sql',
    'payment.sql'
)

foreach ($tableFile in $orderedTableFiles) {
    Invoke-SqlFile -ServerName $Server -DatabaseName $Database -FilePath (Join-Path $tableDir $tableFile)
}

$deployStages = @(
    (Join-Path $repoRoot 'dbo\views'),
    (Join-Path $repoRoot 'dbo\stored_procedures'),
    (Join-Path $repoRoot 'dbo\indexes'),
    (Join-Path $repoRoot 'dbo\foreign_keys')
)

foreach ($stage in $deployStages) {
    if (Test-Path $stage) {
        $files = Get-ChildItem -Path $stage -Filter '*.sql' -File | Sort-Object Name
        foreach ($file in $files) {
            Invoke-SqlFile -ServerName $Server -DatabaseName $Database -FilePath $file.FullName
        }
    }
}

Invoke-SqlFile -ServerName $Server -DatabaseName $Database -FilePath $postDeployPath
Invoke-SqlFile -ServerName $Server -DatabaseName $Database -FilePath $smokePath

Write-Host 'E2E deployment + smoke tests completed successfully.'