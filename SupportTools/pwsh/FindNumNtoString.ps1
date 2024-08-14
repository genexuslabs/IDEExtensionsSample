<#
.SYNOPSIS
    Processes COBOL files in a specified folder and extracts info about detected
    conversions from a numeric variable of a given length to an string

.DESCRIPTION
    This script searches for files in a specified folder that match a pattern used to
    convert a numeric variable of a given length to a string.
    The output are lines specifying the file on which the pattern was found, the name
    of the COBOL var being converted, and a guess of the name of the related GeneXus
    variable or attribute.

.PARAMETER FolderPath
    The path to the folder containing the files to be processed. This parameter is mandatory.

.PARAMETER FilePattern
    The pattern to match files in the folder. The default pattern is "*.cob".

.PARAMETER NumLength
    An integer parameter that specifies the numeric length of variables or attributes
    for which to search their conversion to a string.

.EXAMPLE
    .\FindNumNtoString.ps1 -FolderPath "C:\MyFolder" -FilePattern "*.cob" -NumLength 7
    This command processes all .cob files in the specified folder and searches for the pattern "MOVE 7 TO GX-STR-LEN".

.NOTES
    Author: Your Name
    Date: Today's Date
#>
param (
    [Parameter(Mandatory=$true)]
    [string]$FolderPath,

    [string]$FilePattern = "*.cob",

    [Parameter(Mandatory=$true)]
    [int]$NumLength
)

$searchPattern = "MOVE $NumLength TO GX-STR-LEN"

# Check if the folder path exists
if (-Not (Test-Path -Path $FolderPath)) {
    Write-Error "The specified folder path does not exist: $FolderPath"
    exit 1
}

# Get all files matching the file pattern in the folder
$files = Get-ChildItem -Path $FolderPath -Filter $FilePattern

# Iterate over each file
foreach ($file in $files) {
    # Read all lines from the file
    $lines = Get-Content -Path $file.FullName
    
    # Iterate over the lines to find the match
    for ($i = 1; $i -lt $lines.Length; $i++) {
        if ($lines[$i].Trim() -eq $searchPattern) {
            # Print the file name and the previous line, trimmed of leading and trailing spaces
            $previousLine = $lines[$i - 1].Trim()   
            # if previousLine doesnt start with "MOVE " take also the previous line
            if ($previousLine -notlike "MOVE *") {
                $previousLine2 = $lines[$i - 2].Trim()
                $previousLine = $previousLine2 + " " + $previousLine
            }

            # isolate COBOL variable name
            $cobolVar = $previousLine -replace "^MOVE ", "" -replace " TO GX-STR-NUM$", ""

            # guess GX variable or attribute name
            $gxVarOrAtt = $cobolVar -replace "^GXV-", "" -replace "^V\d+", "&"

            # or the use of a function
            if ($gxVarOrAtt -eq "GX-YEAR-YY")
            {
				$gxVarOrAtt = "Result of year(...)"
			}
            # check for the "GXINT-*" case
            elseif ($gxVarOrAtt -like "GXINT-*")
			{
                $gxVarOrAtt = "Result of int(...)"
            }


            Write-Output "$($file.Name),$cobolVar,$gxVarOrAtt"
        }
    }
}
