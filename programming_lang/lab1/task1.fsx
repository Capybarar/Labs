open System


let toZeroOne number =
    if number % 2 = 0 then 1 else 0


let readNumbers () =
    printfn "Введите числа через пробел:"
    let input = Console.ReadLine()
    match input.Trim().Split([|' '|], StringSplitOptions.RemoveEmptyEntries) with
    | [||] -> 
        printfn "Ошибка: ввод не может быть пустым. Попробуйте снова."
        None
    | parts ->
        try
            Some (Array.map int parts |> Array.toList)
        with
        | :? FormatException ->
            printfn "Ошибка: введены некорректные данные. Вводите только целые числа."
            None


let main () =
    printfn "Программа преобразует числа в 0 (нечетные) и 1 (четные)"
    
    match readNumbers() with
    | Some numbers ->
        let result = List.map toZeroOne numbers
        printfn "Введенные числа: %A" numbers
        printfn "Результат преобразования: %A" result
    | None -> 
        printfn "Не удалось обработать ввод. Попробуйте снова."


main ()
