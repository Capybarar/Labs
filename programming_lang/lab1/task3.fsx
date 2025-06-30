open System


type Complex = { real: float; imag: float }


let add a b = 
    { real = a.real + b.real; imag = a.imag + b.imag }

let subtract a b = 
    { real = a.real - b.real; imag = a.imag - b.imag }

let multiply a b =
    { real = a.real * b.real - a.imag * b.imag
      imag = a.real * b.imag + a.imag * b.real }

let divide a b =
    let denominator = b.real * b.real + b.imag * b.imag
    { real = (a.real * b.real + a.imag * b.imag) / denominator
      imag = (a.imag * b.real - a.real * b.imag) / denominator }


let rec power (z: Complex) (n: int) =
    let rec fastPow acc current = function
        | 0 -> acc
        | k when k % 2 = 1 -> fastPow (multiply acc current) (multiply current current) (k/2)
        | k -> fastPow acc (multiply current current) (k/2)
    
    if n < 0 then
        let denominator = power z (-n)
        { real = denominator.real / (denominator.real**2 + denominator.imag**2)
          imag = -denominator.imag / (denominator.real**2 + denominator.imag**2) }
    else
        fastPow { real = 1.0; imag = 0.0 } z n


let inputComplex prompt =
    printfn "%s" prompt
    printf "  Действительная часть: "
    let re = Console.ReadLine() |> float
    printf "  Мнимая часть: "
    let im = Console.ReadLine() |> float
    { real = re; imag = im }


let inputInt prompt =
    printf "%s" prompt
    Console.ReadLine() |> int


let printComplex c =
    let format x = sprintf "%.2f" x
    match c.imag with
    | 0.0 -> printfn "%s" (format c.real)
    | im when im > 0.0 -> printfn "%s + %si" (format c.real) (format im)
    | im -> printfn "%s - %si" (format c.real) (format (abs im))


let main() =
    printfn "КАЛЬКУЛЯТОР КОМПЛЕКСНЫХ ЧИСЕЛ"
    
    let num1 = inputComplex "Введите первое комплексное число:"
    let num2 = inputComplex "Введите второе комплексное число:"
    let exponent = inputInt "Введите степень для возведения (целое число): "
    
    printfn "\nРЕЗУЛЬТАТЫ:"
    printf "1. Сумма: "; add num1 num2 |> printComplex
    printf "2. Разность: "; subtract num1 num2 |> printComplex
    printf "3. Произведение: "; multiply num1 num2 |> printComplex
    printf "4. Частное: "; divide num1 num2 |> printComplex
    printf "5. Первое число в степени %d: " exponent; power num1 exponent |> printComplex
    printf "6. Второе число в степени %d: " exponent; power num2 exponent |> printComplex


main()