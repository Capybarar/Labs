program test;
var
  x, y: integer;
  r: record
    a: integer;
    b: real;
  end;
begin
  x := 10;
  r.a := x + 5;
  r.b := 3.14
end.