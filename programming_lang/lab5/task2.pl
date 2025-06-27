
start :-
    write('Введите список чисел: '),
    read(List),
    ( is_list(List) ->
        count_non_zero(List, Count),
        format('Количество ненулевых элементов: ~w~n', [Count])
    ;   write('Ошибка: введённое значение не является списком.')
    ).

count_non_zero([], 0).

count_non_zero([H|T], Count) :-
    ( H =\= 0 ->
        count_non_zero(T, TailCount),
        Count is TailCount + 1
    ;   count_non_zero(T, Count)
    ).
