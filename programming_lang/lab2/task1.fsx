open System

// Функция для проверки, является ли число чётным
let isEven number = number % 2 = 0

// Функция для преобразования списка чисел в список true/false
let mapToEvenCheck numbers = List.map isEven numbers

// Функция для чтения списка чисел с клавиатуры
let readNumbersFromInput () =
    printfn "Введите список чисел через пробел:"
    let input = Console.ReadLine()
    match input.Trim().Split([|' '|], StringSplitOptions.RemoveEmptyEntries) |> Array.map (fun s -> s.Trim()) with
    | [||] -> 
        printfn "Ошибка: ввод не может быть пустым. Пожалуйста, попробуйте снова."
        None
    | parts ->
        try
            let numbers = parts |> Array.map int |> Array.toList
            Some numbers
        with
        | :? FormatException -> 
            printfn "Ошибка: введены некорректные данные. Пожалуйста, вводите только числа."
            None

// Основная логика программы
let main () =
    printfn "Программа проверяет чётность чисел в списке."
    let numbersOption = readNumbersFromInput ()
    
    match numbersOption with
    | Some numbers ->
        let result = mapToEvenCheck numbers
        printfn "Результат проверки чётности:"
        printfn "Исходный список: %A" numbers
        printfn "Чётность элементов: %A" result
    | None -> 
        printfn "Не удалось обработать ввод. Попробуйте снова."

// Запуск программы
main ()
