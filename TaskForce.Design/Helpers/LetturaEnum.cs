using System.Text.RegularExpressions;

namespace TaskForce.Design.Helpers
{
    public static class LetturaEnum
    {
        public static string Lettura(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var result = Regex.Replace(input, "([a-z])([A-Z])", "$1 $2");

            var sostituzioni = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Conformita", "Conformità" },
            };

            var parole = result.Split(' ');
            for (int i = 0; i < parole.Length; i++)
            {
                if (sostituzioni.TryGetValue(parole[i], out var sostituita))
                {
                    parole[i] = sostituita;
                }
            }

            if (input == "AllVII")
                return "All-VII";

            if (input == "Mod1B")
                return "Mod-1B";

            var final = string.Join(' ', parole);
            return char.ToUpper(final[0]) + final[1..].ToLower();
        }
    }
}
