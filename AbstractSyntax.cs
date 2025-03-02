
using Parser;
using System;
using System.Collections.Generic;
namespace Parser
{

    public interface Visitior<T>
    {
        T Visit(Expression expr);

        T Visit(Statement stmt, Type funReturnType);

        T Visit(Program program);
    }

    //GENERAL TYPE

    public enum Type
    {
        INT,
        BOOL,
        VOID,
        OK,
        ERR
    }

  /*  public abstract class ExprVisitor<T>
    {
        public abstract T visit(Expression exp);
    }*/


    //EXPRESSION

    public class Expression
    {
        public int Line;
        public int Column;
        public Type _tp;

        public void SetLocation(QUT.Gppg.LexLocation loc)
        {
            Line = loc.StartLine;
            Column = loc.StartColumn;
        }

        public Type Accept(Visitior<Type> visitor)
        {
            return visitor.Visit(this);
        }

        /* public T accept<T>(ExprVisitor<T> visitor)
         {
             return visitor.visit(this);
         }*/
    }

    public class AssignmentExpression : Expression
    {
        public string identifier;
        public Expression right;

        public AssignmentExpression(string target, Expression right)
        {
            this.identifier = target;
            this.right = right;
        }
        
    }

    public class BinaryOperatorExpression : Expression
    {
        public Expression left, right;
        public BinaryOperatorExpression.OperatorType op;

        public BinaryOperatorExpression(BinaryOperatorExpression.OperatorType o, Expression l, Expression r)
        {
            this.left = l;
            this.right = r;
            this.op = o;
        }

        public enum OperatorType
        {
            OR, AND, EQ, NEQ, GREE, LESE, GRE, LES, ADD, SUB, MUL, DIV
        };
    }


    public class BooleanLiteralExpression : Expression
    {
        public bool value;

        public BooleanLiteralExpression(string value)
        {
            this.value = Convert.ToBoolean(value);
        }
    }

    public class FunctionCallExpression : Expression
    {
        public string identifier;
        public List<Expression> arguments;

        public FunctionCallExpression(string target, List<Expression> arguments)
        {
            this.identifier = target;
            this.arguments = arguments;
        }

    }

    public class IntegerLiteralExpression : Expression
    {
        public int num;

        public IntegerLiteralExpression(string num)
        {
            this.num = Convert.ToInt32(num);
        }
    }

    public class UnaryOperatorExpression : Expression
    {
        public UnaryOperatorExpression.OperatorType type;
        public Expression expr;

        public UnaryOperatorExpression(UnaryOperatorExpression.OperatorType type, Expression expr)
        {
            this.type = type;
            this.expr = expr;
        }

        public enum OperatorType
        {
            NEG,
            NOT,
        }
    }

    public class VariableExpression : Expression
    {
        public string id;

        public VariableExpression(string id)
        {
            this.id = id;
        }
    }

    


    //STATEMENT

    public class Statement
    {

        public int Line;
        public int Column;
        public void SetLocation(QUT.Gppg.LexLocation loc)
        {
            Line = loc.StartLine;
            Column = loc.StartColumn;
        }
        public Type Accept(Visitior<Type> visitor, Type funReturnType)
        {
            return visitor.Visit(this, funReturnType);
        }
    }

    public class BlockStatement : Statement
    {
        public List<Statement> body;

        public BlockStatement(List<Statement> body)
        {
            this.body = body;
        }
    }

    public class ExpressionStatement : Statement
    {
        public Expression expr;

        public ExpressionStatement(Expression expr)
        {
            this.expr = expr;
        }
    }


    public class IfStatement : Statement
    {
        public Expression condition;
        public Statement prim;
        public Statement alternative;

        public IfStatement(Expression condition, Statement prim, Statement alternative)
        {
            this.condition = condition;
            this.prim = prim;
            this.alternative = alternative;
        }
    }

    public class ReturnStatement : Statement
    {
        public Expression expr;

        public ReturnStatement(Expression expr)
        {
            this.expr = expr;
        }
    }

    public class VariableDeclarationStatement : Statement
    {
        public string name;
        public Type type;

        public VariableDeclarationStatement(Type type, string name)
        {
            this.name = name;
            this.type = type;
        }
    }

    public class WhileStatement : Statement
    {
        public Expression condition;
        public Statement body;

        public WhileStatement(Expression condition, Statement body)
        {
            this.condition = condition;
            this.body = body;
        }
    }


    //PROGRAM

    public class Program 
    {
        public List<FunctionDeclaration> decls;

        public Program(List<FunctionDeclaration> decls)
        {
            this.decls = decls;
        }

        public int Line;
        public int Column;
        public void SetLocation(QUT.Gppg.LexLocation loc)
        {
            Line = loc.StartLine;
            Column = loc.StartColumn;
        }

        public Type Accept(Visitior<Type> visitor)
        {
            return visitor.Visit(this);
        }
    }

    //FUNCTION DECLARATION
    public class FunctionDeclaration
    {
        public string name;
        public Type returnType;
        public List<VariableDeclaration> parameters;
        public List<Statement> body;

        public FunctionDeclaration(Type returnType, string name, List<VariableDeclaration> parameters, List<Statement> body)
        {
            this.name = name;
            this.returnType = returnType;
            this.parameters = parameters;
            this.body = body;
        }

        public int Line;
        public int Column;
        public void SetLocation(QUT.Gppg.LexLocation loc)
        {
            Line = loc.StartLine;
            Column = loc.StartColumn;
        }
    }
    


    //VARIABLE DECLARATION
    public class VariableDeclaration
    {
        public string name;
        public Type type;

        public VariableDeclaration(Type type, string name)
        {
            this.name = name;
            this.type = type;
        }

        public int Line;
        public int Column;
        public void SetLocation(QUT.Gppg.LexLocation loc)
        {
            Line = loc.StartLine;
            Column = loc.StartColumn;
        }
    }


}
