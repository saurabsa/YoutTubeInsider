using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouTubeInsider
{
    public class LanguageService
    {
        private static string[] JavaKeywords = { "abstract", "assert", "boolean", "break", "byte", "case", "catch", "char", "class", "const", "continue",
        "default", "do", "double", "else", "enum", "extends", "final", "finally", "float", "for", "goto", "if", "implements",
        "import", "instanceof", "int", "interface", "long", "native", "new", "package", "private", "protected", "public",
        "return", "short", "static", "strictfp", "super", "switch", "synchronized", "this", "throw", "throws", "transient",
        "try", "void", "volatile", "while", "false", "null", "true", "java" };

        private static string[] CSharpKeywords = { "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked",
        "class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else", "enum", "event", "explicit",
        "extern", "false", "finally", "fixed", "float", "for", "foreach", "goto", "if", "implicit", "in", "int", "interface",
        "internal", "is", "lock", "long", "namespace", "new", "null", "object", "operator", "out", "override", "params",
        "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc",
        "static", "string", "struct", "switch", "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked",
        "unsafe", "ushort", "using", "var", "virtual", "void", "volatile", "while", "add", "alias", "get", "global", "partial",
        "remove", "set", "value", "where", "yield", "c#", "cs", "csharp"};

        private static string[] CPlusPlusKeywords = { "alignas", "alignof", "and", "and_eq", "asm", "atomic_cancel", "atomic_commit", "atomic_noexcept", "auto",
        "bitand", "bitor", "bool", "break", "case", "catch", "char", "char16_t", "char32_t", "class", "compl", "concept", "const",
        "constexpr", "const_cast", "continue", "decltype", "default", "delete", "do", "double", "dynamic_cast", "else", "enum", "explicit",
        "export", "extern", "false", "float", "for", "friend", "goto", "if", "inline", "int", "import", "long", "module", "mutable", "namespace",
        "new", "noexcept", "not", "not_eq", "nullptr", "operator", "or", "or_eq", "private", "protected", "public", "register", "reinterpret_cast",
        "requires", "return", "short", "signed", "sizeof", "static", "static_assert", "static_cast", "struct", "switch", "synchronized", "template",
        "this", "thread_local", "throw", "true", "try", "typedef", "typeid", "typename", "union", "unsigned", "using", "virtual", "void", "volatile",
        "wchar_t", "while", "xor", "xor_eq", "override", "final", "transaction_safe", "transaction_safe_dynamic ", "if", "elif", "else",
        "endif", "defined", "ifdef", "ifndef", "define", "undef", "include", "line", "error", "pragma", "cpp"};

        private static string[] CKeywords = { "auto", "break", "case", "char", "const", "continue", "default", "do", "double", "else", "enum", "extern",
        "float", "for", "goto", "if", "int", "long", "register", "return", "short", "signed", "sizeof", "static", "struct", "switch",
        "typedef", "union", "unsigned", "void", "volatile", "while"};

        private static string[] PythonKeywords = { "and", "del", "from", "not", "while", "as", "elif", "global", "or", "with", "assert", "else", "if", "pass",
        "yield", "break", "except", "import", "print", "class", "exec", "in", "raise", "continue", "finally", "is", "return", "def", "for", "lambda",
        "try", "False", "True", "None", "nonlocal", "python"};

        public static string analyzeType(List<string> textWords, string videoName)
        {
            uint javaPoints = 0, cppPoints = 0, cSharpPoints = 0, cPoints = 0, pythonPoints = 0;
            foreach (string word in textWords)
            {
                if(JavaKeywords.Contains(word.ToLower()))
                {
                    javaPoints++;
                }
                if (CSharpKeywords.Contains(word.ToLower()))
                {
                    cSharpPoints++;
                }
                if (CPlusPlusKeywords.Contains(word.ToLower()))
                {
                    cppPoints++;
                }
                if (CKeywords.Contains(word.ToLower()))
                {
                    cPoints++;
                }
                if (PythonKeywords.Contains(word.ToLower()))
                {
                    pythonPoints++;
                }
            }

            List<string> analyzedLanguage = findMaxPoints(javaPoints, cppPoints, cSharpPoints, cPoints, pythonPoints);
            if(analyzedLanguage.Count == 1)
            {
                return analyzedLanguage.First();
            }
            else if (analyzedLanguage.Count == 0)
            {
                return "txt";
            }
            else if (analyzedLanguage.Count > 1)
            {
                if(videoName.ToLower().Contains("visual")
                    || videoName.ToLower().Contains("studio")
                    || videoName.ToLower().Contains("sharp")
                    || videoName.ToLower().Contains("c#"))
                {
                    return "cs";
                }
                else if (videoName.ToLower().Contains("eclipse")
                    || videoName.ToLower().Contains("intellij")
                    || videoName.ToLower().Contains("android")
                    || videoName.ToLower().Contains("java"))
                {
                    return "java";
                }
                else if (videoName.ToLower().Contains("c++")
                    || videoName.ToLower().Contains("cpp")
                    || videoName.ToLower().Contains("plus"))
                {
                    return "cpp";
                }
                else if (videoName.ToLower().Contains("c"))
                {
                    return "c";
                }
                else if (videoName.ToLower().Contains("python"))
                {
                    return "py";
                }
                else
                {
                    return "txt";
                }
            }

            return "txt";
        }

        private static List<string> findMaxPoints(uint javaPoints, uint cppPoints, uint cSharpPoints, uint cPoints, uint pythonPoints)
        {
            uint max = 0;
            if(javaPoints >= max)
            {
                max = javaPoints;
            }
            if (cppPoints >= max)
            {
                max = cppPoints;
            }
            if (cSharpPoints >= max)
            {
                max = cSharpPoints;
            }
            if (cPoints >= max)
            {
                max = cPoints;
            }
            if (pythonPoints >= max)
            {
                max = pythonPoints;
            }

            List<string> analyzedLanguage = new List<string>();
            if(max == javaPoints)
            {
                analyzedLanguage.Add("java");
            }
            if (max == cppPoints)
            {
                analyzedLanguage.Add("cpp");
            }
            if (max == cSharpPoints)
            {
                analyzedLanguage.Add("cs");
            }
            if (max == cPoints)
            {
                analyzedLanguage.Add("c");
            }
            if (max == pythonPoints)
            {
                analyzedLanguage.Add("py");
            }

            return analyzedLanguage;
        }
    }
}
