open System


type Tree =
    | Leaf
    | Node of int * Tree * Tree  

let rec foldTree nodeFunc leafVal tree =
    match tree with
    | Leaf -> leafVal
    | Node(value, left, right) ->
        nodeFunc value (foldTree nodeFunc leafVal left) (foldTree nodeFunc leafVal right)


let containsDigit digit number =
    let rec check n =
        if n = 0 then false
        else (n % 10 = digit) || check (n / 10)
    check (abs number)  


let countDigits digit tree =
    foldTree (fun v l r -> 
        (if containsDigit digit v then 1 else 0) + l + r) 0 tree


let rec readTree () =
    printfn "Введите узел (число) или 'L' для листа:"
    match Console.ReadLine().Trim().ToUpper() with
    | "L" -> Leaf
    | input ->
        match Int32.TryParse input with
        | true, value ->
            printfn "Левое поддерево для %d:" value
            let left = readTree()
            printfn "Правое поддерево для %d:" value
            let right = readTree()
            Node(value, left, right)
        | _ -> 
            printfn "Ошибка: введите целое число или 'L'"
            readTree()


let printTree tree =
    let rec print indent tree =
        match tree with
        | Leaf -> printfn "%s└── Лист" indent
        | Node(v, l, r) ->
            printfn "%s└── %d" indent v
            print (indent + "    ") l
            print (indent + "    ") r
    print "" tree


let main () =
    printfn "Программа подсчитывает, сколько узлов дерева содержат заданную цифру"
    
    printfn "Построение дерева:"
    let tree = readTree()
    
    printfn "\nВведенное дерево:"
    printTree tree
    
    printf "\nВведите цифру для поиска (0-9): "
    let digit = Console.ReadLine() |> Int32.Parse
    
    let count = countDigits digit tree
    printfn "\nКоличество узлов, содержащих цифру %d: %d" digit count


main ()
