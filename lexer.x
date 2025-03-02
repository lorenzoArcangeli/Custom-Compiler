%namespace Parser
%using QUT.Gppg; 
%option out:Generated/Lexer.cs

alpha [a-zA-Z_]
digit [0-9_]
alphanum {alpha}|{digit}
booleanLiterals (true|false)

%%

" "|"\n"|"\r"   { }
\/\/[^\r\n]*[\r\n]   { }

"," {return (int) Tokens.COMMA;}
";" {return (int) Tokens.SEMI;}
"(" {return (int) Tokens.LPAR;}
")" {return (int) Tokens.RPAR;}
"{" {return (int) Tokens.LBRA;}
"}" {return (int) Tokens.RBRA;}

"=" {return (int) Tokens.ASN;}
"||" {return (int) Tokens.OR;}
"&&" {return (int) Tokens.AND;}
"==" {return (int) Tokens.EQ;}
"!=" {return (int) Tokens.NEQ;}
"<" {return (int) Tokens.LES;}
">" {return (int) Tokens.GRE;}
"<=" {return (int) Tokens.LESE;}
">=" {return (int) Tokens.GREE;}
"+" {return (int) Tokens.ADD;}
"-" {return (int) Tokens.SUB;}
"*" {return (int) Tokens.MUL;}
"/" {return (int) Tokens.DIV;}
"!" {return (int) Tokens.NEG;}

"void" {return (int) Tokens.VOID;}
"int" {return (int) Tokens.INT;}
"bool" {return (int) Tokens.BOOL;}

"if" {return (int) Tokens.IF;}
"else" {return (int) Tokens.ELSE;}
"while" {return (int) Tokens.WHILE;}
"return" {return (int) Tokens.RETURN;}



{digit}+ {
    yylval.Value = yytext;
    return (int) Tokens.INTVALUE;
}

{booleanLiterals} {
    yylval.Value = yytext;
    return (int) Tokens.BOOLVALUE;
}

{alpha}{alphanum}* {
    yylval.Value = yytext;
    return (int) Tokens.ID;
}

. {
       yylval.Value = yytext;
        return (int) Tokens.ERR;

    }


%{
yylloc = new LexLocation(tokLin, tokCol, tokELin, tokECol);
%}

%%

override public void yyerror(string msg, object[] args) 
{ 
	Console.WriteLine("{0} on line {1} column {2}, Offending token {3}.", 
			 msg, yylloc.StartLine, yylloc.StartColumn, yylval.Value); 
}

