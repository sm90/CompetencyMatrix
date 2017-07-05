[cmdletbinding(SupportsShouldProcess=$true)]
param($publishProperties=@{}, $packOutput, $pubProfilePath)

# to learn more about this file visit https://go.microsoft.com/fwlink/?LinkId=524327

try{
    if ($publishProperties['ProjectGuid'] -eq $null){
        $publishProperties['ProjectGuid'] = 'c586c8a6-f669-4cfc-871a-0f2ab9bdb1eb'
    }

    $publishModulePath = Join-Path (Split-Path $MyInvocation.MyCommand.Path) 'publish-module.psm1'
    Import-Module $publishModulePath -DisableNameChecking -Force

    $publishUtilitiesPath = Join-Path (Split-Path $MyInvocation.MyCommand.Path) 'Modules\PublishUtilities.psm1'
    Import-Module $publishUtilitiesPath -DisableNameChecking -Force

	EnvironmentToWebConfig -publishProperties $publishProperties -packOutput $packOutput 


	$publishProperties['AuthType'] = "NTLM"
	
    # call Publish-AspNet to perform the publish operation
    Publish-AspNet -publishProperties $publishProperties -packOutput $packOutput -pubProfilePath $pubProfilePath
}
catch{
    "An error occurred during publish.`n{0}" -f $_.Exception.Message | Write-Error
}