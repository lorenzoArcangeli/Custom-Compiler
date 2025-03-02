%output=Generated/Parser.cs
%namespace Parser


%union {
public string Value;
public Expression Expression;
public Statement Statement;
public Type Type;
public Program Program;
public FunctionDeclaration FunctionDeclaration;
public VariableDeclaration VariableDeclaration;
public List<VariableDeclaration> VariableDeclarationList;
public List<Statement> StatementList;
public List<Expression> ExpressionList;
public List<FunctionDeclaration> FunctionalDeclarationList; 
}


%token <Value> INTVALUE
%token <Value> BOOLVALUE
%token <Value> ID
%token <Value> ERR

%token COMMA ","
%token SEMI ";"
%token LPAR "("
%token RPAR ")"
%token LBRA "{"
%token RBRA "}"

%token ASN "="
%token OR "||"
%token AND "&&"
%token EQ "=="
%token NEQ "!="
%token LES "<"
%token GRE ">"
%token LESE "<="
%token GREE ">="
%token ADD "+"
%token SUB "-"
%token MUL "*"
%token DIV "/"
%token NEG "!"

%token VOID "void"
%token INT "int"
%token BOOL "bool"

%token IF "if"
%token ELSE "else"
%token WHILE "while"
%token RETURN "return"

%type <FunctionalDeclarationList> DeclList
%type <FunctionDeclaration> Decl
%type <Type> Type
%type <Statement> Stmt
%type <Expression> Expr, Expr1, Expr2, Expr3, Expr4, Expr5, Expr6, Expr7, Expr8, Expr9
%type <VariableDeclarationList> FormalRest, FormalList
%type <StatementList> StmtList
%type <ExpressionList> ExprList, ExprRest
%type <Progam> Program

%%
Program : DeclList EOF {Program = new Program($1);}
       ;

DeclList : DeclList Decl { $1.Add($2); $$=$1;}
       |  { $$ = new List<FunctionDeclaration>(); }
       ;

Decl : Type ID "(" FormalList ")" "{" StmtList "}" { $$ = new FunctionDeclaration($1, $2, $4, $7); $$.SetLocation(@$);}
       | "void" ID "(" FormalList ")" "{" StmtList "}" { $$ = new FunctionDeclaration(Type.VOID, $2, $4, $7); $$.SetLocation(@$);}
       ;

FormalList : Type ID FormalRest { $3.Add(new VariableDeclaration($1, $2)); $3.Reverse(); $$=$3;}
       |  { $$ = new List<VariableDeclaration>(); }
       ;
                                    
FormalRest : "," Type ID FormalRest { $4.Add(new VariableDeclaration($2, $3)); $$=$4; }
       |  { $$ = new List<VariableDeclaration>(); }
       ;

Type : "int" { $$ = Type.INT; }
       | "bool" { $$ = Type.BOOL;}
       ;

StmtList : StmtList Stmt { $1.Add($2); $$=$1;}
       |  { $$ = new List<Statement>(); }
       ;

Stmt : "{" StmtList "}" { $$ = new BlockStatement($2); $$.SetLocation(@$);}
      |  "if" "(" Expr ")" Stmt { $$ = new IfStatement($3, $5, null);$$.SetLocation(@$); }
      |  "if" "(" Expr ")" Stmt "else" Stmt  { $$ = new IfStatement($3, $5, $7);$$.SetLocation(@$); }
      |  "while" "(" Expr ")" Stmt { $$ = new WhileStatement($3, $5);$$.SetLocation(@$); }
      |  "return" Expr ";" { $$ = new ReturnStatement($2);$$.SetLocation(@$); }
      |  "return" ";" { $$ = new ReturnStatement(null);$$.SetLocation(@$); }
      |  Expr ";" { $$ = new ExpressionStatement($1);$$.SetLocation(@$); }
      |  Type ID ";" { $$ = new VariableDeclarationStatement($1, $2);$$.SetLocation(@$); }
      ;


Expr : ID "=" Expr { $$ = new AssignmentExpression($1, $3);$$.SetLocation(@$); }
      |  Expr1 {$$ = $1;}
      ;

