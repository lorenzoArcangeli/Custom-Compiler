using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
namespace Parser
{
    public class TypeChecker : Visitior<Type>
    {
        //from variable name to type
        Stack<Dictionary<string, Type>> VarStack = new Stack<Dictionary<string, Type>>();
        //from function name to declaration
        Stack<Dictionary<string, FunctionDeclaration>> FuncCallStack = new Stack<Dictionary<string, FunctionDeclaration>>();


        public TypeChecker()
        {
        }

        public void EnterScope()
        {
            VarStack.Push(new Dictionary<string, Type>());
        }

        void ExitScope()
        {
            VarStack.Pop();
        }

        //if a variable is already typed or not
        bool LookUpValue(string name)
        {
            foreach (var context in VarStack)
                if (context.ContainsKey(name))
                    return true;
            return false;
        }

        //find types of a variable
        Type GetValue(string name)
        {

            foreach (var context in VarStack)
                if (context.ContainsKey(name))
                    return context[name];
            return Type.ERR;
        }


        //if a FunCall is already present
        public bool hasFunction(string name)
        {
            foreach (var state in FuncCallStack)
            {
                if (state.ContainsKey(name))
                {
                    return true;
                }
            }
            return false;
        }

        //return the FunCall
        public FunctionDeclaration LookupFunction(string name)
        {
            foreach (var frame in FuncCallStack)
            {
                if (frame.ContainsKey(name))
                    return frame[name];
            }
            return null;
        }


        void ReportError(string str, Expression expr)
        {
            Console.WriteLine($"// fail {expr.Line} {expr.Column}");
            //Console.WriteLine($"{str} at Line {expr.Line} in column {expr.Column}");
            System.Environment.Exit(1);
        }

        void ReportError(string str, Statement stmt)
        {
            Console.WriteLine($"// fail {stmt.Line} {stmt.Column}");
            System.Environment.Exit(1);

        }

        void ReportError(string str, FunctionDeclaration f)
        {
            Console.WriteLine($"// fail {f.Line} {f.Column + 1}");
            System.Environment.Exit(1);

        }

        Type visitIntegerLiteralExpression(IntegerLiteralExpression expr)
        {
            return Type.INT;
        }

        Type visitBooleanLiteralExpression(BooleanLiteralExpression expr)
        {
            return Type.BOOL;
        }

        Type visitVariableExpression(VariableExpression expr)
        {

            if (LookUpValue(expr.id))
                return GetValue(expr.id);
            else
            {
                ReportError("error id", expr);
                return Type.ERR;
            }
        }

        Type visitUnaryOperatorExpression(UnaryOperatorExpression expr)
        {
            var type = expr.expr.Accept(this);
            if (type.Equals(Type.ERR))
            {
                return type;
            }
            //NEG = "!"
            if (expr.type.Equals(UnaryOperatorExpression.OperatorType.NEG) && (!type.Equals(Type.BOOL)))
            {
                ReportError("error uop-1", expr);
                return Type.ERR;
            }
            if (expr.type == UnaryOperatorExpression.OperatorType.NOT && !type.Equals(Type.INT))
            {
                ReportError("error uop-2", expr);
                return Type.ERR;
            }
            return type;
        }

        Type visitBinaryIntegerOperatorExpression(BinaryOperatorExpression expr)
        {
            var type1 = expr.left.Accept(this);
            if (type1.Equals(Type.ERR))
            {
                return type1;
            }
            if (!type1.Equals(Type.INT))
            {
                ReportError("error bop-1", expr);
                return Type.ERR;
            }
            var type2 = expr.right.Accept(this);
            if (type2.Equals(Type.ERR))
            {
                return type2;
            }
            if (!type2.Equals(Type.INT))
            {
                ReportError("error bop-1", expr);
                return Type.ERR;
            }
            return Type.INT;
        }

