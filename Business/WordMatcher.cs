namespace LeonSerensiaExo.Business
{
    public class WordMatcher : IAmTheTest
    {
        /// <summary>
        /// Get suggestions from choices based on their similarity to the term.
        /// </summary>
        /// <param name="term"></param>
        /// <param name="choices"></param>
        /// <param name="numberOfSuggestions"></param>
        /// <returns>a list of choice strings</returns>
        public IEnumerable<string> GetSuggestions(string term, IEnumerable<string> choices, int numberOfSuggestions)
        {
            if (term.Trim() == String.Empty)
                throw new ArgumentException("Term must not be empty.");

            Check(choices);

            if (numberOfSuggestions < 1)
                throw new ArgumentOutOfRangeException("Number of suggestions must be at least 1.");

            return
                choices
                .Where(c => c.Length >= term.Length)
                .Select(c => (choice: c, score: GetBestScoreOf(term, c)))
                .Where(c => c.score < term.Length)
                .OrderBy(c => c.score)
                .ThenBy(c => c.choice.Length)
                .ThenBy(c => c.choice, StringComparer.OrdinalIgnoreCase)
                .Select(c => c.choice)
                .Take(numberOfSuggestions);
        }

        private void Check(IEnumerable<string> choices)
        {
            if (!choices.Any())
                throw new ArgumentException("Choices must not be empty.");
            else if (choices.Any(c => !c.Equals(c.ToLower())))
                throw new ArgumentException("All choices must be in lowercase.");
            else if (choices.Any(c => c.Trim() == String.Empty))
                throw new ArgumentException("Choices must not contain empty words.");
        }

        private int GetBestScoreOf(string src, string container)
        {
            var bestScore = src.Length;

            for (int containerIndex = 0; containerIndex < container.Length; containerIndex++)
            {
                var score = CountDifferencesOf(src, container.Substring(containerIndex));
                if (score < bestScore)
                    bestScore = score;
            }

            return bestScore;
        }

        private int CountDifferencesOf(string src, string container)
        {
            int differenceCount = src.Length;

            int srcIndex = 0;
            while (srcIndex < src.Length && srcIndex < container.Length)
            {
                if (src[srcIndex] == container[srcIndex])
                    differenceCount--;

                srcIndex++;
            }

            return differenceCount;
        }
    }
}
