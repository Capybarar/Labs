start :-
    write('Программа вычисляет отношение трёхзначного числа к сумме его цифр.\n'),
    write('Введите трёхзначное число: '),
    read(Number),
    sum_of_digits(Number, Sum),
    Result is Number / Sum,
    format('Результат: ~w / ~w = ~2f~n', [Number, Sum, Result]).

sum_of_digits(Number, Sum) :-
    number_chars(Number, Chars),
    maplist(char_to_digit, Chars, Digits),
    sum_list(Digits, Sum).

char_to_digit(Char, Digit) :-
    char_code(Char, Code),
    Digit is Code - 48.