Expr1 : Expr1 "||" Expr2 { $$ = new BinaryOperatorExpression(BinaryOperatorExpression.OperatorType.OR, $1, $3);$$.SetLocation(@$); }
      |  Expr2 {$$ = $1;}
      ;

Expr2 : Expr2 "&&" Expr3 { $$ = new BinaryOperatorExpression(BinaryOperatorExpression.OperatorType.AND, $1, $3); $$.SetLocation(@$);}
      |  Expr3 {$$ = $1;}
      ;

Expr3 : Expr3 "==" Expr4 { $$ = new BinaryOperatorExpression(BinaryOperatorExpression.OperatorType.EQ, $1, $3);$$.SetLocation(@$); }
      |  Expr3 "!=" Expr4 { $$ = new BinaryOperatorExpression(BinaryOperatorExpression.OperatorType.NEQ, $1, $3);$$.SetLocation(@$); }
      |  Expr4 {$$ = $1;}
      ;

Expr4 : Expr4 "<" Expr5 { $$ = new BinaryOperatorExpression(BinaryOperatorExpression.OperatorType.LES, $1, $3); $$.SetLocation(@$);}
      |  Expr4 ">" Expr5 { $$ = new BinaryOperatorExpression(BinaryOperatorExpression.OperatorType.GRE, $1, $3); $$.SetLocation(@$);}
      |  Expr4 "<=" Expr5 { $$ = new BinaryOperatorExpression(BinaryOperatorExpression.OperatorType.LESE, $1, $3); $$.SetLocation(@$);}
      |  Expr4 ">=" Expr5 { $$ = new BinaryOperatorExpression(BinaryOperatorExpression.OperatorType.GREE, $1, $3); $$.SetLocation(@$);}
      |  Expr5 {$$ = $1;}
      ;

Expr5 : Expr5 "+" Expr6 { $$ = new BinaryOperatorExpression(BinaryOperatorExpression.OperatorType.ADD, $1, $3); $$.SetLocation(@$);}
      |  Expr5 "-" Expr6 { $$ = new BinaryOperatorExpression(BinaryOperatorExpression.OperatorType.SUB, $1, $3); $$.SetLocation(@$);}
      |  Expr6 {$$ = $1;}
      ;

Expr6 : Expr6 "*" Expr7 { $$ = new BinaryOperatorExpression(BinaryOperatorExpression.OperatorType.MUL, $1, $3); $$.SetLocation(@$);}
      |  Expr6 "/" Expr7 { $$ = new BinaryOperatorExpression(BinaryOperatorExpression.OperatorType.DIV, $1, $3);$$.SetLocation(@$); }
      |  Expr7 {$$ = $1;}
      ;

Expr7 : "!" Expr7 { $$ = new UnaryOperatorExpression(UnaryOperatorExpression.OperatorType.NEG, $2); $$.SetLocation(@$);}
      |  "-" Expr7 { $$ = new UnaryOperatorExpression(UnaryOperatorExpression.OperatorType.NOT, $2); $$.SetLocation(@$);}
      |  Expr8 {$$ = $1;}
      ;

Expr8 : INTVALUE { $$ = new IntegerLiteralExpression($1); $$.SetLocation(@$);}
      | BOOLVALUE { $$ = new BooleanLiteralExpression($1); $$.SetLocation(@$);}
      | ID { $$ = new VariableExpression($1); $$.SetLocation(@$);}
      | Expr9 {$$=$1; $$.SetLocation(@$);}
      ;

Expr9 : ID "(" ExprList ")" { $$ = new FunctionCallExpression($1, $3); $$.SetLocation(@$);}
      | "(" Expr ")" { $$= $2; $$.SetLocation(@$);}
      ;

ExprList : Expr ExprRest {$2.Add($1); $2.Reverse(); $$=$2;}
       |  { $$ = new List<Expression>(); }
       ; 

ExprRest : "," Expr ExprRest {$3.Add($2); $$=$3;}
       |  { $$ = new List<Expression>(); } 
       ;


%%
public Program Program;
public Parser( Scanner s ) : base( s ) { }
