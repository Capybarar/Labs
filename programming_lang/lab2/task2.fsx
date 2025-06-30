open System

let romanToDecimal roman =
    match roman with
    | "I" -> 1
    | "II" -> 2
    | "III" -> 3
    | "IV" -> 4
    | "V" -> 5
    | "VI" -> 6
    | "VII" -> 7
    | "VIII" -> 8
    | "IX" -> 9
    | _ -> 
        printfn "Ошибка: некорректная римская цифра '%s'. Допустимы значения от I до IX." roman
        -1 // Возвращаем -1 в случае ошибки

let readRomanNumbers () =
    printfn "Введите римские числа от I до IX через пробел:"
    let input = Console.ReadLine()
    match input.Trim().Split([|' '|], StringSplitOptions.RemoveEmptyEntries) with
    | [||] -> 
        printfn "Ошибка: ввод не может быть пустым. Попробуйте снова."
        None
    | parts -> 
        Some (Array.toList parts)

let main () =
    printfn "Программа вычисляет сумму римских чисел (I-IX) в десятичной системе."
    let romanNumbersOption = readRomanNumbers ()
    
    match romanNumbersOption with
    | Some romanNumbers ->
        let decimalNumbers = List.map romanToDecimal romanNumbers
        
        // Проверяем, есть ли некорректные значения (-1)
        if List.contains -1 decimalNumbers then
            printfn "Обнаружены некорректные римские числа. Пожалуйста, вводите только от I до IX."
        else
            // Вычисляем сумму с помощью List.fold
            let sum = 
                decimalNumbers 
                |> List.fold (fun acc num -> acc + num) 0
            
            printfn "Римские числа: %A" romanNumbers
            printfn "Десятичные значения: %A" decimalNumbers
            printfn "Сумма: %d" sum
    | None -> 
        printfn "Не удалось обработать ввод. Попробуйте снова."

main ()