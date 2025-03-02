# Custom Compiler in C#

This project implements a custom compiler written in C#. It performs several typical phases of the compilation process, including lexical analysis, parsing, AST construction, and type checking.

---

## Main Features

- **Lexical Analysis**  
  Utilizes a lexer generated with *Gardens Point LEX (GPLEX)* to tokenize the source code.

- **Parsing**  
  Uses a parser generated with *Gardens Point Parser Generator (GPPG)* to analyze the syntactic structure of the code.

- **Abstract Syntax Tree (AST)**  
  Defined in `AbstractSyntax.cs`, the AST represents expressions, declarations, functions, and other language constructs.

- **Type Checking**  
  Implemented in `TypeChecker.cs` using the Visitor pattern to traverse the AST, ensuring semantic correctness and reporting errors with precise line and column information.

- **Error Reporting**  
  The compiler outputs error messages with line and column details to facilitate debugging.

---

## Prerequisites

- **.NET SDK**: Ensure you have .NET (version 5.0 or higher) or Visual Studio installed.
- **Tools Used**:  
  - *Gardens Point LEX (GPLEX)* for generating the lexer.
  - *Gardens Point Parser Generator (GPPG)* for generating the parser.
  - The [QUT.Gppg](https://github.com/QutEcoacoustics/Qut.Gppg) library (or its NuGet package) is included in the project.

---

## Installation and Compilation

1. **Clone the Repository**  
   ```bash
   git clone <REPOSITORY_URL>
   cd <REPOSITORY_NAME>
