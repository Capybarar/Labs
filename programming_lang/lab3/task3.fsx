open System
open System.IO

// Функция для подсчета файлов, начинающихся с заданного символа
let countFilesByFirstLetter (directory: string) (letter: char) =
    try
        Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories)
        |> Seq.filter (fun file -> 
            let fileName = Path.GetFileName(file)
            fileName.Length > 0 && Char.ToUpper(fileName.[0]) = Char.ToUpper(letter))
        |> Seq.length
    with
    | :? UnauthorizedAccessException -> 
        printfn "Нет доступа к некоторым каталогам"
        -1
    | :? DirectoryNotFoundException ->
        printfn "Каталог не найден"
        -1

// Основная функция
let main () =
    printf "Введите путь к каталогу (Без кириллицы): "
    let path = Console.ReadLine()
    
    printf "Введите символ для поиска: "
    let charInput = Console.ReadKey().KeyChar
    printfn "\n" // Переход на новую строку после ввода символа
    
    let count = countFilesByFirstLetter path charInput
    
    if count >= 0 then
        printfn "Количество файлов, начинающихся на '%c': %d" charInput count
    else
        printfn "Не удалось завершить подсчет."

main()
