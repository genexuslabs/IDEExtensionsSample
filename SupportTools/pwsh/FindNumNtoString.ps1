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

            # on $previousLine remove leading "MOVE " and trailing " TO GX-STR-NUM"
            $previousLine = $previousLine -replace "^MOVE ", ""
            $previousLine = $previousLine -replace " TO GX-STR-NUM$", ""

            Write-Output "$($file.Name),$previousLine"
        }
    }
}