        Type visitBinaryOrOperatorExpression(BinaryOperatorExpression expr)
        {
            var type1 = expr.left.Accept(this);
            if (type1.Equals(Type.ERR))
            {
                return type1;
            }
            if (!type1.Equals(Type.BOOL))
            {
                ReportError("error bop-2", expr);
                return Type.ERR;
            }
            var type2 = expr.right.Accept(this);
            if (type2.Equals(Type.ERR))
            {
                return type2;
            }
            if (!type2.Equals(Type.BOOL))
            {
                ReportError("error bop-2", expr);
                return Type.ERR;
            }
            return Type.BOOL;
        }

        Type visitBinaryAndOperatorExpression(BinaryOperatorExpression expr)
        {
            var type1 = expr.left.Accept(this);
            if (type1.Equals(Type.ERR))
            {
                return type1;
            }
            if (!type1.Equals(Type.BOOL))
            {
                ReportError("error bop-3", expr);
                return Type.ERR;
            }
            var type2 = expr.right.Accept(this);
            if (type2.Equals(Type.ERR))
            {
                return type2;
            }
            if (!type2.Equals(Type.BOOL))
            {
                ReportError("error bop-3", expr);
                return Type.ERR;
            }
            return Type.BOOL;
        }

        Type visitBinaryEqualityOperatorExpression(BinaryOperatorExpression expr)
        {
            var type1 = expr.left.Accept(this);
            if (type1.Equals(Type.ERR))
            {
                return type1;
            }
            var type2 = expr.right.Accept(this);
            if (type2.Equals(Type.ERR))
            {
                return type2;
            }
            if (!type1.Equals(type2))
            {
                ReportError("error bop-4", expr);
                return Type.ERR;
            }
            return Type.BOOL;
        }

        Type visitBinaryInequalityOperatorExpression(BinaryOperatorExpression expr)
        {
            var type1 = expr.left.Accept(this);
            if (type1.Equals(Type.ERR))
            {
                return type1;
            }
            if (!type1.Equals(Type.INT))
            {
                ReportError("error bop-5", expr);
                return Type.ERR;
            }
            var type2 = expr.right.Accept(this);
            if (type2.Equals(Type.ERR))
            {
                return type2;
            }
            if (!type2.Equals(Type.INT))
            {
                ReportError("error bop-5", expr);
                return Type.ERR;
            }
            return Type.BOOL;
        }

        Type visitAssignmentExpression(AssignmentExpression expr)
        {
            var id = expr.identifier;
            var type = expr.right.Accept(this);
            if (type.Equals(Type.ERR))
            {
                return type;
            }
            if (type.Equals(Type.VOID))
            {
                ReportError("error asn-1", expr);
                return Type.ERR;
            }
            if (!type.Equals(GetValue(id)))
            {
                ReportError("error asn-2", expr);
                return Type.ERR;
            }
            return type;
        }

        Type visitFunctionalCallExpression(FunctionCallExpression expr)
        {
            var id = expr.identifier;
            if (id != "print")
            {
                List<Expression> exprs = expr.arguments;
                if (!hasFunction(id))
                {
                    ReportError("call-1", expr);
                    return Type.ERR;
                }
                var fun = LookupFunction(id);
                List<VariableDeclaration> args = ((FunctionDeclaration)fun).parameters;
                if (args.Count != exprs.Count)
                {
                    ReportError("call-2", expr);
                    return Type.ERR;
                }
                for (int i = 0; i < exprs.Count; i++)
                {
                    var type = exprs[i].Accept(this);
                    if (type.Equals(Type.ERR))
                    {
                        return type;
                    }
                    if (!(type.Equals(args[i].type)))
                    {
                        ReportError("call-3", expr);
                        return Type.ERR;
                    }
                }
                return fun.returnType;
            }
            else
            {
                List<Expression> exprs = expr.arguments;
                for (int i = 0; i < exprs.Count; i++)
                {
                    var type = exprs[i].Accept(this);
                    if (type.Equals(Type.ERR))
                    {
                        return type;
                    }
                }
                return Type.VOID;

            }
        }

