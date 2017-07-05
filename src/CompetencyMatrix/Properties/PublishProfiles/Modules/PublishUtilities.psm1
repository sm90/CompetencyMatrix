#
# PublishUtilities.psm1
#
function EnvironmentToWebConfig {
    [cmdletbinding()]
    param(
        [Parameter(Position=0)]
        [HashTable]$publishProperties,
        [Parameter(Position=1)]
        [System.IO.FileInfo]$packOutput
    )
    process {
        try {
            [Reflection.Assembly]::LoadWithPartialName("System.Xml.Linq") | Out-Null
            $webConfigPath = Join-Path $packOutput 'web.config'
            $envName = $publishProperties['EnvironmentName']

            [System.Xml.Linq.XDocument]$xDoc = [System.Xml.Linq.XDocument]::Load($webConfigPath)

            $allNodes = $xDoc.DescendantNodes()

			$aspNetCoreNode = $allNodes | Where-Object { $_.NodeType -eq [System.Xml.XmlNodeType]::Element -and $_.Name -eq 'aspNetCore' } | Select -First 1
			$envVariablesNode = $aspNetCoreNode.Element('environmentVariables')

			$envVarENVIRONMENT = $envVariablesNode.Element('environmentVariable')
			
			if($envVarENVIRONMENT -eq $null)
			{
				$xname = [System.Xml.Linq.XName]::Get('environmentVariable')
				$envVarENVIRONMENT = New-Object -TypeName System.Xml.Linq.XElement -ArgumentList $xname
				$envVariablesNode.Add($envVarENVIRONMENT)
			}

			$envVarENVIRONMENT.SetAttributeValue('name', 'ASPNETCORE_ENVIRONMENT')
			$envVarENVIRONMENT.SetAttributeValue('value', $envName)
			$xDoc.Save($webConfigPath) | Out-Null
        }
        catch {
			throw
        }
    }
}
