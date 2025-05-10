$rootPath = "."
$indent = "  "

function Show-Files($path, $level) {
    $indentation = $indent * $level
    
    # Get directories
    $dirs = Get-ChildItem -Path $path -Directory | Sort-Object Name
    
    # Get files in current directory
    $files = Get-ChildItem -Path $path -File | Sort-Object Name
    
    # Print directory name
    if ($level -gt 0) {
        Write-Output "$indentation$((Get-Item $path).Name)/"
    } else {
        Write-Output "$path/"
    }
    
    # Print files
    foreach ($file in $files) {
        Write-Output "$indentation$indent$($file.Name)"
    }
    
    # Process subdirectories
    foreach ($dir in $dirs) {
        if ($dir.Name -ne "bin" -and $dir.Name -ne "obj") {
            Show-Files -path "$path\$($dir.Name)" -level ($level + 1)
        }
    }
}

Write-Output "Files Structure (excluding bin and obj directories):"
Show-Files -path $rootPath -level 0
