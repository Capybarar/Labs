open System


let isEven number = number % 2 = 0


let readNumbers () =
    printfn "Введите числа через пробел (например: 1 2 3 4 5):"
    Console.ReadLine().Split([|' '|], StringSplitOptions.RemoveEmptyEntries)
    |> Seq.map (fun s -> 
        match Int32.TryParse(s) with
        | (true, num) -> Some num
        | _ -> None)
    |> Seq.choose id  


let main () =
    printfn "Программа определяет чётность чисел (true - чётное, false - нечётное)"
    
    let numbers = readNumbers()
    let result = numbers |> Seq.map isEven |> Seq.toList
    
    printfn "Введенные числа: %A" (Seq.toList numbers)
    printfn "Результат проверки чётности: %A" result


main ()
