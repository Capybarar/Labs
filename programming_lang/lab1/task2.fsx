open System


let rec getEvenDigits number =
    if number = 0 then
        []
    else
        let digit = number % 10
        let rest = number / 10
        let evenDigitsFromRest = getEvenDigits rest
        if digit % 2 = 0 then
            digit :: evenDigitsFromRest
        else
            evenDigitsFromRest


let readNumber () =
    printf "Введите целое число: "
    match Int32.TryParse(Console.ReadLine()) with
    | (true, num) -> Some num
    | _ -> 
        printfn "Ошибка: введено некорректное число."
        None


let main () =
    printfn "Программа извлекает четные цифры из числа"
    
    match readNumber() with
    | Some number ->
        let evenDigits = getEvenDigits (abs number) // Обрабатываем отрицательные числа
        printfn "Число: %d" number
        printfn "Четные цифры: %A" evenDigits
    | None -> 
        printfn "Не удалось обработать ввод. Попробуйте снова."


main ()