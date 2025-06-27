start :-
    write('Введите множество A: '), read(A),
    write('Введите множество B: '), read(B),
    write('Введите множество C: '), read(C),

  
    list_to_set(A, SetA),
    list_to_set(B, SetB),
    list_to_set(C, SetC),

    format('\nМножества после обработки:\n'),
    format('A = ~w\nB = ~w\nC = ~w~n~n', [SetA, SetB, SetC]),

    
    union_set(SetB, SetC, UnionBC),
    intersection_set(SetA, UnionBC, LeftResult),

    
    intersection_set(SetA, SetB, InterAB),
    intersection_set(SetA, SetC, InterAC),
    union_set(InterAB, InterAC, RightResult),

    
    format('Левая часть A ∩ (B ∪ C) = ~w~n', [LeftResult]),
    format('Правая часть (A ∩ B) ∪ (A ∩ C) = ~w~n', [RightResult]),

    ( LeftResult == RightResult ->
        write('✅ Второй дистрибутивный закон выполнен.\n')
    ;   write('❌ Второй дистрибутивный закон НЕ выполнен.\n')
    ).

list_to_set(List, Set) :-
    is_list(List), !,
    sort(List, Set).


union_set(A, B, Union) :-
    ord_union(A, B, Union).


intersection_set(A, B, Intersect) :-
    ord_intersection(A, B, Intersect).
