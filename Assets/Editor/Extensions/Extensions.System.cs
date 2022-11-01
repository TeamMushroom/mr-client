using System.Text;
using TPM;

namespace TPMEditor
{
    public static class StringExtension
    {
        private static readonly char[] PascalCaseToSnakeCaseBlackList =
        {
            '_', '-'
        };

        public static string PascalCaseToSnakeCase(this string @this)
        {
            var stringBuilder = new StringBuilder();

            var isPrevUpper = false;
            var isPrevBlackListCharacter = false;
            for (int i = 0; i < @this.Length; i++)
            {
                var c = @this[i];
                if (!char.IsUpper(c))
                {
                    stringBuilder.Append(c);
                    isPrevUpper = false;
                    isPrevBlackListCharacter = PascalCaseToSnakeCaseBlackList.Contains(c);
                    continue;
                }

                if (i > 0 && !isPrevUpper && !isPrevBlackListCharacter)
                {
                    stringBuilder.Append("_");
                }

                stringBuilder.Append(char.ToLower(c));
                isPrevUpper = true;
                isPrevBlackListCharacter = false;
            }

            return stringBuilder.ToString();
        }

        public static string SnakeCaseToPascalCase(this string @this)
        {
            var stringBuilder = new StringBuilder();

            bool isPrevUnderScore = false;
            for (var i = 0; i < @this.Length; i++)
            {
                var c = @this[i];
                if (c == '_')
                {
                    isPrevUnderScore = true;
                    continue;
                }

                stringBuilder.Append(isPrevUnderScore || i == 0 ? char.ToUpper(c) : c);
                isPrevUnderScore = false;
            }

            return stringBuilder.ToString();
        }
    }
}