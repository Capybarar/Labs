:- use_module(library(clpfd)).

start :-
    Kids = [misha, maksim, lenya, dima],
    Cats = [druzhok, elisey, fantik, murlyka],
    Colors = [ryzhiy, seryy, beliy, chernyy],

    % Переменные: каждому котёнку присваивается цвет
    length(CatColors, 4),
    CatColors ins 0..3, all_distinct(CatColors),

    % Перебираем все перестановки котят для мальчиков
    permutation(Cats, PermCats),

    % Связываем котят с цветами
    maplist(attach_color(CatColors), Cats, Colors, CatColorPairs),

    % Сопоставляем мальчики <-> котята <-> цвета
    pairlists(Kids, PermCats, KidCatPairs),

    % Добавляем цвета
    combine(KidCatPairs, CatColorPairs, Assignments),

    % Проверяем условия
    check_all_conditions(Assignments, BadCount),
    BadCount =:= 1,

    % Вывод результата
    write('РЕШЕНИЕ:\n'),
    forall(member(A, Assignments), print_assignment(A)).

% Связывает котёнка с цветом по индексу
attach_color(CatColors, Cat, Color, Cat-Color) :-
    cat_index(Cat, Index),
    nth0(Index, CatColors, ColorIndex),
    color_by_index(ColorIndex, Color).

% Устанавливаем соответствие между цветами и числами
color_by_index(0, ryzhiy).
color_by_index(1, seryy).
color_by_index(2, beliy).
color_by_index(3, chernyy).

% Назначаем индексы котятам
cat_index(druzhok, 0).
cat_index(elisey, 1).
cat_index(fantik, 2).
cat_index(murlyka, 3).

% Проверяем все условия
check_all_conditions(Assignments, BadCount) :-
    findall(false,
        ( condition(1, Assignments), fail ; true,
          condition(2, Assignments), fail ; true,
          condition(3, Assignments), fail ; true,
          condition(4, Assignments), fail ; true,
          condition(5, Assignments), fail ; true,
          condition(6, Assignments), fail ; true,
          condition(7, Assignments), fail ; true,
          condition(8, Assignments), fail ; true,
          condition(9, Assignments), fail ; true,
          condition(10, Assignments), fail ; true ),
        Falses),
    length(Falses, BadCount).

% Проверка отдельного условия
condition(1, Assignments) :- get_cat_color(fantik, C, Assignments), C == ryzhiy, !, fail.
condition(2, Assignments) :- get_cat_color(murlyka, C, Assignments), C == seryy, !, fail.
condition(3, Assignments) :- get_cat_color(druzhok, C, Assignments), C == beliy, !, fail.
condition(4, Assignments) :- get_cat_color(elisey, C, Assignments), C == seryy, !, fail.
condition(5, Assignments) :- get_kid_cat_color(misha, _, C, Assignments), C \== chernyy, !, fail.
condition(6, Assignments) :- get_kid_cat(maksim, C, Assignments), C \== murlyka, !, fail.
condition(7, Assignments) :- get_kid_cat(lenya, C, Assignments), C \== elisey, !, fail.
condition(8, Assignments) :- get_kid_cat_color(dim, _, C, Assignments), C \== beliy, !, fail.
condition(9, Assignments) :- get_kid_cat(dim, fantik, Assignments), !, fail.
condition(10, Assignments) :- get_cat_color(druzhok, C, Assignments), C == seryy, !, fail.

% Вспомогательные предикаты
get_kid_cat(Kid, Cat, Assignments) :- member(Kid-Cat-_, Assignments).
get_kid_cat_color(Kid, Cat, Color, Assignments) :- member(Kid-Cat-Color, Assignments).
get_cat_color(Cat, Color, Assignments) :- member(_-Cat-Color, Assignments).

% Парное объединение двух списков
pairlists([], [], []).
pairlists([H1|T1], [H2|T2], [H1-H2|T3]) :-
    pairlists(T1, T2, T3).

combine([], [], []).
combine([Kid-Cat|T1], [Cat-Color|T2], [Kid-Cat-Color|T3]) :-
    combine(T1, T2, T3).

print_assignment(Kid-Cat-Color) :-
    format('~w -> ~w (~w)~n', [Kid, Cat, Color]).
