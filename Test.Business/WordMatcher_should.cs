using LeonSerensiaExo.Business;

namespace LeonSerensia.Test
{
    public class WordMatcher_should
    {
        private IAmTheTest _wordHelper;

        public WordMatcher_should()
        {
            _wordHelper = new WordMatcher();
        }

        [Fact]
        public void Get_suggestions_with_a_choices_contained_word()
        {
            var term = "gros";
            var choices = new List<string> { "gros", "gras", "graisse", "agressif", "go", "ros", "gro" };

            var suggestions = 2;
            var result = _wordHelper.GetSuggestions(term, choices, suggestions);
            Assert.NotEqual(result, new List<string> { "anything", "anotherthing" });
            Assert.Equal(new List<string> { "gros", "gras" }, result);

            suggestions = 4;
            result = _wordHelper.GetSuggestions(term, choices, suggestions);
            Assert.Equal(new List<string> { "gros", "gras", "agressif", "graisse" }, result);

            choices = new List<string> { "gros", "gr2as", "graisse", "agressif", "go", "ros", "gro" };
            result = _wordHelper.GetSuggestions(term, choices, suggestions);
            Assert.Equal(new List<string> { "gros", "agressif", "gr2as", "graisse" }, result);
        }

        [Fact]
        public void Get_suggestions_with_a_choices_uncontained_word()
        {
            var term = "gris";
            var choices = new List<string> { "gros", "gras", "graisse", "pasagressif", "go", "ros", "gro" };

            var suggestions = 2;
            var result = _wordHelper.GetSuggestions(term, choices, suggestions);
            Assert.Equal(new List<string> { "gras", "gros" }, result);

            suggestions = 4;
            result = _wordHelper.GetSuggestions(term, choices, suggestions);
            Assert.Equal(new List<string> { "gras", "gros", "pasagressif", "graisse" }, result);
        }

        [Fact]
        public void Get_suggestions_with_just_one_letter_in_common()
        {
            var term = "zrt";
            var choices = new List<string> { "gros", "gras", "graisse", "pasagressif", "go", "ros", "gro" };

            var suggestions = 2;
            var result = _wordHelper.GetSuggestions(term, choices, suggestions);
            Assert.Equal(new List<string> { "gro", "gras" }, result);

            suggestions = 4;
            result = _wordHelper.GetSuggestions(term, choices, suggestions);
            Assert.Equal(new List<string> { "gro", "gras", "gros", "graisse" }, result);
        }

        [Theory]
        [InlineData("unpluslongmotquetous")]
        [InlineData("zkt")]
        [InlineData("z")]
        public void Get_no_suggestions(string term)
        {
            var choices = new List<string> { "gros", "gras", "graisse", "pasagressif", "go", "ros", "gro" };
            var suggestions = 2;

            var result = _wordHelper.GetSuggestions(term, choices, suggestions);
            Assert.Empty(result);
        }

        [Fact]
        public void Throw_argument_out_of_range_exception_on_empty_word()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _wordHelper.GetSuggestions("", new string[] { "choice", "choice2" }, 2);
            });
        }

        [Fact]
        public void Throw_argument_exception_on_empty_choices()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _wordHelper.GetSuggestions("anything", new string[] { }, 2);
            });
        }
    }
}