        public Type Visit(Expression exp)
        {
            Type t = Type.ERR;
            if (exp is IntegerLiteralExpression)
                t = this.visitIntegerLiteralExpression((IntegerLiteralExpression)exp);
            if (exp is BooleanLiteralExpression)
                t = this.visitBooleanLiteralExpression((BooleanLiteralExpression)exp);
            if (exp is VariableExpression)
                t = this.visitVariableExpression((VariableExpression)exp);
            if (exp is UnaryOperatorExpression)
                t = this.visitUnaryOperatorExpression((UnaryOperatorExpression)exp);
            if (exp is BinaryOperatorExpression)
            {
                BinaryOperatorExpression.OperatorType oper = ((BinaryOperatorExpression)exp).op;
                if (oper.Equals(BinaryOperatorExpression.OperatorType.ADD) ||
                    oper.Equals(BinaryOperatorExpression.OperatorType.SUB) ||
                    oper.Equals(BinaryOperatorExpression.OperatorType.MUL) ||
                    oper.Equals(BinaryOperatorExpression.OperatorType.DIV))
                    t = this.visitBinaryIntegerOperatorExpression((BinaryOperatorExpression)exp);
                if (oper.Equals(BinaryOperatorExpression.OperatorType.OR))
                    t = this.visitBinaryOrOperatorExpression((BinaryOperatorExpression)exp);
                if (oper.Equals(BinaryOperatorExpression.OperatorType.AND))
                    t = this.visitBinaryAndOperatorExpression((BinaryOperatorExpression)exp);
                if (oper.Equals(BinaryOperatorExpression.OperatorType.EQ) ||
                    oper.Equals(BinaryOperatorExpression.OperatorType.NEQ))
                    t = this.visitBinaryEqualityOperatorExpression((BinaryOperatorExpression)exp);
                if (oper.Equals(BinaryOperatorExpression.OperatorType.LES) ||
                    oper.Equals(BinaryOperatorExpression.OperatorType.GRE) ||
                    oper.Equals(BinaryOperatorExpression.OperatorType.LESE) ||
                    oper.Equals(BinaryOperatorExpression.OperatorType.GREE))
                    t = this.visitBinaryInequalityOperatorExpression((BinaryOperatorExpression)exp);
            }
            if (exp is AssignmentExpression)
                t = this.visitAssignmentExpression((AssignmentExpression)exp);
            if (exp is FunctionCallExpression)
                t = this.visitFunctionalCallExpression((FunctionCallExpression)exp);
            return t;
        }


        public Type Visit(Statement stmt, Type funReturnType)
        {
            Type t = Type.ERR;
            if (stmt is BlockStatement)
                t = visitBlockStament((BlockStatement)stmt, funReturnType);
            if (stmt is ReturnStatement)
                t = visitReturnStatement((ReturnStatement)stmt, funReturnType);
            if (stmt is VariableDeclarationStatement)
                t = visitVariableDeclarationStatement((VariableDeclarationStatement)stmt, funReturnType);
            if (stmt is ExpressionStatement)
                t = visitExpressionStatement((ExpressionStatement)stmt, funReturnType);
            if (stmt is IfStatement)
                t = visitIfStatement((IfStatement)stmt, funReturnType);
            if (stmt is WhileStatement)
                t = visitWhileStatement((WhileStatement)stmt, funReturnType);
            return t;
        }

        public Type Visit(Program program)
        {
            return vitisProgram(program);
        }

        public Type vitisProgram(Program program)
        {
            List<FunctionDeclaration> decls = program.decls;
            //first add all the function that I have in the program
            foreach (FunctionDeclaration f in decls)
            {
                string name = f.name;
                Dictionary<string, FunctionDeclaration> functionDictionary = new Dictionary<string, FunctionDeclaration>();
                functionDictionary.Add(name, f);
                FuncCallStack.Push(functionDictionary);
            }
            //check each function
            foreach (FunctionDeclaration f in decls)
            {

                List<VariableDeclaration> args = f.parameters;
                //push all the variables in the signature of the function
                EnterScope();
                foreach (VariableDeclaration p in args)
                {
                    VarStack.Peek().Add(p.name, p.type);
                }
                BlockStatement body = new BlockStatement(f.body);
                Type type = body.Accept(this, f.returnType);
                //At this point all the error are handle by the visit method excpet for the empty block statement ERR
                //since it does not have location so I need to use the location of the function
                if (type.Equals(Type.ERR))
                {
                    ReportError("", f);
                    return type;
                }
            }
            return Type.OK;
        }

