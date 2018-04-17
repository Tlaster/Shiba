grammar Shiba;

root
   : obj
   ;

obj
   : TOKEN '{' pair (',' pair)* '}'
   ;

pair
   : TOKEN ':' value
   | obj
   ;

value
   : STRING
   | NUMBER
   | BOOLEAN
   | 'null'
   ;

STRING
   : '"' (~'"')* '"'
   ;

BOOLEAN
   : 'true'
   | 'false'
   ;

TOKEN
   : ([a-z] | [A-Z] | '_')+ ([a-z] | [A-Z] | [0-9] | '_')*
   ;

NUMBER
   : '-'? INT ('.' [0-9] +)? EXP?
   ;


fragment INT
   : '0' | [1-9] [0-9]*
   ;

// no leading zeros

fragment EXP
   : [Ee] [+\-]? INT
   ;

Hws: [ \t] -> skip;
Vws: [\r\n\f] -> skip;
DocComment: '/**' .*? ('*/' | EOF) -> skip;
BlockComment: '/*'  .*? ('*/' | EOF) -> skip;
LineComment: '//' ~[\r\n]* -> skip;
LineCommentExt: '//' ~'\n'* ( '\n' Hws* '//' ~'\n'* )* -> skip;