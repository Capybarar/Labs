open System


type Tree =
    | Leaf
    | Node of float * Tree * Tree


let rec mapTree f tree =
    match tree with
    | Leaf -> Leaf
    | Node(value, left, right) -> Node(f value, mapTree f left, mapTree f right)


let replaceNumber x =
    if x < 0.0 then 0.0 else 1.0


let rec readTree level =
    let indent = String(' ', level * 4)
    printfn "%sВведите узел (формат: значение) или 'L' для листа:" indent
    printf "%s> " indent
    let input = Console.ReadLine().Trim()
    
    match input.ToUpper() with
    | "L" -> Leaf
    | _ ->
        try
            let value = float input
            printfn "%sЛевое поддерево для %.2f:" indent value
            let left = readTree (level + 1)
            printfn "%sПравое поддерево для %.2f:" indent value
            let right = readTree (level + 1)
            Node(value, left, right)
        with
        | _ -> 
            printfn "%sНекорректный ввод, попробуйте снова" indent
            readTree level


let printTree tree =
    let rec print level tree =
        let indent = String(' ', level * 4)
        match tree with
        | Leaf -> printfn "%s└── Лист" indent
        | Node(v, l, r) ->
            printfn "%s└── %.2f" indent v
            print (level + 1) l
            print (level + 1) r
    
    printfn "\nДерево:"
    print 0 tree


let main () =
    printfn "Программа преобразует дерево вещественных чисел:"
    printfn "  отрицательные -> 0.0"
    printfn "  положительные -> 1.0"
    printfn "\nВведите дерево рекурсивно:"
    
    let tree = readTree 0
    
    printfn "\nИсходное дерево:"
    printTree tree
    
    let transformedTree = mapTree replaceNumber tree
    
    printfn "\nПреобразованное дерево:"
    printTree transformedTree


main ()
