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
        printfn "Ошибка: некорректная римская цифра '%s' (допустимы I-IX). Число будет пропущено." roman
        0  


let readRomanSequence () =
    printfn "Введите римские числа (I-IX) через пробел:"
    Console.ReadLine().Split([|' '|], StringSplitOptions.RemoveEmptyEntries)
    |> Seq.map (fun s -> s.Trim().ToUpper())


let main () =
    printfn "Программа вычисляет сумму римских чисел (I-IX)"
    
    let romanNumbers = readRomanSequence()
    
    
    let sum = 
        romanNumbers
        |> Seq.map romanToDecimal
        |> Seq.fold (fun acc num -> acc + num) 0
    
    printfn "Введенные числа: %A" (Seq.toList romanNumbers)
    printfn "Десятичные значения: %A" (Seq.map romanToDecimal romanNumbers |> Seq.toList)
    printfn "Сумма: %d" sum


main ()