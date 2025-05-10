$rootPath = "."
$indent = "  "

function Show-Directory($path, $level) {
    $items = Get-ChildItem -Path $path -Directory
    foreach ($item in $items) {
        $indentation = $indent * $level
        Write-Output "$indentation$($item.Name)"
        Show-Directory -path "$path\$($item.Name)" -level ($level + 1)
    }
}

Write-Output "Folder Structure:"
Show-Directory -path $rootPath -level 0