        public Type visitBlockStament(BlockStatement block, Type funReturnType)
        {
            List<Statement> statements = block.body;
            EnterScope();
            foreach (Statement s in statements)
            {
                //first: type check
                Type type = s.Accept(this, funReturnType);
                if (type.Equals(Type.ERR))
                    return type;
                //if is a return I have to skip the other statements
                if (s is ReturnStatement)
                    break;
            }
            if (block.body.Count == 0 && (!funReturnType.Equals(Type.VOID)))
            {
                return Type.ERR;
            }
            ExitScope();
            return Type.OK;
        }

        public Type visitReturnStatement(ReturnStatement returnStatement, Type funReturnType)
        {
            if (returnStatement.expr == null && (!funReturnType.Equals(Type.VOID)))
            {
                ReportError("error ret", returnStatement);
                return Type.ERR;
            }
            if (returnStatement.expr == null && (funReturnType.Equals(Type.VOID)))
                return Type.OK;
            Expression expr = returnStatement.expr;
            Type type = expr.Accept(this);
            if (type.Equals(Type.ERR))
                return type;
            if (!type.Equals(funReturnType))
            {
                ReportError("error ret", returnStatement);
                return Type.ERR;
            }
            return Type.OK;
        }

        public Type visitVariableDeclarationStatement(VariableDeclarationStatement variableDeclarationStatement, Type funReturnType)
        {
            Type type = variableDeclarationStatement.type;
            string name = variableDeclarationStatement.name;
            if (LookUpValue(name))
            {
                ReportError("error decl", variableDeclarationStatement);
                return Type.ERR;
            }
            //Dictionary<string, Type> newVariable = new Dictionary<string, Type>();
            //newVariable.Add(name, type);
            //VarStack..Push(newVariable);
            VarStack.Peek().Add(name, type);
            //StackVarStack.Peek().Push(newVariable);
            return Type.OK;
        }

        public Type visitIfStatement(IfStatement ifStatement, Type funReturnType)
        {
            Expression condition = ifStatement.condition;
            Type type = condition.Accept(this);
            if (type.Equals(Type.ERR))
                return type;
            if (!type.Equals(Type.BOOL))
            {
                ReportError("error if", ifStatement);
                return Type.ERR;
            }
            Statement stmt1 = ifStatement.prim;
            Type type1 = stmt1.Accept(this, funReturnType);
            if (type1.Equals(Type.ERR))
                return type1;
            if (ifStatement.alternative != null)
            {
                Statement stmt2 = ifStatement.alternative;
                Type type2 = stmt2.Accept(this, funReturnType);
                if (type2.Equals(Type.ERR))
                    return type2;
            }
            return Type.OK;
        }

        public Type visitWhileStatement(WhileStatement whileStatement, Type funReturnType)
        {
            Expression condition = whileStatement.condition;
            Type type = condition.Accept(this);
            if (type.Equals(Type.ERR))
                return type;
            if (!type.Equals(Type.BOOL))
            {
                ReportError("error while", whileStatement);
                return Type.ERR;
            }
            Statement body = whileStatement.body;
            Type typeBody = body.Accept(this, funReturnType);
            if (typeBody.Equals(Type.ERR))
                return typeBody;

            return Type.OK;
        }

        public Type visitExpressionStatement(ExpressionStatement expressionStatement, Type funReturnType)
        {
            Expression expr = expressionStatement.expr;
            Type type = expr.Accept(this);
            if (type.Equals(Type.ERR))
                return type;
            return Type.OK;
        }
    }
}
