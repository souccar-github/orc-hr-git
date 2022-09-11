using Souccar.Reflector;

namespace Souccar.CodeGenerator
{
    public interface ICodeGenerator
    {
        void Generate(ClassTree classTree);
    }
}
